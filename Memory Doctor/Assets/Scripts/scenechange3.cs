using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenechange3 : MonoBehaviour
{
    public void gamestart(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
