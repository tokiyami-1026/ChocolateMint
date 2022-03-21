namespace ChocolateMint.Service
{
    /// <summary>
    /// サービス登録時に外部からパラメータを渡せるようにするインターフェース
    /// </summary>
    /// <typeparam name="TParameter">パラメータのタイプ</typeparam>
    public interface IServiceParameter<TParameter>
    {
        /// <summary>
        /// セットアップ前に呼ばれる
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        void PreStartup(TParameter parameter);
    }
}
