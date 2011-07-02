using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;

namespace metafetch
{
    public class UserSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public StringCollection LibraryPaths
        {
            get
            {
                return (StringCollection)this["LibraryPaths"];
            }

            set
            {
                this["LibraryPaths"] = value;
            }
        }
    }
}
