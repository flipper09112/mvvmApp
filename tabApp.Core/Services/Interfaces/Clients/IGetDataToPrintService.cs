﻿using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Clients
{
    public interface IGetDataToPrintService
    {
        List<PrintPreview> GetDataToPrint(Client clientSelected);
    }
}
