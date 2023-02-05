using Path.Core.Dto;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path.Core.Interfaces
{
    public interface IRedisBase
    {
        void SetLoginUser(LoginUser user);
        void RemoveLoginUser(LoginUser user);
        List<LoginUser> GetLoginUsers();



    }
}
