using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAndDestroy : MonoBehaviour
{
    [Tooltip("The object to use as a preview when a snowman can be built.\n\n" +
        "雪だるまを作成できるときにプレビューとして使用するオブジェクト。")]
    [SerializeField] private GameObject snowmanPreviewPrefab = null;
    [Tooltip("The color of the preview when the player is NOT allowed to build in the current spot.\n\n" +
        "プレーヤーが現在のスポットでビルドできる場合のプレビューの色。")]
    [SerializeField] private Color validColor = Color.green;
    [Tooltip("The color of the preview when the player is NOT allowed to build in the current spot.\n\n" +
        "プレーヤーが現在のスポットにビルドすることを許可されていない場合のプレビューの色。")]
    [SerializeField] private Color invalidColor = Color.red;
    [Tooltip("The prefab to use when building a snowman.\n\n" +
        "雪だるまを作るときに使用するプレハブ。")]
    [SerializeField] private GameObject buildSnowmanPrefab = null;
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
    /// <summary>
    /// If the player is in build mode, this is true. If they are in destroy mode, this is false.<br/>
    /// プレーヤーがビルドモードの場合、これは当てはまります。 それらが破棄モードの場合、これは誤りです。
    /// </summary>
    private bool inBuildMode = true;
    private bool canBuild = true;

    private Renderer previewRenderer = null;
    private MaterialPropertyBlock validColorProp;
    private MaterialPropertyBlock invalidColorProp;

    private void Start()
    {
        validColorProp = new MaterialPropertyBlock();
        invalidColorProp = new MaterialPropertyBlock();
        validColorProp.SetColor("_Color", validColor);
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
            if (Physics.Raycast(rayFromMouse, out RaycastHit hitInfo, 1000))
            {
                //the player is in build mode, and building is allowed on that object,
                //プレーヤーはビルドモードであり、そのオブジェクトでのビルドが許可されています、
                if (inBuildMode)
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

                        if (!hitInfo.transform.CompareTag("NoBuild"))
                        {
                            PreviewRenderer.SetPropertyBlock(validColorProp);
                            canBuild = true;
                        }
                        else
                        {
                            PreviewRenderer.SetPropertyBlock(invalidColorProp);
                            canBuild = false;
                        }
                    }
                    else
                    {
                        SnowmanPreview.SetActive(false);
                        canBuild = false;
                    }
                }
                else
                {
                    SnowmanPreview.SetActive(false);

                    //TODO: Destroy Mode
                }
            }
            else { SnowmanPreview.SetActive(false); }
        }

        //If the player clicks the primary mouse button,
        //プレーヤーがマウスの主ボタンをクリックした場合、
        if (Input.GetMouseButtonDown(0))
        {
            //and an action is prepared,
            //そしてアクションが準備されます、
            if (actionPrepared)
            {
                //Do that action, whether it is building or destroying.
                //構築中か破壊中かにかかわらず、そのアクションを実行します。
                if (inBuildMode && canBuild)
                {
                    Instantiate(
                        buildSnowmanPrefab,
                        SnowmanPreview.transform.position,
                        SnowmanPreview.transform.rotation
                    );
                }
                else if (!inBuildMode)
                {
                    //TODO: Destroy Mode
                }
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
