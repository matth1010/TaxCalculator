using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TaxCalculator.Web.ViewModels;

namespace TaxCalculator.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public IndexModel(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            // Set the API base address
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
        }

        public List<TaxCalculationViewModel> TaxRecords { get; set; }
        [BindProperty]
        public TaxCalculationViewModel taxCalculation { get; set; }

        public async Task OnGetAsync()
        {
            TaxRecords = await _httpClient.GetFromJsonAsync<List<TaxCalculationViewModel>>("api/tax/records");
        }
    }
}