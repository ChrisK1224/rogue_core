using System.Collections.Generic;

namespace rogueWebUI.Pages
{
    public interface ILocationRepository
    {
        List<string> GetContinents();
        List<string> GetCountries(string continent);
    }
}