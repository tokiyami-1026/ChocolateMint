using ChocolateMint.Scene;
using ChocolateMint.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneTransitionTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var transitionService = ServiceManager.GetService<GameSceneManagementService>();
            transitionService.RequestTransitionGameScene<SandBoxSceneB,int>(1234567);
        }
    }
}
