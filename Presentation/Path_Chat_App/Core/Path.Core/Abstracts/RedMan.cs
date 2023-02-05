using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Path.Core.Dto;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path.Core.Abstracts
{
    public class RedMan : IDisposable
    {
        /// <summary>
        /// Redisin host edildiği adresin referans değişkeni
        /// </summary>
        private string redisHost;
        /// <summary>
        /// Redise verilmiş olan şifrenin referans değişkeni
        /// </summary>
        private string redisPass;
        ConnectionMultiplexer redisMultiFactory;
        StackExchange.Redis.IDatabase redisDb;

        public RedMan(int dbNumber = 0)
        {
            //Using ile işlem sonrası dispose olmasını sağlıyoruz
            //Using içinde Configuration manager dan redisin bağlantı bilgilerini alıyoruz
            using (ConfigurationManager config = new())
            {
                //Conf un hangi json file dan alınacağını belittik
                config.AddJsonFile("appsettings.json");
                //Redis host
                redisHost = config.GetSection("RedisHost").Value ?? "127.0.0.1:6379";
                //Redis pass bilgilerini aldık
                redisPass = config.GetSection("RedisPassword").Value ?? "path123456";
            }

            redisMultiFactory = ConnectionMultiplexer.Connect($"{redisHost},password={redisPass}");
            redisDb = redisMultiFactory.GetDatabase(dbNumber);
        }


        #region String operasyonlar
        public void SetString(string key, string value) => redisDb.StringSet(key, value);
        public void AppendString(string key, string value) => redisDb.StringAppend(key, value);
        public string GetString(string key) => redisDb.StringGet(key);
        #endregion

        #region List Operasyonlar
        public void AddList<T>(string key, T entity)
        {
            RemoveList<T>(key, entity);
            string strVal = JsonConvert.SerializeObject(entity);
            redisDb.ListRightPush(key, strVal);
        }

       
        public List<T> GetList<T>(string key)
        {
            var values = redisDb.ListRange(key, 0, -1);
            return Array.ConvertAll(values, x => JsonConvert.DeserializeObject<T>(x)).ToList();


        }

        public void RemoveList<T>(string key, T entity) => redisDb.ListRemove(key, JsonConvert.SerializeObject(entity));

        #endregion










        /// <summary>
        /// Klasik dispose kardeşimiz.
        /// </summary>
        public void Dispose()
        {
            //redisPoolManager.Dispose();
            //if (redisClient.IsValueCreated)
            //    redisClient.Value.Dispose();
        }


        #region ServiceStack.Redis Lazy örnekli redis
        ///// <summary>
        ///// Redisi yönetceğimiz pool manager referasn değişkeni
        ///// </summary>
        //private PooledRedisClientManager redisPoolManager;
        ///// <summary>
        ///// Redisin lazy kopyasının yönetileceği referans değişkeni
        ///// </summary>
        //private Lazy<IRedisClient> redisClient;
        //private Lazy<IRedisList> redisUserList;

        ///// <summary>
        ///// Redis Manager consturctor
        ///// </summary>
        ///// <param name="_dbNumber">Database numarası</param>
        //public RedMan(long _dbNumber = 0)
        //{
        //    //Using ile işlem sonrası dispose olmasını sağlıyoruz
        //    //Using içinde Configuration manager dan redisin bağlantı bilgilerini alıyoruz
        //    using (ConfigurationManager config = new())
        //    {
        //        //Conf un hangi json file dan alınacağını belittik
        //        config.AddJsonFile("appsettings.json");
        //        //Redis host
        //        redisHost = config.GetSection("RedisHost").Value ?? "127.0.0.1:6379";
        //        //Redis pass bilgilerini aldık
        //        redisPass = config.GetSection("RedisPassword").Value ?? "path123456";
        //    }
        //    //Redis pool managera hangi db ve hangi host ile bağlanacağını söyeleyerek referansını oluşturduk
        //    redisPoolManager = new(_dbNumber, new[] { $"{redisPass}@{redisHost}" });
        //    //Redisin lazy kopyasını oluşturduk (Kullanılmadığı sürede safe olsun ve yaratılmasın diye lazy  ayrıca 2. görüşmede sorulan sorulardan biriydi.)
        //    redisClient = new Lazy<IRedisClient>(GetNewClient);
        //    redisUserList = new Lazy<IRedisList>();

        //}
        ///// <summary>
        ///// Pool managerdan redisin bir copyasını vermesini istedik. 
        ///// </summary>
        ///// <returns></returns>
        //private IRedisClient GetNewClient()
        //{
        //    return redisPoolManager.GetClient();
        //}

        ///// <summary>
        ///// Gelecek olan komutların işleneceği method
        ///// </summary>
        ///// <typeparam name="T">Nesne tipi</typeparam>
        ///// <param name="cmd">Yapılacak Action</param>
        ///// <returns></returns>
        //private T Command<T>(Func<IRedisClient, T> cmd)
        //{
        //    using (var client = GetNewClient())
        //    {
        //        return cmd(client);
        //    }
        //}

        ///// <summary>
        ///// Bir nesneyi set etmek için
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="entity"></param>
        //public void Set<T>(string key, T entity)
        //{
        //    Command(_ => _.Set(key, entity));
        //}

        ///// <summary>
        ///// Bir nesneyi getirmek için
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public T Get<T>(string key)
        //{
        //    return Command(_ => _.Get<T>(key));
        //} 
        #endregion


    }
}
