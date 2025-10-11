using FridgeManagementSystem.Data;
using FridgeManagementSystem.Models;
using FridgeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
//using QuestPDF;

namespace FridgeManagementSystem.Areas.MaintenanceSubSytem.Controllers
{
    [Area("MaintenanceSubSystem")]
    public class MaintenanceController : Controller
    {
        private readonly FridgeDbContext _context;
        private readonly IMaintenanceRequestService _mrService;
        public MaintenanceController(FridgeDbContext context, IMaintenanceRequestService mrService)
        {
            _context = context;
            _mrService = mrService;
        }

        // ✅ Helper property – only Active Maintenance Technicians
        private IQueryable<Employee> MaintenanceTechnicians =>
            _context.Employees
                .Where(e => e.Role == EmployeeRoles.MaintenanceTechnician && e.Status == "Active");

        // ✅ Utility: keep request + visit statuses in sync
        private void UpdateVisitAndRequestStatus(MaintenanceVisit visit, Models.TaskStatus status)
        {
            visit.Status = status;

            if (visit.MaintenanceRequest != null)
            {
                visit.MaintenanceRequest.TaskStatus = status;
            }
            else
            {
                var request = _context.MaintenanceRequest
                    .FirstOrDefault(r => r.MaintenanceRequestId == visit.MaintenanceRequestId);
                if (request != null)
                    request.TaskStatus = status;
            }

            _context.SaveChanges();
        }

        // ✅ Show all active requests
        public async Task<IActionResult> MaintenanceRequests()
        {
            var requests = await _context.MaintenanceRequest
                .Include(r => r.Fridge)
                    .ThenInclude(f => f.Customer)
                       .ThenInclude(c=>c.Location)
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return View(requests);
        }

        // ✅ Search requests
        public async Task<IActionResult> Search(string query)
        {
            var filterRequests = _context.MaintenanceRequest
                .Include(r => r.Fridge)
                    .ThenInclude(f => f.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                filterRequests = filterRequests.Where(r =>
                    r.Fridge.Brand.Contains(query) ||
                    r.Fridge.Customer.FullName.Contains(query) ||
                    r.Fridge.Customer.Location.Address.Contains(query) ||
                    r.Fridge.Model.Contains(query));
            }

            return View("MaintenanceRequests", await filterRequests.ToListAsync());
        }

        // ✅ GET: Schedule Visit
        public IActionResult ScheduleVisit()
        {
            // Only show requests that are not already scheduled
            var requests = _context.MaintenanceRequest
                 .Include(r => r.Fridge)
                     .ThenInclude(f => f.Customer)
                         .ThenInclude(c => c.Location)
                 // Ensure the request is active and has no scheduled visit attached
                 .Where(r => r.IsActive &&
                             !_context.MaintenanceVisit.Any(v => v.MaintenanceRequestId == r.MaintenanceRequestId &&
                                                                 v.Status == Models.TaskStatus.Scheduled))
                 .ToList();
            ViewBag.Requests = new SelectList(requests, "MaintenanceRequestId", "Fridge.FridgeName");

            ViewBag.RequestsJson = Newtonsoft.Json.JsonConvert.SerializeObject(
                requests.Select(r => new
                {
                    id = r.MaintenanceRequestId,
                    customer = r.Fridge?.Customer?.FullName ?? "N/A",
                    customerAddress = r.Fridge?.Customer?.Location?.Address?? "N/A",
                    model = r.Fridge?.Model ?? "N/A"
                })
            );

            return View(new MaintenanceVisit
            {
                ScheduledDate = DateTime.Today,
                ScheduledTime = DateTime.Now.TimeOfDay
            });
        }

        // ✅ POST: Schedule Visit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScheduleVisit(MaintenanceVisit model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please provide all required fields.";
                return RedirectToAction("ScheduleVisit");
            }

            var request = _context.MaintenanceRequest
                .FirstOrDefault(r => r.MaintenanceRequestId == model.MaintenanceRequestId);

            if (request == null)
            {
                TempData["Error"] = "Selected request does not exist.";
                return RedirectToAction("ScheduleVisit");
            }

            model.FridgeId = request.FridgeId;
            model.MaintenanceRequestId = request.MaintenanceRequestId;

            var technician = _context.Employees.FirstOrDefault();
            if (technician == null)
            {
                TempData["Error"] = "No maintenance technician available. Please add one first.";
                return RedirectToAction("ScheduleVisit");
            }

            model.EmployeeID = technician.EmployeeID;

            _context.MaintenanceVisit.Add(model);
            _context.SaveChanges();

            UpdateVisitAndRequestStatus(model, Models.TaskStatus.Scheduled);

            TempData["Message"] = "Maintenance visit scheduled successfully!";
            return RedirectToAction("ScheduleVisit");
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
        public async Task< IActionResult> CompleteMaintenance(int visitId)
        {
            var visit = _context.MaintenanceVisit
                .Include(v => v.MaintenanceRequest)
                .FirstOrDefault(v => v.MaintenanceVisitId == visitId);

            if (visit == null) return NotFound();

            UpdateVisitAndRequestStatus(visit, Models.TaskStatus.Complete);
            // Create the next month's maintenance request (if none exists)
            var created = await _mrService.CreateNextMonthlyRequestAsync(visit.FridgeId);

            if (created != null)
            {
                TempData["Message"] = $"Maintenance completed — next maintenance request created for {created.RequestDate:yyyy-MM-dd}.";
            }
            else
            {
                TempData["Message"] = "Maintenance completed. No new request created because a pending/scheduled request already exists.";
            }

            return RedirectToAction("PerformMaintenance", new { visitId });
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
                .Where(v => v.MaintenanceRequest.FridgeId == fridgeId)
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
