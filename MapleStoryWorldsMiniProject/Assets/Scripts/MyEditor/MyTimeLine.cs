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
        // ��ư �ݹ� ���
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
        // ����� ���� á����
        if (timeLine.Count >= maxSaveCount)
        {
            // ���� ó���� ������ ����� �����ϴ� ����
            // ó�� ����� ��ȿ���� üũ
            if (firstDoCell != null)
            {
                // ó�� ����� Ÿ�� ���ο��� ����
                timeLine.Remove(firstDoCell);

                // ó�� ����� ���� ����� �� ó�� ������� ����
                firstDoCell = firstDoCell.nextCell;

                // ���� ��ϵ� ó�� ����� ���� ����� ����
                firstDoCell.prevCell = null;
            }
            // ��ȿ���� ������ ����� �߸��ƴٰ� �����ϰ� �ʱ�ȭ
            else
            {
                timeLine.Clear();
                firstDoCell = null;
                prevDoCell = null;
                nextDoCell = null;
            }
        }

        // ���� üũ
        if (timeLine.Count < maxSaveCount)
        {
            // �ű� ��� ����
            var newCell = new MyTimeLineCell(isMake, tilePoint)
            {
                // ���� �������� ����� �ű� ����� ���� ������� ����
                prevCell = prevDoCell
            };

            // ���� ��ϵ��� ��� ����
            var targetCell = nextDoCell;
            var safeCount = 0;
            while (safeCount < maxSaveCount)
            {
                // ������ üũ
                if (targetCell == null)
                {
                    break;
                }
                else
                {
                    // ����� Ÿ�� ���ο��� ����
                    timeLine.Remove(targetCell);

                    // ���� ����� Ÿ������
                    targetCell = targetCell.nextCell;

                    // ���� ���� ���� ī��Ʈ ����
                    safeCount++;
                }
            }
            nextDoCell = null;

            // �ٷ� ���� ����� ���� ������ ������� ����
            if (prevDoCell != null)
            {
                prevDoCell.nextCell = newCell;
            }
            prevDoCell = newCell;

            // �ű� ����� Ÿ�� ���ο� �߰�
            timeLine.Add(newCell);
        }

        RefreshButtons();
    }

    /// <summary>
    /// ��ư ����
    /// </summary>
    public void RefreshButtons()
	{
        // ���� ��� ��ư ����
        if (unDoButton != null)
        {
            unDoButton.interactable = isActive && prevDoCell != null;
        }
        if (unDoText != null)
        {
            unDoText.color = isActive && prevDoCell != null ? ableColor : disableColor;
        }

        // ����� ��ư ����
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
    /// ���� ��Ҹ� ����
    /// </summary>
    public void UnDo()
    {
		if (isActive)
		{
            if (prevDoCell != null && poolParentTile != null)
            {
                // ���� ��� ����
                if (prevDoCell.isMake)
                {
                    poolParentTile.RemoveTile(prevDoCell.tilePoint);
                }
                else
                {
                    poolParentTile.CreateTile(prevDoCell.tilePoint);
                }

                // ��� ���� ����
                nextDoCell = prevDoCell;
                prevDoCell = prevDoCell.prevCell;
            }

            RefreshButtons();
        }
    }

    /// <summary>
    /// ������� ����
    /// </summary>
    public void ReDo()
	{
        if (isActive)
		{
            if (nextDoCell != null && poolParentTile != null)
            {
                // ����� ����
                if (nextDoCell.isMake)
                {
                    poolParentTile.CreateTile(nextDoCell.tilePoint);
                }
                else
                {
                    poolParentTile.RemoveTile(nextDoCell.tilePoint);
                }

                // ��� ���� ����
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
