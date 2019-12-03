using DenisChallenge.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DenisChallenge.Service.interfaces
{
    public interface IAanbodApi
    {
        List<GroeperingsTabelViewModel> GetTopMakelaars(IConfiguration config, bool isTuin);
    }
}
