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
        //private TimeSpan SessionDuration = TimeSpan.FromSeconds(10);
        private Stopwatch stopWatch = new Stopwatch();
        private bool alreadyStarted;
        private Task _task;
        private bool _destroy;

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

            _task = Task.Run(async () => {
                while(true)
                {
                    if (stopWatch.IsRunning && stopWatch.Elapsed.Minutes >= SessionDuration.Minutes)
                   // if (stopWatch.IsRunning && stopWatch.Elapsed.Seconds >= SessionDuration.Seconds)
                    {
                        if (!alreadyStarted) return;
                        alreadyStarted = false;
                        Stop();
                        await _navigationService.Navigate<SnoozeViewModel>();
                        return;
                    }

                    if (_destroy)
                        return;
                }
            });
        }

        public void Destroy()
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
            }
            _destroy = true;
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
