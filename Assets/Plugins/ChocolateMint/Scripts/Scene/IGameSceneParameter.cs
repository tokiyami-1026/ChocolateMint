
/// <summary>
/// シーン遷移時に外部からパラメータを渡せるようにするインターフェース
/// </summary>
/// <typeparam name="TSceneParameter">パラメータのタイプ</typeparam>
public interface IGameSceneParameter<TSceneParameter>
{
    /// <summary>
    /// 初期化前に呼ばれる
    /// </summary>
    /// <param name="sceneParameter">パラメータ</param>
    void PreInitialize(TSceneParameter sceneParameter);
}
