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
        void SetLoginUser(LoginUser _user);
        LoginUser GetLoginUser(string _sessionID);
        IList<LoginUser> GetAllLoginUser();
        void RemoveLoginUser(LoginUser _user);
        void RemoveLoginUser(string _sessionID);

    }
}
