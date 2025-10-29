using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
//using QuestPDF;

namespace FridgeManagementSystem.Areas.MaintenanceSubSystem.Controllers
{
    [Area("MaintenanceSubSystem")]
    public class MaintenanceController : Controller
    {
        private readonly ILogger<MaintenanceController> _logger;
        private readonly FridgeDbContext _context;
        private readonly IMaintenanceRequestService _mrService;
        public MaintenanceController(FridgeDbContext context, IMaintenanceRequestService mrService, ILogger<MaintenanceController> logger)
        {
            _context = context;
            _mrService = mrService;
            _logger = logger;
        }

        // ✅ Helper property – only Active Maintenance Technicians
        private IQueryable<Employee> MaintenanceTechnicians =>
            _context.Employees
                .Where(e => e.Role == EmployeeRoles.MaintenanceTechnician && e.Status == "Active");

        // ✅ Utility: keep request + visit statuses in sync
        private void UpdateVisitAndRequestStatus(MaintenanceVisit visit, Models.TaskStatus status)
        {
            if (visit == null) return;

            // Update visit status
            visit.Status = status;
            if (_context.Entry(visit).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
                _context.MaintenanceVisit.Attach(visit);

            _context.Entry(visit).Property(v => v.Status).IsModified = true;

            // Update linked request
            var request = visit.MaintenanceRequest ?? _context.MaintenanceRequest
                            .FirstOrDefault(r => r.MaintenanceRequestId == visit.MaintenanceRequestId);

            if (request != null)
            {
                request.TaskStatus = status;

                if (status == Models.TaskStatus.Complete)
                {
                    request.CompletedDate = DateTime.Now;
                    request.IsActive = false;
                }
                else
                {
                    request.IsActive = true; // for scheduled/in-progress requests
                }

                if (_context.Entry(request).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
                    _context.MaintenanceRequest.Attach(request);

                _context.Entry(request).Property(r => r.TaskStatus).IsModified = true;
                _context.Entry(request).Property(r => r.IsActive).IsModified = true;

                if (status == Models.TaskStatus.Complete)
                    _context.Entry(request).Property(r => r.CompletedDate).IsModified = true;
            }

            _context.SaveChanges();
        }

        // ✅ Show all active requests
        // ✅ Show requests with filter
        public async Task<IActionResult> MaintenanceRequests(string statusFilter)
        {
            var requestsQuery = _context.MaintenanceRequest
                .Include(r => r.Fridge)
                    .ThenInclude(f => f.Customer)
                        .ThenInclude(c => c.Location)
                .Where(r => r.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                if (Enum.TryParse<Models.TaskStatus>(statusFilter, out var parsedStatus))
                {
                    requestsQuery = requestsQuery.Where(r => r.TaskStatus == parsedStatus);
                }
            }

            var requests = await requestsQuery
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            ViewBag.SelectedStatus = statusFilter ?? "All";

            return View(requests);
        }


        //// ✅ Search requests
        //public async Task<IActionResult> Search(string query)
        //{
        //    var filterRequests = _context.MaintenanceRequest
        //        .Include(r => r.Fridge)
        //            .ThenInclude(f => f.Customer)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(query))
        //    {
        //        filterRequests = filterRequests.Where(r =>
        //            r.Fridge.Brand.Contains(query) ||
        //            r.Fridge.Customer.FullName.Contains(query) ||
        //            r.Fridge.Customer.Location.Address.Contains(query) ||
        //            r.Fridge.Model.Contains(query));
        //    }

        //    return View("MaintenanceRequests", await filterRequests.ToListAsync());
        //}
        [HttpGet]
        public IActionResult GetTechnicianSchedule()
        {
            var visits = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                            .ThenInclude(c => c.Location)
                .Where(v => v.Status == Models.TaskStatus.Scheduled || v.Status == Models.TaskStatus.Rescheduled)
                .AsEnumerable()
                .Select(v => new
                {
                    id = v.MaintenanceVisitId,
                    title = v.MaintenanceRequest.Fridge?.Brand ?? "Unknown",
                    start = $"{v.ScheduledDate:yyyy-MM-dd}T{v.ScheduledTime:hh\\:mm}",  // ✅ Add Time
                    end = $"{v.ScheduledDate:yyyy-MM-dd}T{(v.ScheduledTime + TimeSpan.FromHours(1)):hh\\:mm}", // ✅ Auto 1-hr duration
                    allDay = false,
                    backgroundColor = "#3498db",
                    borderColor = "#2980b9",
                    textColor = "#fff",
                    extendedProps = new
                    {
                        time = v.ScheduledTime.ToString(@"hh\:mm"),   // ✅ FIX — Send the time
                        date = v.ScheduledDate.ToString("yyyy-MM-dd"),
                        fridge = v.MaintenanceRequest.Fridge?.Brand ?? "Unknown Fridge",
                        customer = v.MaintenanceRequest.Fridge?.Customer?.FullName ?? "Unknown Customer",
                        address = v.MaintenanceRequest.Fridge?.Customer?.Location?.Address ?? "Address N/A"
                    }
                })
                .ToList();

            return Json(visits);
        }


        private static string GetStatusColor(Models.TaskStatus status)
        {
            return status switch
            {
                Models.TaskStatus.Scheduled => "#3498db",    // Blue
                Models.TaskStatus.Rescheduled => "#f39c12",  // Orange
                Models.TaskStatus.InProgress => "#2ecc71",   // Green
                Models.TaskStatus.Complete => "#27ae60",    // Dark Green
                Models.TaskStatus.Cancelled => "#e74c3c",    // Red
                _ => "#95a5a6"                        // Gray
            };
        }

        public IActionResult ScheduleVisit()
        {
            PopulateRequestsViewBag();
            // return an empty visit with defaults so asp-for works in view
            return View(new MaintenanceVisit
            {
                ScheduledDate = DateTime.Today,
                ScheduledTime = DateTime.Now.TimeOfDay
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScheduleVisit(MaintenanceVisit model)
        {
            PopulateRequestsViewBag();

            // 1) Server-side validation
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please provide all required fields.";
                return View(model);
            }

            // 2) Load the request with related fridge/customer/location
            var request = _context.MaintenanceRequest
                .Include(r => r.Fridge).ThenInclude(f => f.Customer).ThenInclude(c => c.Location)
                .FirstOrDefault(r => r.MaintenanceRequestId == model.MaintenanceRequestId && r.IsActive);

            if (request == null)
            {
                TempData["Error"] = "Selected request does not exist or is not active.";
                return View(model);
            }

            // 3) Robust duplicate check: only block if an active visit already exists
            //    (Scheduled or InProgress are considered active; Completed/Cancelled are NOT)
            var existingActiveVisit = _context.MaintenanceVisit
                .Any(v => v.MaintenanceRequestId == request.MaintenanceRequestId &&
                         (v.Status == Models.TaskStatus.Scheduled ||
                          v.Status == Models.TaskStatus.InProgress));

            if (existingActiveVisit)
            {
                TempData["Error"] = "A maintenance visit is already scheduled or in progress for this request.";
                return View(model);
            }

            // 4) Map fridge/request ids
            model.FridgeId = request.FridgeId;
            model.MaintenanceRequestId = request.MaintenanceRequestId;

            // 5) Assign technician
            var technician = MaintenanceTechnicians.FirstOrDefault();
            if (technician == null)
            {
                TempData["Error"] = "No maintenance technician available. Please add one first.";
                return View(model);
            }
            model.EmployeeID = technician.EmployeeID;

            // 6) Set visit status and save
            model.Status = Models.TaskStatus.Scheduled;
            _context.MaintenanceVisit.Add(model);
            _context.SaveChanges();

            // 7) Update the request to Scheduled via the helper so IsActive remains true
            UpdateVisitAndRequestStatus(model, Models.TaskStatus.Scheduled);

            TempData["Message"] = "Maintenance visit scheduled successfully!";
            return RedirectToAction(nameof(ScheduleVisit));
        }

        // Helper: populate dropdown & JSON safely
        private void PopulateRequestsViewBag()
        {
            // 1️⃣ Get all active, pending requests
            var requests = _context.MaintenanceRequest
                .Include(r => r.Fridge)
                    .ThenInclude(f => f.Customer)
                        .ThenInclude(c => c.Location)
                .Where(r => r.IsActive && r.TaskStatus == Models.TaskStatus.Pending)
                .ToList();

            // 2️⃣ Prepare the dropdown display and JSON for JS
            var items = requests.Select(r => new
            {
                Id = r.MaintenanceRequestId,
                Display = $"{r.Fridge?.Brand ?? "N/A"} - {r.Fridge?.Model ?? "N/A"} | {r.Fridge?.Customer?.FullName ?? "N/A"} ({r.Fridge?.Customer?.Location?.City ?? "N/A"})",
                Customer = r.Fridge?.Customer?.FullName ?? "N/A",
                Address = r.Fridge?.Customer?.Location?.Address ?? "N/A",
                Model = r.Fridge?.Model ?? "N/A"
            }).ToList();

            ViewBag.Requests = new SelectList(items, "Id", "Display");

            ViewBag.RequestsJson = Newtonsoft.Json.JsonConvert.SerializeObject(
                items.Select(i => new { id = i.Id, customer = i.Customer, customerAddress = i.Address, model = i.Model })
            );

            // 3️⃣ Debug log (optional)
            System.Diagnostics.Debug.WriteLine($"[PopulateRequestsViewBag] pendingRequests={items.Count}");
            foreach (var it in items)
                System.Diagnostics.Debug.WriteLine($"[Request] id={it.Id} disp={it.Display}");
        }

        // ✅ Show visits
        public IActionResult VisitList()
        {
            var visits = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                .Include(v => v.Employee)
                .Include(v => v.FaultReport)
                .Where(v => v.MaintenanceRequest.TaskStatus == Models.TaskStatus.Scheduled
                         || v.MaintenanceRequest.TaskStatus == Models.TaskStatus.Rescheduled
                         || v.MaintenanceRequest.TaskStatus == Models.TaskStatus.InProgress)
                .OrderBy(v => v.ScheduledDate)
                .ThenBy(v => v.ScheduledTime)
                .ToList();

            return View(visits);
        }

        // ✅ Reschedule
        [HttpPost]
        public IActionResult RescheduleVisit(int visitId, DateTime newDate, TimeSpan newTime)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null)
                return Json(new { success = false, message = "Visit not found." });

            visit.ScheduledDate = newDate;
            visit.ScheduledTime = newTime;

            UpdateVisitAndRequestStatus(visit, Models.TaskStatus.Rescheduled);

            return Json(new { success = true, message = "Visit rescheduled successfully!" });
        }

        // ✅ Cancel
        [HttpPost]
        public IActionResult CancelVisit(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null)
                return Json(new { success = false, message = "Visit not found." });

            UpdateVisitAndRequestStatus(visit, Models.TaskStatus.Cancelled);

            return Json(new { success = true, message = "Visit cancelled successfully!" });
        }

        // ✅ Perform Maintenance
        public IActionResult PerformMaintenance(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                          .ThenInclude(c=>c.Location)
                .Include(v => v.Employee)
                .Include(v => v.FaultReport)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null)
            {
                TempData["Error"] = "Scheduled visit not found.";
                return RedirectToAction("VisitList");
            }

            if (visit.MaintenanceRequest?.Fridge == null)
            {
                TempData["Error"] = "This visit is missing a linked fridge.";
                return RedirectToAction("VisitList");
            }

            ViewBag.ChecklistDone = visit.MaintenanceChecklist != null;
            return View(visit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartMaintenance(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null) return NotFound();

            UpdateVisitAndRequestStatus(visit, Models.TaskStatus.InProgress);

            TempData["Message"] = "Maintenance task started successfully!";
            return RedirectToAction("PerformMaintenance", new { visitId });
        }
            [HttpPost]
            public async Task<IActionResult> CompleteMaintenance(int visitId)
            {
                var visit = await _context.MaintenanceVisit
                    .Include(v => v.MaintenanceRequest)
                        .ThenInclude(r => r.Fridge)
                    .Include(v => v.Employee)
                    .FirstOrDefaultAsync(v => v.MaintenanceVisitId == visitId);

                if (visit == null) return NotFound();

                // ✅ Update status and linked request via helper
                UpdateVisitAndRequestStatus(visit, Models.TaskStatus.Complete);

                // 4) Create next monthly MaintenanceRequest (service prevents duplicates)
                var nextRequest = await _mrService.CreateNextMonthlyRequestAsync(visit.FridgeId);

                MaintenanceVisit nextVisit = null;
                if (nextRequest != null)
                {
                nextVisit = new MaintenanceVisit
                {
                    MaintenanceRequestId = nextRequest.MaintenanceRequestId,
                    FridgeId = visit.FridgeId,
                    ScheduledDate = nextRequest.RequestDate ?? DateTime.Today, // fallback to today
                    ScheduledTime = visit.ScheduledTime,
                    Status = Models.TaskStatus.Scheduled,
                    EmployeeID = visit.EmployeeID
                };

                _context.MaintenanceVisit.Add(nextVisit);
                await _context.SaveChangesAsync();

                // ✅ Ensure request gets status updated properly
                UpdateVisitAndRequestStatus(nextVisit, Models.TaskStatus.Scheduled);
                await _context.SaveChangesAsync();

            }

            // Feedback
            if (nextVisit != null)
                    TempData["Message"] = $"Maintenance completed. Next visit scheduled for {nextVisit.ScheduledDate:yyyy-MM-dd}.";
                else if (nextRequest != null)
                    TempData["Message"] = $"Maintenance completed. Next request created for {nextRequest.RequestDate:yyyy-MM-dd}.";
                else
                    TempData["Message"] = "Maintenance completed. No new request created (already a pending/scheduled request).";

                return RedirectToAction("PerformMaintenance", new { visitId = visitId });
            }


        // ✅ Checklist
        public IActionResult MaintenanceChecklist(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null)
                return NotFound();

            var checklist = _context.MaintenanceChecklist
                .FirstOrDefault(c => c.MaintenanceVisitId == visitId)
                ?? new MaintenanceChecklist { MaintenanceVisitId = visitId };

            return View(checklist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveChecklist(MaintenanceChecklist checklist)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return View("MaintenanceChecklist", checklist);
            }

            var existing = _context.MaintenanceChecklist
                .FirstOrDefault(c => c.MaintenanceVisitId == checklist.MaintenanceVisitId);

            if (existing != null)
            {
                existing.TemperatureStatus = checklist.TemperatureStatus;
                existing.CoolantLevel = checklist.CoolantLevel;
                existing.DoorSealCondition = checklist.DoorSealCondition;
                existing.LightingStatus = checklist.LightingStatus;
                existing.PowerCableCondition = checklist.PowerCableCondition;
                existing.CondenserCoilsCleaned = checklist.CondenserCoilsCleaned;
            }
            else
            {
                _context.MaintenanceChecklist.Add(checklist);
            }

            _context.SaveChanges();

            TempData["Message"] = "Checklist saved successfully!";
            return RedirectToAction("PerformMaintenance", new { visitId = checklist.MaintenanceVisitId });
        }

