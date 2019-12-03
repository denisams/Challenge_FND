using DenisChallenge.Domain.Entities;
using DenisChallenge.Domain.ViewModels;
using DenisChallenge.Service.interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DenisChallenge.Service
{
    public class AanbodApi : IAanbodApi
    {
        private dynamic GetPartnerApi(IConfiguration config, bool hasTuin)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = config.GetSection("N_APIs:BaseURL").Value;
                string key = config.GetSection("N_APIs:Key").Value;

                string withTuin = "/tuin";

                HttpResponseMessage response = client.GetAsync(
                    baseURL +
                    $"{key}" +
                    $"/?type=koop&zo=/amsterdam" +
                    (hasTuin ? withTuin : string.Empty) +
                    $"/&page=1&pagesize=500").Result;

                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;

                dynamic result = JsonConvert.DeserializeObject(content);

                return result;
            }
        }

        private List<GroeperingsTabelViewModel> ConvertMakelaarsToTop(dynamic apiResult)
        {
            Aanbod aanbod = new Aanbod();
            foreach (var item in apiResult.Objects)
            {
                aanbod.EigenschapBeschrijving.Add(
                    new EigenschapBeschrijving
                    {
                        MakelaarNaam = item.MakelaarNaam
                    });
            }

            var result = aanbod.EigenschapBeschrijving
                       .GroupBy(x => x.MakelaarNaam)
                       .Select(g => new { MekelaarNaam = g.Key, Kwantiteit = g.Count() })
                       .OrderByDescending(o => o.Kwantiteit)
                       .Take(10);

            List<GroeperingsTabelViewModel> groeperingsTabelViewModel = new List<GroeperingsTabelViewModel>();

            foreach (var item in result)
            {
                groeperingsTabelViewModel.Add(
                    new GroeperingsTabelViewModel
                    {
                        Kwantiteit = item.Kwantiteit,
                        MakelaarNaam = item.MekelaarNaam
                    });
            }

            return groeperingsTabelViewModel;
        }

        public List<GroeperingsTabelViewModel> GetTopMakelaars(IConfiguration config, bool hasTuin)
        {
            var getMakelaars = GetPartnerApi(config, hasTuin);
            var groeperingsTabelViewModel = ConvertMakelaarsToTop(getMakelaars);

            return groeperingsTabelViewModel;
        }
    }
}
