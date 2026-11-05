using Pharmacy_Inventory_System.Data;
using Pharmacy_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacy_Inventory_System.Pages.Medications
{
    public class EditModel : PageModel
    {
        private readonly MedicationRepository _repository;
        private readonly MedicationImageService _images;

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var medication = await _repository.GetByIdAsync(id);

            if (medication == null)
                return NotFound();

            Medication = medication;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id != Medication.Id)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string? error = _images.Validate(ImageFile);

                if (error != null)
                    ModelState.AddModelError(nameof(ImageFile), error);
            }

            if (!ModelState.IsValid)
            {
                Medication.Image = existing.Image;
                return Page();
            }

            string oldImage = existing.Image;
            string newImage = oldImage;

            if (ImageFile != null && ImageFile.Length > 0)
                newImage = await _images.SaveAsync(ImageFile);

            Medication.Image = newImage;

            try
            {
                bool updated = await _repository.UpdateAsync(Medication);

                if (!updated)
                {
                    if (newImage != oldImage)
                        _images.Delete(newImage);

                    return NotFound();
                }
            }
            catch
            {
                if (newImage != oldImage)
                    _images.Delete(newImage);

                throw;
            }

            if (newImage != oldImage)
                _images.Delete(oldImage);

            TempData["Success"] = "Medication updated successfully.";

            return RedirectToPage("Index");
        }
    }
}