        // ✅ Components Used
        public IActionResult ComponentsUsed(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                          .ThenInclude(c=>c.Location)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null)
                return NotFound();

            return View(visit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveComponents(int visitId, List<ComponentUsed> components)
        {
            if (components != null && components.Any())
            {
                foreach (var component in components)
                {
                    component.MaintenanceVisitId = visitId;
                    _context.ComponentUsed.Add(component);
                }

                _context.SaveChanges();
                TempData["Message"] = "Components saved successfully!";
            }
            else
            {
                TempData["Error"] = "No components to save.";
            }

            return RedirectToAction("PerformMaintenance", new { visitId });
        }
        // GET: Add/Edit Visit Notes
        public IActionResult VisitNotes(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null)
                return NotFound();

            return View(visit);
        }

        // POST: Save Visit Notes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveVisitNotes(int visitId, string visitNotes)
        {
            var visit = _context.MaintenanceVisit.FirstOrDefault(v => v.MaintenanceVisitId == visitId);
            if (visit == null)
                return NotFound();

            visit.VisitNotes = visitNotes;
            _context.SaveChanges();

            TempData["Message"] = "Visit notes saved successfully!";
            return RedirectToAction("PerformMaintenance", new { visitId });
        }

        // ✅ Fault Report
        public IActionResult CreateFaultReport(int visitId, int fridgeId)
        {
            var fridge = _context.Fridge
                .Include(f => f.Customer)
                    .ThenInclude(c=>c.Location)
                .FirstOrDefault(f => f.FridgeId == fridgeId);

            var visit = _context.MaintenanceVisit
                .Include(v => v.Employee)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (fridge == null || visit == null)
                return NotFound();

            var report = new FaultReport
            {
                MaintenanceVisitId = visitId,
                FridgeId = fridgeId,
                ReportDate = DateTime.Now
            };

            ViewBag.EmployeeName = visit.Employee?.FullName;
            ViewBag.CustomerName = fridge.Customer?.FullName;
            ViewBag.Address = fridge.Customer?.Location?.Address;
            ViewBag.FridgeModel = fridge.Model;

            return View(report);
        }

