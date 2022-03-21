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
        Debug.Log($"PreInitialize B : {sceneParameter}");
    }

    public override void Initialize()
    {
        Debug.Log("Initialize B");
    }

    public override void Update()
    {
        Debug.Log("Update B");
    }
}
