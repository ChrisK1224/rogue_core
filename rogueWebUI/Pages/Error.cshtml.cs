using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace rogueWebUI.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private readonly ILocationRepository _locationRepo;

        public ErrorModel(ILocationRepository locationRepo)
        {
            _locationRepo = locationRepo;
        }

        public List<SelectListItem> Continents { get; set; }
        public string SelectedContinent { get; set; }

        public List<SelectListItem> Countries { get; set; }
        public string SelectedCountry { get; set; }

        public void OnGet()
        {
            Continents = _locationRepo.GetContinents()
                                      .Select(x => new SelectListItem() { Value = x, Text = x })
                                      .ToList();
            SelectedContinent = Continents.First().Value;

            Countries = _locationRepo.GetCountries(SelectedContinent)
                                     .Select(x => new SelectListItem() { Value = x, Text = x })
                                     .ToList();
            SelectedCountry = Countries.First().Value;
        }

        public JsonResult OnGetCountriesFilter(string continent)
        {
            return new JsonResult(_locationRepo.GetCountries(continent));
        }
        //public string RequestId { get; set; }

        //public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        //private readonly ILogger<ErrorModel> _logger;

        //public ErrorModel(ILogger<ErrorModel> logger)
        //{
        //    _logger = logger;
        //}
        //public string Message { get; set; }

        //public void OnGet()
        //{
        //    Message = "Your application description page.";
        //}
        //public JsonResult OnGetCountriesFilter(string continent)
        //{
        //    return new JsonResult(_locationRepo.GetCountries(continent));
        //}
        ////public void OnGet()
        ////{
        ////    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        ////    Message = "Your application description page.";
        ////}
        //public static String testhtml()
        //{
        //    return "<div style=\"height:100%;width:100%;\"><label style=\"font-size:30px;\">ChooseTable</label><ul id=\"myUL\"><li><span class=\"caret\"><im</li><li><span class=\"caret\"><img src=\"~/pics/bundle.png\" style=\"height:25px; width: 25px; \"></img><label>Data</label></span><ul class=\"nested\"></ul></li></ul></div>";
        //}
        ////public void OnGet()
        ////{
        ////    Message = "Your application description page.";
        ////}
    }
}
