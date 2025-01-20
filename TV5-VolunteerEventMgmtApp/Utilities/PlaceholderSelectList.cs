using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public class PlaceholderSelectList : SelectList
    {
        private string _placeholder;
        public PlaceholderSelectList(string placeholder, IEnumerable items) : base(items)
        {
            _placeholder = placeholder;
            
        }

        public PlaceholderSelectList(string placeholder, IEnumerable items, object selectedValue) : base(items, selectedValue)
        {
            _placeholder = placeholder;
        }

        public PlaceholderSelectList(string placeholder, IEnumerable items, string dataValueField, string dataTextField) : base(items, dataValueField, dataTextField)
        {
            _placeholder = placeholder;
        }

        public PlaceholderSelectList(string placeholder, IEnumerable items, string dataValueField, string dataTextField, object selectedValue) : base(items, dataValueField, dataTextField, selectedValue)
        {
            _placeholder = placeholder;
        }

        public PlaceholderSelectList(string placeholder, IEnumerable items, string dataValueField, string dataTextField, object selectedValue, string dataGroupField) : base(items, dataValueField, dataTextField, selectedValue, dataGroupField)
        {
            _placeholder = placeholder;
        }
    }
}
