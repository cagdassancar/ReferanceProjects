using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path.Core.Dto
{
    [Serializable]
    public class LoginUser
    {       
        public string UserName { get; set; }
        public string SessionID { get; set; }

    }
}
