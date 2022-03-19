using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSceneParameter<TSceneParameter>
{
    void PreInitialize(TSceneParameter sceneParameter);
}
