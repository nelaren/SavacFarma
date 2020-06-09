using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;

namespace SavacFarma.Client.Shared.Clases
{
    public class MenuItem
    {

        public string Text { get; set; }
        public int idItem { get; set; }
        public int Tipo { get; set; }
        public List<MenuItem> Items { get; set; }
        public string TelerikIcon { get; set; }
        public string UrlField { get; set; }
    }
}
