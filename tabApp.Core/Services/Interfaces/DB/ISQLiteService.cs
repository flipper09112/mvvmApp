﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces.DB
{
    public interface ISQLiteService
    {
        SQLiteConnection Connection();
    }
}