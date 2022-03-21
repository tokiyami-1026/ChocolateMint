using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocolateMint.Common;
using ChocolateMint.Scene;

public class SandBoxSceneA : GameSceneController<DisplayContentModel, SandBoxSceneAView>
{
    public override string ContentAssetPath => "Assets/SandBox/Scenes/SandBoxSceneA.unity";

    public override void Terminate()
    {
        Debug.Log("Terminate A");
    }

    public override void Update()
    {
        Debug.Log("Update A");
    }
}
