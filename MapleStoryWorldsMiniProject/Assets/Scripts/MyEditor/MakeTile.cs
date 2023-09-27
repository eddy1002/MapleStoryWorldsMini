using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTile : MonoBehaviour
{
    #region PublicMember
    public GamePlayer gamePlayer;
    public Camera mainCam;

    public RectTransform canvas;
    public RectTransform scene;

    public MyTimeLine myTimeLine;
    public PoolParent_Tile poolParentTile;

    public float tileSize = 100f;
    public bool isActive = false;
    #endregion

    #region PrivateMember
    private bool isLeftClick = false;
    private bool isRightClick = false;
	#endregion

	// Update is called once per frame
	void Update()
    {
        CheckInput();
        CreateTileInEditor();
        RemoveTileInEditor();
    }

    private void OnEnable()
    {
        if (gamePlayer != null)
        {
            gamePlayer.onPlayChanged -= SetPlayMode;
            gamePlayer.onPlayChanged += SetPlayMode;
            SetPlayMode(gamePlayer.isPlay);
        }
    }

    private void OnDisable()
    {
        if (gamePlayer != null)
        {
            gamePlayer.onPlayChanged -= SetPlayMode;
        }
    }

    private void CheckInput()
    {
        if (isActive)
        {
            // 좌클릭 인식
            if (Input.GetMouseButtonDown(0))
            {
                isLeftClick = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isLeftClick = false;
            }

            // 우클릭 인식
            if (Input.GetMouseButtonDown(1))
            {
                isRightClick = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isRightClick = false;
            }
        }
        else
        {
            isLeftClick = false;
            isRightClick = false;
        }
    }

    private void CreateTileInEditor()
    {
        // 타일 생성
        if (isActive && isLeftClick)
        {
            if (poolParentTile != null)
            {
                var uiPoint = GameHelper.ScreenToUIPoint(mainCam, canvas, scene, Input.mousePosition);
                if (GameHelper.IsInScene(uiPoint, canvas, scene))
                {
                    var tilePoint = GameHelper.UIPoint2TilePoint(uiPoint, tileSize);
                    if (poolParentTile.CreateTile(tilePoint))
                    {
                        // 타임라인 기록
                        if (myTimeLine != null)
                        {
                            myTimeLine.SaveTimeLine(true, tilePoint);
                        }
                    }
                }
            }
        }
    }

    private void RemoveTileInEditor()
    {
        // 타일 제거
        if (isActive && isRightClick)
        {
            if (poolParentTile != null)
            {
                var uiPoint = GameHelper.ScreenToUIPoint(mainCam, canvas, scene, Input.mousePosition);
                if (GameHelper.IsInScene(uiPoint, canvas, scene))
                {
                    var tilePoint = GameHelper.UIPoint2TilePoint(uiPoint, tileSize);
                    if (poolParentTile.RemoveTile(tilePoint))
                    {
                        // 타임라인 기록
                        if (myTimeLine != null)
                        {
                            myTimeLine.SaveTimeLine(false, tilePoint);
                        }
                    }
                }
            }
        }
    }

    #region Callback
    public void SetPlayMode(bool isPlay)
    {
        isActive = !isPlay;
    }
    #endregion
}
