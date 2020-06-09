using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
namespace SavacFarma.Client.Shared.Auth
{
    public class TokenRenovator : IDisposable
    {
        Timer timer;

        public TokenRenovator(ILoginService loginService)
        {
            _loginService = loginService;
        }

        private readonly ILoginService _loginService;

       

        public void Start()
        {
            timer = new Timer();
            timer.Interval = 1000 * 60 * 4;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _loginService.HandlingTokenExpiration();
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
