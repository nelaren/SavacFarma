using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace SavacFarma.Shared.Localization
{
    public abstract class LocalizationManager : ILocalizationManager
    {
        private Dictionary<string, object> _dictionary;


        public Dictionary<string, object> Dictionary
        {
            get
            {
                if (_dictionary is null)
                    _dictionary = new Dictionary<string, object>();
                return _dictionary;
            }
        }



        public string this[string resource]
        {
            get
            {
                var splited = resource.Split(".");
                if (splited.Length > 1)
                    return GetString(splited[0], splited[1]);
                else
                    return "Resource NOT found";
            }

        }


        string GetString(string resourcename, string searchstring)
        {
            string _string = null;

            var orm = _dictionary.Where(k => k.Key.ToLower() == resourcename.ToLower()).Select(k => k.Value).FirstOrDefault();


            if (orm != null)
            {

                var prop = orm.GetType().GetProperty(searchstring);
                if (prop != null)
                    try
                    {
                        _string = prop.GetValue(orm, null).ToString();
                    }
                    catch { }
            }

            if (_string == null) _string = _dictionary.Count().ToString();
            return _string;
        }



        public virtual void SetLanguage(string cultureInfoName)
        { }

    }
}
