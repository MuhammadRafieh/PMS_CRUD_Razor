using Pharmacy_Inventory_System.Data;
using Pharmacy_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacy_Inventory_System.Pages.Medications
{
    public class IndexModel : PageModel
    {
        private readonly MedicationRepository _repository;

        public IndexModel(MedicationRepository repository)
        {
            _repository = repository;
        }

        public List<Medication> Medications { get; set; } = new();

        public async Task OnGetAsync()
        {
            Medications = await _repository.GetAllAsync();
        }
    }
}
