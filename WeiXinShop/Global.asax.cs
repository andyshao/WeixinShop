using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WeiXinShop
{
    public class Global : System.Web.HttpApplication
    {
        private ServiceStack.Redis.IRedisClientsManager clientManager;
        protected void Application_Start(object sender, EventArgs e)
        {
            string ls_hosts, ls_port;
            ls_hosts = GyRedis.GyRedis.RedisHost;
            ls_port = GyRedis.GyRedis.RedisPort.ToString();

            this.clientManager = new ServiceStack.Redis.PooledRedisClientManager(ls_hosts + ":" + ls_port);
            Harbour.RedisSessionStateStore.RedisSessionStateStoreProvider.SetClientManager(this.clientManager);
            Harbour.RedisSessionStateStore.RedisSessionStateStoreProvider.SetOptions(new Harbour.RedisSessionStateStore.RedisSessionStateStoreOptions()
            {
                KeySeparator = ":",
                OnDistributedLockNotAcquired = sessionId =>
                {
                    baseclass.Log.WriteTextLog("Application_Start", "Error",
                        "Session   could not establish distributed lock. " +
                        "This most likely means you have to increase the " +
                        "DistributedLockAcquireSeconds/DistributedLockTimeoutSeconds.");
                }
            });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            //this.clientManager.Dispose();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}