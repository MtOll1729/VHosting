using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class SettingsOption
    {
        public int SettingsId { get; set; }
        public int OptionId { get; set; }
        public string? Value { get; set; }

        public virtual UserSetting Settings { get; set; } = null!;
    }
}