        [HttpPost]
        public IActionResult SaveFaultReport(FaultReport report)
        {
            if (!ModelState.IsValid)
            {
                var fridge = _context.Fridge.Include(f => f.Customer)
                    .FirstOrDefault(f => f.FridgeId == report.FridgeId);

                var visit = _context.MaintenanceVisit.Include(v => v.Employee)
                    .FirstOrDefault(v => v.MaintenanceVisitId == report.MaintenanceVisitId);

                ViewBag.EmployeeName = visit?.Employee?.FullName;
                ViewBag.CustomerName = fridge?.Customer?.FullName;
                ViewBag.Address = fridge?.Customer?.Location?.Address;
                ViewBag.FridgeModel = fridge?.Model;

                return View("CreateFaultReport", report);
            }
            report.StatusFilter = "Pending";
            _context.FaultReport.Add(report);
            _context.SaveChanges();

            TempData["Message"] = "Fault has been logged successfully!";
            return RedirectToAction("PerformMaintenance", new { visitId = report.MaintenanceVisitId });
        }

        public IActionResult ReportedFaults()
        {
            var faults = _context.FaultReport
                .Include(f => f.Fridge)
                    .ThenInclude(f => f.Customer)
                .Include(f => f.MaintenanceVisit)
                    .ThenInclude(v => v.Employee)
                .OrderByDescending(f => f.ReportDate)
                .ToList();

            return View(faults);
        }

