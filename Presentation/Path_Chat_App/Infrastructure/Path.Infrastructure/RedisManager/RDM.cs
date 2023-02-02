using Microsoft.Extensions.Configuration;
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

    public class RDM : IRedisBase
    {
        /// <summary>
        /// Redis server'in bulunduğu IP adresi
        /// </summary>
        private string _redisIP;
        /// <summary>
        /// Redis server'in Port adresi
        /// </summary>
        private int _redisPort;
        /// <summary>
        /// Redis server'a verilmiş şifre
        /// </summary>
        private string _redisPassword;


        private const int loginUserDb = 0 ;
        private const int channelDb = 1;


        public RDM()
        {
            using (ConfigurationManager config = new())
            {
                config.AddJsonFile("appsettings.json");
                _redisIP = config.GetSection("RedisServer").Value ?? "127.0.0.1";
                _redisPort = Int32.Parse(config.GetSection("RedisPort").Value ?? "6379");
                _redisPassword = config.GetSection("RedisPassword").Value ?? "path123456";
            }
        }


        #region User Operations
        public void SetLoginUser(LoginUser _user)
        {

            using (IRedisClient client = new RedisClient(_redisIP, _redisPort, _redisPassword, loginUserDb))
            {
                if (GetLoginUser(_user.SessionID) == null)
                    client.As<LoginUser>().Store(_user);
            }
        }
        public LoginUser GetLoginUser(string _sessionID)
        {

            using (IRedisClient client = new RedisClient(_redisIP, _redisPort, _redisPassword, loginUserDb))
            {
                LoginUser user = null;
                client.As<LoginUser>().GetAll().Each((i, v) =>
                {
                    if (v.SessionID == _sessionID)
                    {
                        user = v;
                        return;
                    }

                });

                return user;
            }
        }
        public IList<LoginUser> GetAllLoginUser()
        {

            using (IRedisClient client = new RedisClient(_redisIP, _redisPort, _redisPassword, loginUserDb))
            {
                return client.As<LoginUser>().GetAll();
            }
        }
        public void RemoveLoginUser(LoginUser _user)
        {

            using (IRedisClient client = new RedisClient(_redisIP, _redisPort, _redisPassword, loginUserDb))
            {
                client.As<LoginUser>().Delete(_user);
            }

        }
        public void RemoveLoginUser(string _sessionID)
        {
            using (IRedisClient client = new RedisClient(_redisIP, _redisPort, _redisPassword, loginUserDb))
            {
                LoginUser user = null;
                client.As<LoginUser>().GetAll().Each((i, v) =>
                {
                    if (v.SessionID == _sessionID)
                        user = v;

                });
            }
        }
        #endregion




        #region Old
        ///// <summary>
        ///// Redisin lazy instance'ı 
        ///// (Görüşmelerde lazy durumu konuşulduğu için lazy kullanmak istedim)
        ///// </summary>
        //private Lazy<IRedisClient> _redisFactory;

        ///// <summary>
        ///// Redis'in istemci havuzunu yönetmek için Pooled ı kullandım
        ///// </summary>
        //private PooledRedisClientManager _redisManager;


        //public RDM()
        //{

        //    _redisFactory = new Lazy<IRedisClient>(GetRedisClient);
        //    _redisManager = new PooledRedisClientManager(/*$"{_redisIP}:{_redisPort}"*/);
        //}

        //private IRedisClient GetRedisClient()
        //{
        //    return _redisManager.GetClient();
        //}


        ///// <summary>
        ///// Redis'
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="entity"></param>
        //public void SetLoginUser<T>(T entity)
        //{
        //    using (var client = ((RedisClient)GetRedisClient()))
        //    {
        //        client.ChangeDb(0);
        //        client.As<T>().Store(entity);
        //    }
        //}

        ///// <summary>
        ///// Redis'
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="entity"></param>
        //public void RemoveLoginUser<T>(T entity)
        //{
        //    using (var client = ((RedisClient)GetRedisClient()))
        //    {
        //        client.ChangeDb(0);
        //        client.As<T>().Store(entity);
        //    }
        //}

        //public T GetLoginUser<T>(string sessionID)
        //{
        //    using (var client = ((RedisClient)GetRedisClient()))
        //    {
        //        client.Get()
        //    }
        //}

        //public void Set2<LoginUser>(LoginUser entity)
        //{
        //    Run(_ => _.As<LoginUser>().Store(entity));
        //}
        //public void Dispose()
        //{
        //    _redisManager.Dispose();
        //    if (_redisFactory.IsValueCreated)
        //    {
        //        _redisFactory.Value.Dispose();
        //    }
        //} 
        #endregion
    }
}
