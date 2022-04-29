using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class Account
    {
        public Account()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Permissions { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
