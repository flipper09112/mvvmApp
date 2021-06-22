using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces.Timer;
using tabApp.Core.ViewModels.Snooze;

namespace tabApp.Core.Services.Implementations.Timer
{
    public class InativityTimerService : IInativityTimerService
    {
        private IMvxNavigationService _navigationService;
        
        private TimeSpan SessionDuration = TimeSpan.FromMinutes(5);
        private Stopwatch stopWatch = new Stopwatch();
        private bool alreadyStarted;

        public InativityTimerService(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void Restart()
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Restart();
                alreadyStarted = true;
            }
            else
            {
                stopWatch.Restart();
            }
        }

        public void Start()
        {
            if (alreadyStarted)
            {
                Restart();
                return;
            } else
            {
                alreadyStarted = true;
                stopWatch.Start();
            }

            Task.Run(async () => {
                while(true)
                {
                    if (stopWatch.IsRunning && stopWatch.Elapsed.Minutes >= SessionDuration.Minutes)
                    {
                        if (!alreadyStarted) return;
                        alreadyStarted = false;
                        Stop();
                        await _navigationService.Navigate<SnoozeViewModel>();
                        return;
                    }
                }
            });
        }

        public void Stop()
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
            }
        }
    }
}
