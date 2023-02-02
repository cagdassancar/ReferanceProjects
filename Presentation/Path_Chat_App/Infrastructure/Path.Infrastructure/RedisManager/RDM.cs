using Path.Core.Interfaces;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path.Infrastructure.RedisManager
{
    public class RDM : IRedisBase
    {

        public  RDM()
        {

        }


        public static void AddLoginUser(string newUserName)
        {
            using (IRedisClient client = new RedisClient())
            {
                var names = client.Lists["loginUsers"];
                names.Add("Murat");
                names.Add("Yadigar");
                names.Add("Serkan");
            }

            using (IRedisClient client = new RedisClient())
            {
                var names = client.Lists["names"];

                foreach (var name in names)
                    Console.WriteLine($"İsim : {name}");
            }
        }


    }
}
