using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.Faturation;
using tabApp.Core.Services.Implementations.Faturation.Helpers;

namespace tabApp.Core.Services.Interfaces.Faturation
{
    public interface IFaturationService
    {
        TrasnportationDoc DocumentSelected { get; set; }

        //classes
        TrasnportationsDocs TrasnportationsDocs { get; }
        Administration Administration { get; }
        Implementations.Faturation.Helpers.Clients Clients { get; }
        FatProducts Products { get; }
        Client ClientSelected { get; set; }
    }
}
