using System;
using System.Collections.Generic;

namespace ChocolateMint.Service
{
    /// <summary>
    /// サービス管理クラス
    /// </summary>
    static public class ServiceManager
    {
        /// <summary>
        /// 全サービス
        /// </summary>
        static private Dictionary<Type, ServiceBase> services = new Dictionary<Type, ServiceBase>();

        /// <summary>
        /// サービスの登録
        /// </summary>
        static public void RegisterService<TService>() where TService : ServiceBase, new()
        {
            var service = new TService();
            services.Add(typeof(TService), service);

            // 登録されたサービスの開始処理をたたく
            service.Startup();
        }

        /// <summary>
        /// サービスの取得
        /// </summary>
        static public TService GetService<TService>() where TService : ServiceBase
        {
            return (TService)services[typeof(TService)];
        }

        /// <summary>
        /// サービスの終了処理
        /// </summary>
        static internal void Shutdown()
        {
            foreach (var service in services.Values)
            {
                service.Shutdown();
            }
        }
    }
}