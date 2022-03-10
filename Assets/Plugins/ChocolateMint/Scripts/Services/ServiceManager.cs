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
        /// サービスの登録（パラメータ付き）
        /// </summary>
        /// <typeparam name="TService">登録するサービスタイプ</typeparam>
        /// <typeparam name="TParameter">サービスに渡すパラメータタイプ</typeparam>
        /// <param name="parameter">サービスに渡すパラメータ</param>
        static public void RegisterService<TService, TParameter>(TParameter parameter) where TService : ServiceBase, IServiceParameter<TParameter>, new()
        {
            var service = new TService();
            service.PreStartup(parameter);
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