using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.Domain.Entities;

namespace Biblioteka.WPF.Events
{
    public class UserLoggedInEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
