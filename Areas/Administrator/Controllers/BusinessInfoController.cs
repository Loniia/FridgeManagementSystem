using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FridgeManagementSystem.Data; // adjust namespace
using FridgeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // adjust namespace

namespace FridgeManagementSystem.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Admin")]
    public class BusinessInfoController : Controller
    {

        private readonly FridgeDbContext _context;
        private readonly ILogger<BusinessInfoController> _logger;
        private readonly IWebHostEnvironment _environment;

        public BusinessInfoController(FridgeDbContext context,
                                    ILogger<BusinessInfoController> logger,
                                    IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        // GET: BusinessInfo
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    _logger.LogInformation("No business info found, redirecting to Create");
                    return RedirectToAction(nameof(Create));
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business information");
                TempData["ErrorMessage"] = "An error occurred while retrieving business information.";
                return View();
            }
        }

        // GET: BusinessInfo/Manage - Comprehensive management view
        public async Task<IActionResult> Manage()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos.FirstOrDefaultAsync();
                var viewModel = businessInfo != null ?
                    MapToViewModel(businessInfo) :
                    new BusinessInfoViewModel();

                ViewBag.Industries = await GetIndustryList();
                ViewBag.BusinessTypes = await GetBusinessTypeList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading business management page");
                TempData["ErrorMessage"] = "An error occurred while loading the management page.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: BusinessInfo/Manage - Handle comprehensive updates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(BusinessInfoViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.LogoFile != null)
                    {
                        viewModel.LogoUrl = await UploadFile(viewModel.LogoFile, "logos");
                    }

                    if (viewModel.BannerFile != null)
                    {
                        viewModel.BannerImageUrl = await UploadFile(viewModel.BannerFile, "banners");
                    }

                    var existingInfo = await _context.BusinessInfos.FirstOrDefaultAsync();

                    if (existingInfo != null)
                    {
                        MapToEntity(viewModel, existingInfo);
                        existingInfo.UpdatedDate = DateTime.Now;
                        _context.Update(existingInfo);
                    }
                    else
                    {
                        var newInfo = MapToEntity(viewModel, new BusinessInfo());
                        _context.Add(newInfo);
                    }

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Business information updated successfully");
                    TempData["SuccessMessage"] = "Business information updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Industries = await GetIndustryList();
                ViewBag.BusinessTypes = await GetBusinessTypeList();
                TempData["ErrorMessage"] = "Please correct the validation errors.";
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating business information");
                TempData["ErrorMessage"] = "An error occurred while updating business information.";

                ViewBag.Industries = await GetIndustryList();
                ViewBag.BusinessTypes = await GetBusinessTypeList();
                return View(viewModel);
            }
        }

        // GET: BusinessInfo/About - Public facing "About Us" page
        [AllowAnonymous]
        public async Task<IActionResult> About()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    return View("NoBusinessInfo");
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading About page");
                return View("Error");
            }
        }

        // GET: BusinessInfo/Services - Public facing "What We Do" page
        [AllowAnonymous]
        public async Task<IActionResult> Services()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    return View("NoBusinessInfo");
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Services page");
                return View("Error");
            }
        }

        // Helper Methods
        private BusinessInfoViewModel MapToViewModel(BusinessInfo entity)
        {
            return new BusinessInfoViewModel
            {
                BusinessInfoId = entity.BusinessInfoId,
                CompanyName = entity.CompanyName,
                RegistrationNumber = entity.RegistrationNumber,
                TaxNumber = entity.TaxNumber,
                Address = entity.Address,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                CompanyDescription = entity.CompanyDescription,
                MissionStatement = entity.MissionStatement,
                ServicesDescription = entity.ServicesDescription,
                CoreValues = entity.CoreValues,
                Industry = entity.Industry,
                YearFounded = entity.YearFounded,
                BusinessType = entity.BusinessType,
                LogoUrl = entity.LogoUrl,
                BannerImageUrl = entity.BannerImageUrl,
                FacebookUrl = entity.FacebookUrl,
                LinkedInUrl = entity.LinkedInUrl,
                TwitterUrl = entity.TwitterUrl
            };
        }

        private BusinessInfo MapToEntity(BusinessInfoViewModel viewModel, BusinessInfo entity)
        {
            entity.CompanyName = viewModel.CompanyName;
            entity.RegistrationNumber = viewModel.RegistrationNumber;
            entity.TaxNumber = viewModel.TaxNumber;
            entity.Address = viewModel.Address;
            entity.Phone = viewModel.Phone;
            entity.Email = viewModel.Email;
            entity.Website = viewModel.Website;
            entity.CompanyDescription = viewModel.CompanyDescription;
            entity.MissionStatement = viewModel.MissionStatement;
            entity.ServicesDescription = viewModel.ServicesDescription;
            entity.CoreValues = viewModel.CoreValues;
            entity.Industry = viewModel.Industry;
            entity.YearFounded = viewModel.YearFounded;
            entity.BusinessType = viewModel.BusinessType;
            entity.LogoUrl = viewModel.LogoUrl;
            entity.BannerImageUrl = viewModel.BannerImageUrl;
            entity.FacebookUrl = viewModel.FacebookUrl;
            entity.LinkedInUrl = viewModel.LinkedInUrl;
            entity.TwitterUrl = viewModel.TwitterUrl;

            return entity;
        }

        private async Task<string> UploadFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        private Task<List<SelectListItem>> GetIndustryList()
        {
            var list = new List<SelectListItem>
        {
            new SelectListItem { Value = "Beverage Manufacturing", Text = "Beverage Manufacturing" },
            new SelectListItem { Value = "Food & Beverage Distribution", Text = "Food & Beverage Distribution" },
            new SelectListItem { Value = "Retail", Text = "Retail" },
            new SelectListItem { Value = "Hospitality", Text = "Hospitality" },
            new SelectListItem { Value = "Equipment Rental", Text = "Equipment Rental" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };

            return Task.FromResult(list);
        }

        private Task<List<SelectListItem>> GetBusinessTypeList()
        {
            var list = new List<SelectListItem>
        {
            new SelectListItem { Value = "Fridge Management Services", Text = "Fridge Management Services" },
            new SelectListItem { Value = "Beverage Supplier", Text = "Beverage Supplier" },
            new SelectListItem { Value = "Equipment Provider", Text = "Equipment Provider" },
            new SelectListItem { Value = "Service Company", Text = "Service Company" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };

            return Task.FromResult(list);
        }

        // GET: BusinessInfo/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                bool exists = await _context.BusinessInfos.AsNoTracking().AnyAsync();
                if (exists)
                {
                    _logger.LogWarning("Attempt to create business info when it already exists");
                    TempData["WarningMessage"] = "Business information already exists. You can edit it from the main page.";
                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create GET action");
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: BusinessInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessInfo businessInfo)
        {
            try
            {
                bool exists = await _context.BusinessInfos.AsNoTracking().AnyAsync();
                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "Business information already exists. Please edit it instead.");
                    TempData["ErrorMessage"] = "Business information already exists. Please edit it instead.";
                    return View(businessInfo);
                }

                if (ModelState.IsValid)
                {
                    _context.Add(businessInfo);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Business information created successfully - ID: {BusinessInfoId}", businessInfo.BusinessInfoId);
                    TempData["SuccessMessage"] = "Business information created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Please correct the errors below.");
                TempData["ErrorMessage"] = "Please correct the validation errors.";
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating business information");
                ModelState.AddModelError(string.Empty, "An error occurred while creating business information.");
                TempData["ErrorMessage"] = "An error occurred while creating business information.";
                return View(businessInfo);
            }
        }

        // GET: BusinessInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    _logger.LogWarning("Invalid ID provided to Edit - ID: {Id}", id);
                    return BadRequest("Invalid ID");
                }

                var businessInfo = await _context.BusinessInfos.FindAsync(id);
                if (businessInfo == null)
                {
                    _logger.LogWarning("Business info not found for Edit - ID: {Id}", id);
                    return NotFound();
                }

                return View(businessInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading business info for editing - ID: {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: BusinessInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BusinessInfo businessInfo)
        {
            try
            {
                if (id != businessInfo.BusinessInfoId)
                {
                    _logger.LogWarning("ID mismatch in Edit - Route: {RouteId}, Model: {ModelId}", id, businessInfo.BusinessInfoId);
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(businessInfo);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Business information updated successfully - ID: {BusinessInfoId}", id);
                        TempData["SuccessMessage"] = "Business information updated successfully!";
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        _logger.LogError(ex, "Concurrency exception updating business info - ID: {BusinessInfoId}", id);
                        if (!BusinessInfoExists(id))
                        {
                            return NotFound();
                        }
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Please correct the validation errors.";
                return View(businessInfo);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating business information - ID: {BusinessInfoId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating business information.");
                TempData["ErrorMessage"] = "An error occurred while updating business information.";
                return View(businessInfo);
            }
        }

        // Helper method to check if business info exists
        private bool BusinessInfoExists(int id)
        {
            return _context.BusinessInfos.Any(e => e.BusinessInfoId == id);
        }

        // Add these methods to your BusinessInfoController

        [HttpGet]
        [AllowAnonymous]
        [Route("/api/businessinfo")]
        public async Task<IActionResult> GetBusinessInfoApi()
        {
            try
            {
                var businessInfo = await _context.BusinessInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (businessInfo == null)
                {
                    return NotFound(new { message = "Business information not found" });
                }

                // Return as JSON
                return Ok(new
                {
                    companyName = businessInfo.CompanyName,
                    missionStatement = businessInfo.MissionStatement,
                    companyDescription = businessInfo.CompanyDescription,
                    phone = businessInfo.Phone,
                    email = businessInfo.Email,
                    address = businessInfo.Address,
                    // Add other properties as needed
                    home = new
                    {
                        title = "Welcome to " + businessInfo.CompanyName,
                        content = businessInfo.CompanyDescription,
                        contactInfo = new
                        {
                            email = businessInfo.Email,
                            phone = businessInfo.Phone
                        }
                    },
                    about = new
                    {
                        title = "About " + businessInfo.CompanyName,
                        content = businessInfo.CompanyDescription,
                        additionalInfo = businessInfo.MissionStatement
                    },
                    // ... similar for other sections
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in business info API");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

    }
}

