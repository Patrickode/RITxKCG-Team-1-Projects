using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabMemory : MonoBehaviour
{
    [SerializeField] private GameObject mousePosPivot = null;
    private bool grabbedThisFrame = false;

    void Update()
    {
        if (SceneStateManager.CurrentState != SceneState.Game) { return; }

        //Get the mouse's position and raycast from the camera in the direction of the mouse's position.
        //マウスの位置を取得し、カメラからマウスの位置の方向にレイキャストします。
        Ray rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

        //If the raycast hits something,
        //レイキャストが何かに当たった場合、
        if (Physics.Raycast(rayFromMouse, out RaycastHit hitInfo, 1000))
        {
            mousePosPivot.transform.position = hitInfo.point + Vector3.up * 1f;

            if (hitInfo.transform.CompareTag("Memory"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hitInfo.transform.parent = hitInfo.transform.parent == null ? mousePosPivot.transform : null;
                    hitInfo.transform.gameObject.layer = 2;
                    grabbedThisFrame = true;
                }
            }
        }

        if (mousePosPivot.transform.childCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!grabbedThisFrame)
                {
                    foreach (Transform child in mousePosPivot.transform) { child.gameObject.layer = 0; }
                    mousePosPivot.transform.DetachChildren();
                }
            }
            else
            {
                foreach (Transform child in mousePosPivot.transform)
                {
                    child.localPosition = Vector3.zero;
                }
            }
        }

        grabbedThisFrame = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(mousePosPivot.transform.position, 0.2f);
    }
}
