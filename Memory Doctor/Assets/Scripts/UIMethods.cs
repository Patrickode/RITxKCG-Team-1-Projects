using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMethods : MonoBehaviour
{
    /// <summary>
    /// Loads a scene by its build index. If <paramref name="sceneIndex"/> < 0, <paramref name="sceneIndex"/>
    /// is negated and added to the current scene index. (-1 = one scene forward from the current scene)
    /// <br/>
    /// ビルドインデックスでシーンをロードします。<paramref name="sceneIndex"/> < 0の場合、
    /// <paramref name="sceneIndex"/>は否定され、現在のシーンインデックスに追加されます。
    /// (-1 = 現在のシーンから 1 つ前のシーン)
    /// </summary>
    /// <param name="sceneIndex">
    /// The index of the scene to load.
    /// <br/>
    /// ロードするシーンのインデックス
    /// </param>
    public void LoadSceneByIndex(int sceneIndex)
    {
        sceneIndex = sceneIndex < 0 ? SceneManager.GetActiveScene().buildIndex + -sceneIndex : sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

    /// <summary>
    /// Closes the game.
    /// <br/>
    /// ゲームを終了します。
    /// </summary>
    public void QuitGame() { Application.Quit(); }

    /// <summary>
    /// Enables an object.
    /// <br/>
    /// ゲームを終了します。
    /// </summary>
    public void EnableObject(GameObject objectToEnable) { SetObjectActive(objectToEnable, true); }
    /// <summary>
    /// Disables an object.
    /// <br/>
    /// ゲームを終了します。
    /// </summary>
    public void DisableObject(GameObject objectToEnable) { SetObjectActive(objectToEnable, false); }
    /// <summary>
    /// Sets an object's active state to true or false.
    /// <br/>
    /// オブジェクトのアクティブな状態を true または false に設定します。
    /// </summary>
    /// <param name="objectToSet">The object to set.<br/>設定するオブジェクト。</param>
    /// <param name="isActive">
    /// Should <paramref name="objectToSet"/> be active?
    /// <br/>
    /// <paramref name="objectToSet"/>をアクティブにすべきか？
    /// </param>
    public void SetObjectActive(GameObject objectToSet, bool isActive) { objectToSet.SetActive(isActive); }

    public void SetSceneState(int state) { SceneStateManager.SetStateWithInt(state); }
}
