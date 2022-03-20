using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocolateMint.Scene;
using ChocolateMint.Common;
using UniRx;

public class SandBoxSceneB : GameSceneController<SandBoxSceneBModel,SandBoxSceneBView>, IGameSceneParameter<int>
{
    public override string ContentAssetPath => "Assets/SandBox/Scenes/SandBoxSceneB.unity";

    public void PreInitialize(int sceneParameter)
    {
        Debug.Log(sceneParameter);
    }

    public override void Initialize()
    {
        Debug.Log("Initialize");
    }
}
