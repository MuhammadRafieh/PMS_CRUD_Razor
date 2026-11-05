using Pharmacy_Inventory_System.Data;
using Pharmacy_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacy_Inventory_System.Pages.Medications
{
    public class CreateModel : PageModel
    {
        private readonly MedicationRepository _repository;
        private readonly MedicationImageService _images;

        public CreateModel(
            MedicationRepository repository,
            MedicationImageService images)
        {
            _repository = repository;
            _images = images;
        }

        [BindProperty]
        public Medication Medication { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public void OnGet()
        {
            Medication.Status = "A";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(ImageFile), "Please select a medication image.");
            }
            else
            {
                string? error = _images.Validate(ImageFile);

                if (error != null)
                    ModelState.AddModelError(nameof(ImageFile), error);
            }

            if (!ModelState.IsValid)
                return Page();

            string imagePath = await _images.SaveAsync(ImageFile!);

            try
            {
                Medication.Image = imagePath;
                await _repository.CreateAsync(Medication);

                TempData["Success"] = "Medication created successfully.";

                return RedirectToPage("Index");
            }
            catch
            {
                _images.Delete(imagePath);
                throw;
            }
        }
    }
}
