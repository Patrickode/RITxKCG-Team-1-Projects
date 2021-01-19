using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingManager : MonoBehaviour
{
    [Tooltip("The object to use as a preview when a snowman can be built.\n\n" +
        "雪だるまを作成できるときにプレビューとして使用するオブジェクト。")]
    [SerializeField] private GameObject snowmanPreviewPrefab = null;
    [Tooltip("The color of the preview when the player is NOT allowed to build in the current spot.\n\n" +
        "プレーヤーが現在のスポットにビルドすることを許可されていない場合のプレビューの色。")]
    [SerializeField] private Color invalidColor = Color.red;
    /// <summary>
    /// The preview object that is currently being used. Null if no preview is being used right now.<br/>
    /// 現在使用されているプレビューオブジェクト。 現在プレビューが使用されていない場合はnull。
    /// </summary>
    private GameObject activePreview = null;
    /// <summary>
    /// Whether this script is prepared to create or destroy.<br/>
    /// このスクリプトが作成または破棄する準備ができているかどうか。
    /// </summary>
    private bool actionPrepared = false;

    public static Action<RaycastHit> TryCreateOrDestroy;
    private RaycastHit hitInfo;

    private Renderer previewRenderer = null;
    private MaterialPropertyBlock invalidColorProp;

    private void Start()
    {
        invalidColorProp = new MaterialPropertyBlock();
        invalidColorProp.SetColor("_Color", invalidColor);
    }

    /// <summary>
    /// This property is used to get activePreview, and spawn a preview if there wasn't one already.<br/>
    /// このプロパティは、activePreviewを取得し、プレビューがまだない場合はプレビューを生成するために使用されます。
    /// </summary>
    public GameObject SnowmanPreview
    {
        get
        {
            //When attempting to get SnowmanPreview, if there is no active preview object, make one.
            //SnowmanPreviewを取得しようとしたときに、アクティブなプレビューオブジェクトがない場合は、作成します。
            if (!activePreview) { activePreview = Instantiate(snowmanPreviewPrefab); }
            else if (!activePreview.activeSelf) { activePreview.SetActive(true); }
            return activePreview;
        }
    }
    public Renderer PreviewRenderer
    {
        get
        {
            if (!previewRenderer) { previewRenderer = SnowmanPreview.GetComponent<Renderer>(); }
            return previewRenderer;
        }
    }

    private void Update()
    {
        if (actionPrepared)
        {
            //Get the mouse's position and raycast from the camera in the direction of the mouse's position.
            //マウスの位置を取得し、カメラからマウスの位置の方向にレイキャストします。
            Ray rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If the raycast hits something,
            //レイキャストが何かに当たった場合、
            if (Physics.Raycast(rayFromMouse, out hitInfo, 1000))
            {
                //the player is in build mode,
                //プレーヤーはビルドモードであり、
                if (ModeManager.CurrentMode == PlayerMode.Build)
                {
                    //And, finally, if building the snowman here would not make it sideways,
                    //そして最後に、ここで雪だるまを建てても横向きにならない場合は、
                    if (Vector3.Dot(Vector3.up, hitInfo.normal) > 0.4f)
                    {
                        //Spawn a snowman preview if one is not already spawned (see SnowmanPreview's get 
                        //function) and position it at the point where the raycast hit.
                        //スノーマンプレビューがまだスポーンされていない場合はスポーンします（SnowmanPreviewのget
                        //関数を参照）そしてレイキャストが当たるポイントに配置します。
                        Vector3 previewPos = hitInfo.point;
                        previewPos += hitInfo.normal * SnowmanPreview.transform.localScale.y / 2;
                        SnowmanPreview.transform.position = previewPos;
                        SnowmanPreview.transform.up = hitInfo.normal;

                        //Change the color of the snowman preview depending on whether the player could build
                        //a snowman here.
                        //プレイヤーがここで雪だるまを作成できるかどうかに応じて、雪だるまのプレビューの色を変更します。
                        if (hitInfo.transform.CompareTag("NoBuild"))
                        {
                            PreviewRenderer.SetPropertyBlock(invalidColorProp);
                        }
                        else if (hitInfo.transform.CompareTag("Snowman")) { SnowmanPreview.SetActive(false); }
                        else { PreviewRenderer.SetPropertyBlock(null); }
                    }
                    //If building the snowman here would make it sideways, don't show the build preview.
                    ////ここでスノーマンをビルドすると横向きになる場合は、ビルドプレビューを表示しないでください。
                    else
                    {
                        SnowmanPreview.transform.localScale = snowmanPreviewPrefab.transform.localScale;
                        SnowmanPreview.SetActive(false);
                    }
                }
                else if (hitInfo.transform.CompareTag("Snowman"))
                {
                    SnowmanPreview.transform.position = hitInfo.transform.position;
                    SnowmanPreview.transform.localScale = hitInfo.transform.localScale * 1.1f;
                    PreviewRenderer.SetPropertyBlock(invalidColorProp);
                }
                else
                {
                    SnowmanPreview.transform.localScale = snowmanPreviewPrefab.transform.localScale;
                    SnowmanPreview.SetActive(false);
                }
            }
            else
            {
                SnowmanPreview.transform.localScale = snowmanPreviewPrefab.transform.localScale;
                SnowmanPreview.SetActive(false);
            }
        }

        //If the player clicks the primary mouse button,
        //プレーヤーがマウスの主ボタンをクリックした場合、
        if (Input.GetMouseButtonDown(0))
        {
            //and an action is prepared,
            //そしてアクションが準備されます、
            if (actionPrepared)
            {
                TryCreateOrDestroy?.Invoke(hitInfo);
                SnowmanPreview.SetActive(false);
                actionPrepared = false;
            }
            else
            {
                actionPrepared = true;
            }
        }
        //Click secondary (right) mouse button to cancel.
        //キャンセルするには、マウスの2番目（右）ボタンをクリックします。
        else if (Input.GetMouseButtonDown(1))
        {
            actionPrepared = false;
            SnowmanPreview.SetActive(false);
        }
    }
}
