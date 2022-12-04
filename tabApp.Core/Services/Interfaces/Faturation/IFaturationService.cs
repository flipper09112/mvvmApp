using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Implementations.Faturation;

namespace tabApp.Core.Services.Interfaces.Faturation
{
    public interface IFaturationService
    {
        TrasnportationDoc DocumentSelected { get; set; }

        //classes
        TrasnportationsDocs TrasnportationsDocs { get; }
        Administration Administration { get; }
        Implementations.Faturation.Clients Clients { get; }
        FatProducts Products { get; }
    }
}
