using System.Collections.Generic;

namespace SavacFarma.Shared.Localization
{
    public interface ILocalizationManager
    {
        string this[string resource] { get; }

        Dictionary<string, object> Dictionary { get; }

        void SetLanguage(string cultureInfoName);
    }
}