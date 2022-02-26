using UnityEngine;
using ChocolateMint;

public class MyGameMain
{
    static private readonly GameMain GameMain = new GameMain();

    /// <summary>
    /// ゲームのエントリポイント
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void Main()
    {
        // ライブラリを起動する
        GameMain.Run();

        // ゲーム終了時のコールバックを登録しておく
        Application.quitting += OnQuitGame;
    }

    /// <summary>
    /// ゲームが終了した
    /// </summary>
    private static void OnQuitGame()
    {
        // ライブラリの終了処理
        GameMain.Shutdown();

        Application.quitting -= OnQuitGame;
    }
}
