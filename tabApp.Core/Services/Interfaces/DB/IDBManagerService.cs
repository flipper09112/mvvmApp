﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces.DB
{
    public interface IDBManagerService
    {
        void SaveClient(Models.Client client, string toRegist);
    }
}