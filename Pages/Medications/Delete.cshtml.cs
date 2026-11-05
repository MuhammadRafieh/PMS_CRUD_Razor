using Pharmacy_Inventory_System.Data;
using Pharmacy_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacy_Inventory_System.Pages.Medications
{
    public class DeleteModel : PageModel
    {
        private readonly MedicationRepository _repository;
        private readonly MedicationImageService _images;

        public DeleteModel(
            MedicationRepository repository,
            MedicationImageService images)
        {
            _repository = repository;
            _images = images;
        }

        public Medication Medication { get; set; } = new();

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
            var medication = await _repository.GetByIdAsync(id);

            if (medication == null)
                return NotFound();

            bool deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            _images.Delete(medication.Image);

            TempData["Success"] = "Medication deleted successfully.";

            return RedirectToPage("Index");
        }
    }
}
