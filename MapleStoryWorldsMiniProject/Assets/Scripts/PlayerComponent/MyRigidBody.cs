using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidBody : MonoBehaviour
{
    #region PublicMember
    public GamePlayer gamePlayer;
    public PoolParent_Tile poolParentTile;
    public MyCollider myCollider;
    public RectTransform rect;

    public float gravity = 10f;

    public bool isFootHold = false;
    public bool isActive = false;
    #endregion

    #region PrivateMember
    private Vector2 currentVelocity = Vector2.zero;
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            MoveBody();
        }
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

    #region Public
    public void SetVelocityX(float value)
    {
        currentVelocity.x = value;
    }

    public void SetVelocityY(float value)
    {
        currentVelocity.y = value;
    }
    #endregion

    #region Private
    private void CheckFootHold()
    {
        // ���� ���鿡 �پ����� ���� �˻�, ������ ������ �̵� �������� ���� ��
        if (isFootHold && rect != null)
        {
            var current = rect.localPosition;
            if (myCollider != null && poolParentTile != null)
            {
                // ���� ������ ���� ������ �Ʒ� Ÿ�� �˻�
                if (poolParentTile.tileSize > 0f)
                {
                    // ���� ���� �� ��ǥ ���
                    var leftX = current.x - myCollider.left;
                    var rightX = current.x + myCollider.right;

                    // ��ǥ �����ϸ鼭 ��ȯ
                    while (leftX <= rightX)
                    {
                        var downTile = poolParentTile.GetTile(GameHelper.UIPoint2TilePoint(new Vector2(leftX, current.y - poolParentTile.tileSize * 0.25f), poolParentTile.tileSize));

                        // �Ʒ� Ÿ���� �����ϰ� �浹 �����ϸ� ���� ���
                        if (downTile != null && downTile.footHoldTop != null && downTile.footHoldTop.activeSelf)
                        {
                            return;
                        }

                        // �˻� ��
                        if (leftX == rightX)
                        {
                            break;
                        }

                        // üũ ��ǥ ����
                        leftX = Mathf.Min(leftX + poolParentTile.tileSize, rightX);
                    }
                }

                // �߿� ���� Ÿ���� �ϳ��� �����Ƿ� ����
                isFootHold = false;
            }
        }
    }

    private void MoveBody()
	{
        // ���� ����
        CheckFootHold();

        // �߷� ����
        if (!isFootHold)
        {
            currentVelocity.y -= gravity * Time.deltaTime;
        }

        // ���� �̵�
        MoveBodyX();

        // ���� �̵�
        MoveBodyY();
    }

    private void MoveBodyX()
	{
        if (currentVelocity.x != 0f && myCollider != null && poolParentTile != null)
        {
            var curX = rect.localPosition.x;
            var curY = rect.localPosition.y;
            var targetX = curX + currentVelocity.x * Time.deltaTime;

            // �� ������ �Ӹ� ������ Ÿ�� �˻�
            if (poolParentTile.tileSize > 0f)
            {
                // �� �Ӹ� �� ��ǥ ���
                var bottomY = curY + 0.5f; // ���� ���� ���� ������ �Ʒ� Ÿ���� �˻�ǹǷ� ��¦ ������ üũ
                var topY = curY + myCollider.top;

                // ��ǥ �����ϸ鼭 ��ȯ
                while (bottomY <= topY)
                {
                    // �ݶ��̴� �������� �Ÿ�
                    float diff;

					// ���� �̵��� ���� �ݶ��̴� ���� ��ŭ ����
					if (currentVelocity.x < 0f)
                    {
                        diff = -myCollider.left;
                    }
                    // ���� �̵��� ���� �ݶ��̴� ���� ��ŭ ���Ѵ�
                    else
                    {
                        diff = myCollider.right;
                    }

                    // �浹�ϴ� Ÿ�� ��ǥ
                    var bumpTilePoint = GameHelper.UIPoint2TilePoint(new Vector2(targetX + diff, bottomY), poolParentTile.tileSize);

                    // �浹 Ÿ���� �浹 ������ ���
                    var bumpTile = poolParentTile.GetTile(bumpTilePoint);
                    if (bumpTile != null)
                    {
                        // ���� �̵����� �浹
                        if (currentVelocity.x < 0f)
                        {
                            // ���� �̵��� �����鿡 �浹
                            if (bumpTile.footHoldRight != null && bumpTile.footHoldRight.activeSelf)
                            {
                                // �浹�� �Ͼ�� ��ǥ ���
                                var bumpX = bumpTilePoint.x + poolParentTile.tileSize * 0.5f;

                                // �浹 ��ǥ�� �̵� ��� �ȿ� ������ �浹 üũ ����
                                if (bumpX >= targetX + diff && bumpX <= curX + diff)
                                {
                                    targetX = bumpX - diff;
                                    currentVelocity.x = 0f;
                                    break;
                                }
                            }
                        }
                        // ���� �̵����� �浹
                        else if (currentVelocity.x > 0f)
                        {
                            // ���� �̵��� �����鿡 �浹
                            if (bumpTile.footHoldLeft != null && bumpTile.footHoldLeft.activeSelf)
                            {
                                // �浹�� �Ͼ�� ��ǥ ���
                                var bumpX = bumpTilePoint.x - poolParentTile.tileSize * 0.5f;

                                // �浹 ��ǥ�� �̵� ��� �ȿ� ������ �浹 üũ ����
                                if (bumpX <= targetX + diff && bumpX >= curX + diff)
                                {
                                    targetX = bumpX - diff;
                                    currentVelocity.x = 0f;
                                    break;
                                }
                            }
                        }
                    }

                    // �˻� ��
                    if (bottomY == topY)
                    {
                        break;
                    }

                    // üũ ��ǥ ����
                    bottomY = Mathf.Min(bottomY + poolParentTile.tileSize, topY);
                }
            }

            // ��ǥ �̵�
            rect.localPosition = new Vector2(targetX, curY);
        }
    }

    private void MoveBodyY()
	{
        if (currentVelocity.y != 0f)
        {
            var curX = rect.localPosition.x;
            var curY = rect.localPosition.y;
            var targetY = curY + currentVelocity.y * Time.deltaTime;

            // �Ʒ��� �̵��� ���� ���� �浹 üũ
            if (currentVelocity.y < 0f && myCollider != null && poolParentTile != null)
            {
                // ���� ���� �پ������� �̵� �Ұ�
                if (!isFootHold)
                {
                    // ���� ������ ���� ������ Ÿ�� �˻�
                    if (poolParentTile.tileSize > 0f)
                    {
                        // ���� ���� �� ��ǥ ���
                        var leftX = curX - myCollider.left + 0.5f; // ���� �� ������ ���� ���� üũ�ǹǷ� ��¦ �ۺ���
                        var rightX = curX + myCollider.right - 0.5f; // ���� �� ������ ���� ���� üũ�ǹǷ� ��¦ �ȱ���

                        // ��ǥ �����ϸ鼭 ��ȯ
                        while (leftX <= rightX)
                        {
                            // �浹�ϴ� Ÿ�� ��ǥ
                            var bumpTilePoint = GameHelper.UIPoint2TilePoint(new Vector2(leftX, targetY), poolParentTile.tileSize);

                            // �浹 Ÿ���� �浹 ������ ���
                            var bumpTile = poolParentTile.GetTile(bumpTilePoint);
                            if (bumpTile != null)
                            {
                                // �ϰ� �̵��� ���ʸ鿡 �浹
                                if (bumpTile.footHoldTop != null && bumpTile.footHoldTop.activeSelf)
                                {
                                    // �浹�� �Ͼ�� ��ǥ ���
                                    var bumpY = bumpTilePoint.y + poolParentTile.tileSize * 0.5f;

                                    // �浹 ��ǥ�� �̵� ��� �ȿ� ������ �浹 üũ ����
                                    if (bumpY >= targetY && bumpY <= curY)
                                    {
                                        targetY = bumpY;
                                        isFootHold = true;
                                        currentVelocity.y = 0f;
                                        break;
                                    }
                                }
                            }

                            // �˻� ��
                            if (leftX == rightX)
                            {
                                break;
                            }

                            // üũ ��ǥ ����
                            leftX = Mathf.Min(leftX + poolParentTile.tileSize, rightX);
                        }
                    }
                }
            }
            // ���� �̵��� ���� ���� ������ ������ ������ üũ
            else if (currentVelocity.y > 0f)
            {
                isFootHold = false;
            }

            // ��ǥ �̵�
            rect.localPosition = new Vector2(curX, targetY);
        }
    }
    #endregion

    #region Callback
    public void SetPlayMode(bool isPlay)
    {
        isActive = isPlay;
        if (!isPlay)
        {
            if (rect != null)
            {
                rect.localPosition = Vector2.zero;
            }
            currentVelocity = Vector2.zero;
            isFootHold = false;
        }
    }
    #endregion
}
