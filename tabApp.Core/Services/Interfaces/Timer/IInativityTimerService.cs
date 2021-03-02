using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces.Timer
{
    public interface IInativityTimerService
    {
        void Start();
        void Restart();
        void Stop();
    }
}
