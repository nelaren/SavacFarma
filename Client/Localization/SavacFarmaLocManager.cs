using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SavacFarma.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace SavacFarma.Shared.Localization
{
    public class SavacFarmaLocManager : LocalizationManager
    {
        
        private readonly IJSRuntime _js;
        public SavacFarmaLocManager(IJSRuntime js)
        {
            Dictionary.Add("AppStrings", new AppStrings());
            _js = js;
        }
   
        public override void SetLanguage(string cultureInfoName)
        {
            var jsRuntime = (IJSInProcessRuntime)_js;
            jsRuntime.InvokeVoid("blazorCulture.set", cultureInfoName);

        }
    }
}

