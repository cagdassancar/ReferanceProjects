using Microsoft.Extensions.Configuration;
using Path.Core.Abstracts;
using Path.Core.Dto;
using Path.Core.Interfaces;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Diagnostics;
using static ServiceStack.Diagnostics.Events;

namespace Path.Infrastructure.RedisManager
{
    /// <summary>
    /// *** Path Referans Projesi ***
    /// Çağdaş Sancarbarlaz
    /// RDM = Redis operasyonlarını yöneten sınıf 
    /// Amacımız redis hakkındaki bilgim olup olmadığını anlamak malesef zamanım kısıtlı olduğu için hızlı bir şekilde yapmak olduğundan dolayı bu şekilde yaptım.
    /// Bir proje içerisinde Lazy client olarak pool lar üzerinden de kullanabilirim.
    /// </summary>

    public class RDM : RedMan, IRedisBase
    {
        public RDM(int dbNo = 0) : base(dbNo) { }

        public void SetLoginUser(LoginUser user)
        {
            AddList<LoginUser>("loginsUsers", user);
        }

        public LoginUser GetLoginUser(string sessionID)
        {
            List<LoginUser> userList = GetList<LoginUser>("loginsUsers");
            return userList.FirstOrDefault(x => x.SessionID == sessionID);

        }


        public void RemoveLoginUser(LoginUser user)
        {
            RemoveList<LoginUser>("loginsUsers", user);
        }

        public List<LoginUser> GetLoginUsers()
        {
            return GetList<LoginUser>("loginsUsers");
        }





    }
}