        // ✅ Service History
        public IActionResult ServiceHistory(int fridgeId)
        {
            var visits = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f=>f.Customer)
                          .ThenInclude(c=>c.Location)
                 .Include(v=>v.Employee)
                .Include(v => v.MaintenanceChecklist)
                .Include(v => v.ComponentUsed)
                .Include(v => v.FaultReport)
               .Where(v =>
    v.MaintenanceRequest.FridgeId == fridgeId &&
    v.Status == Models.TaskStatus.Complete &&
    v.MaintenanceRequest.CompletedDate != null)
                .OrderByDescending(v => v.ScheduledDate)
                .ToList();

            return View(visits);
        }

        // ✅ PDF Export
        [HttpGet]
        public IActionResult DownloadServiceHistory(int fridgeId)
        {
            var visits = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                    .ThenInclude(r => r.Fridge)
                        .ThenInclude(f => f.Customer)
                          .ThenInclude(c => c.Location)
                 .Include(v => v.Employee)
                .Include(v => v.MaintenanceChecklist)
                .Include(v => v.ComponentUsed)
                .Include(v => v.FaultReport)
                .Where(v => v.MaintenanceRequest.FridgeId == fridgeId)
                .OrderByDescending(v => v.ScheduledDate)
                .ToList();

            if (!visits.Any())
                return NotFound("No service history for that fridge.");

            var fridge = visits.First().MaintenanceRequest.Fridge;
            var generator = new ServiceHistoryPdfGenerator(visits, fridge);

            var pdfBytes = generator.GeneratePdf();
            var fileName = $"ServiceHistory_{fridge.Brand}_{DateTime.Now:yyyyMMdd}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
