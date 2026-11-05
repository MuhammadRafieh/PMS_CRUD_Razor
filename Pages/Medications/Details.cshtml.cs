using Pharmacy_Inventory_System.Data;
using Pharmacy_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacy_Inventory_System.Pages.Medications
{
    public class DetailsModel : PageModel
    {
        private readonly MedicationRepository _repository;

        public DetailsModel(MedicationRepository repository)
        {
            _repository = repository;
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
    }
}
