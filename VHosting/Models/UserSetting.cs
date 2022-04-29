using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class UserSetting
    {
        public UserSetting()
        {
            SettingsOptions = new HashSet<SettingsOption>();
        }

        public int UserId { get; set; }
        public int SettingsId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<SettingsOption> SettingsOptions { get; set; }
    }
}
