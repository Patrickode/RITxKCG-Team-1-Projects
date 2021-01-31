using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneState
{
    Event,
    Game,
    Confirmation,
    Results
}

public class SceneStateManager : MonoBehaviour
{
    private static SceneState currentState = SceneState.Event;
    public static SceneState CurrentState { get { return currentState; } private set { currentState = value; } }

    public static void SetStateWithInt(int stateInt)
    {
        //Thanks https://stackoverflow.com/a/29489
        if (Enum.IsDefined(typeof(SceneState), stateInt))
        {
            CurrentState = (SceneState)stateInt;
        }
    }
}
