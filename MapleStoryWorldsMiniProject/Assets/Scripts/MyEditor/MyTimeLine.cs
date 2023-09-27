using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyTimeLine : MonoBehaviour
{
    #region PublicMember
    public GamePlayer gamePlayer;
    public PoolParent_Tile poolParentTile;
    public Button unDoButton;
    public Button reDoButton;
    public TextMeshProUGUI unDoText;
    public TextMeshProUGUI reDoText;
    public int maxSaveCount = 10;
    public bool isActive = false;
    public Color ableColor = Color.black;
    public Color disableColor = Color.gray;
    #endregion

    #region PrivateMember
    private readonly List<MyTimeLineCell> timeLine = new List<MyTimeLineCell>();
    private MyTimeLineCell firstDoCell = null;
    private MyTimeLineCell prevDoCell = null;
    private MyTimeLineCell nextDoCell = null;
    #endregion

    #region Mono
    private void Awake()
    {
        // 버튼 콜백 등록
        if (unDoButton != null)
        {
            unDoButton.onClick.RemoveAllListeners();
            unDoButton.onClick.AddListener(UnDo);
        }
        if (reDoButton != null)
        {
            reDoButton.onClick.RemoveAllListeners();
            reDoButton.onClick.AddListener(ReDo);
        }

        RefreshButtons();
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
    #endregion

    #region Public
    public void SaveTimeLine(bool isMake, Vector2 tilePoint)
    {
        // 기록이 가득 찼을때
        if (timeLine.Count >= maxSaveCount)
        {
            // 가장 처음에 저정된 기록을 제거하는 과정
            // 처음 기록이 유효한지 체크
            if (firstDoCell != null)
            {
                // 처음 기록을 타임 라인에서 제거
                timeLine.Remove(firstDoCell);

                // 처음 기록의 다음 기록을 맨 처음 기록으로 갱신
                firstDoCell = firstDoCell.nextCell;

                // 새로 등록된 처음 기록의 이전 기록은 제거
                firstDoCell.prevCell = null;
            }
            // 유효하지 않으면 기록이 잘못됐다고 가정하고 초기화
            else
            {
                timeLine.Clear();
                firstDoCell = null;
                prevDoCell = null;
                nextDoCell = null;
            }
        }

        // 갯수 체크
        if (timeLine.Count < maxSaveCount)
        {
            // 신규 기록 생성
            var newCell = new MyTimeLineCell(isMake, tilePoint)
            {
                // 현재 포인터의 기록을 신규 기록의 이전 기록으로 갱신
                prevCell = prevDoCell
            };

            // 다음 기록들을 모두 제거
            var targetCell = nextDoCell;
            var safeCount = 0;
            while (safeCount < maxSaveCount)
            {
                // 마지막 체크
                if (targetCell == null)
                {
                    break;
                }
                else
                {
                    // 기록을 타임 라인에서 제거
                    timeLine.Remove(targetCell);

                    // 다음 기록을 타겟으로
                    targetCell = targetCell.nextCell;

                    // 무한 루프 방지 카운트 증가
                    safeCount++;
                }
            }
            nextDoCell = null;

            // 바로 이전 기록을 새로 생성된 기록으로 갱신
            if (prevDoCell != null)
            {
                prevDoCell.nextCell = newCell;
            }
            prevDoCell = newCell;

            // 신규 기록을 타임 라인에 추가
            timeLine.Add(newCell);
        }

        RefreshButtons();
    }

    /// <summary>
    /// 버튼 갱신
    /// </summary>
    public void RefreshButtons()
	{
        // 실행 취소 버튼 갱신
        if (unDoButton != null)
        {
            unDoButton.interactable = isActive && prevDoCell != null;
        }
        if (unDoText != null)
        {
            unDoText.color = isActive && prevDoCell != null ? ableColor : disableColor;
        }

        // 재실행 버튼 갱신
        if (reDoButton != null)
        {
            reDoButton.interactable = isActive && nextDoCell != null;
        }
        if (reDoText != null)
        {
            reDoText.color = isActive && nextDoCell != null ? ableColor : disableColor;
        }
    }

    /// <summary>
    /// 실행 취소를 수행
    /// </summary>
    public void UnDo()
    {
		if (isActive)
		{
            if (prevDoCell != null && poolParentTile != null)
            {
                // 실행 취소 수행
                if (prevDoCell.isMake)
                {
                    poolParentTile.RemoveTile(prevDoCell.tilePoint);
                }
                else
                {
                    poolParentTile.CreateTile(prevDoCell.tilePoint);
                }

                // 기록 저장 갱신
                nextDoCell = prevDoCell;
                prevDoCell = prevDoCell.prevCell;
            }

            RefreshButtons();
        }
    }

    /// <summary>
    /// 재실행을 수행
    /// </summary>
    public void ReDo()
	{
        if (isActive)
		{
            if (nextDoCell != null && poolParentTile != null)
            {
                // 재실행 수행
                if (nextDoCell.isMake)
                {
                    poolParentTile.CreateTile(nextDoCell.tilePoint);
                }
                else
                {
                    poolParentTile.RemoveTile(nextDoCell.tilePoint);
                }

                // 기록 저장 갱신
                prevDoCell = nextDoCell;
                nextDoCell = nextDoCell.nextCell;
            }

            RefreshButtons();
        } 
    }
    #endregion

    #region Callback
    public void SetPlayMode(bool isPlay)
    {
        isActive = !isPlay;
        RefreshButtons();
    }
    #endregion
}
