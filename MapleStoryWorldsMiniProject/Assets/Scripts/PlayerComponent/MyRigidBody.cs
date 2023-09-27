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
        // 발이 지면에 붙어있을 때만 검사, 떨어져 있으면 이동 과정에서 갱신 됨
        if (isFootHold && rect != null)
        {
            var current = rect.localPosition;
            if (myCollider != null && poolParentTile != null)
            {
                // 좌측 끝에서 우측 끝까지 아래 타일 검사
                if (poolParentTile.tileSize > 0f)
                {
                    // 좌측 우측 끝 좌표 계산
                    var leftX = current.x - myCollider.left;
                    var rightX = current.x + myCollider.right;

                    // 좌표 증가하면서 순환
                    while (leftX <= rightX)
                    {
                        var downTile = poolParentTile.GetTile(GameHelper.UIPoint2TilePoint(new Vector2(leftX, current.y - poolParentTile.tileSize * 0.25f), poolParentTile.tileSize));

                        // 아래 타일이 존재하고 충돌 가능하면 갱신 취소
                        if (downTile != null && downTile.footHoldTop != null && downTile.footHoldTop.activeSelf)
                        {
                            return;
                        }

                        // 검사 끝
                        if (leftX == rightX)
                        {
                            break;
                        }

                        // 체크 좌표 증가
                        leftX = Mathf.Min(leftX + poolParentTile.tileSize, rightX);
                    }
                }

                // 발에 붙은 타일이 하나도 없으므로 갱신
                isFootHold = false;
            }
        }
    }

    private void MoveBody()
	{
        // 지면 갱신
        CheckFootHold();

        // 중력 적용
        if (!isFootHold)
        {
            currentVelocity.y -= gravity * Time.deltaTime;
        }

        // 수평 이동
        MoveBodyX();

        // 수직 이동
        MoveBodyY();
    }

    private void MoveBodyX()
	{
        if (currentVelocity.x != 0f && myCollider != null && poolParentTile != null)
        {
            var curX = rect.localPosition.x;
            var curY = rect.localPosition.y;
            var targetX = curX + currentVelocity.x * Time.deltaTime;

            // 발 끝에서 머리 끝까지 타일 검사
            if (poolParentTile.tileSize > 0f)
            {
                // 발 머리 끝 좌표 계산
                var bottomY = curY + 0.5f; // 실제 발이 땅에 붙으면 아래 타일이 검사되므로 살짝 위부터 체크
                var topY = curY + myCollider.top;

                // 좌표 증가하면서 순환
                while (bottomY <= topY)
                {
                    // 콜라이더 끝까지의 거리
                    float diff;

					// 좌측 이동은 좌측 콜라이더 길이 만큼 뺀다
					if (currentVelocity.x < 0f)
                    {
                        diff = -myCollider.left;
                    }
                    // 우측 이동은 우측 콜라이더 길이 만큼 더한다
                    else
                    {
                        diff = myCollider.right;
                    }

                    // 충돌하는 타일 좌표
                    var bumpTilePoint = GameHelper.UIPoint2TilePoint(new Vector2(targetX + diff, bottomY), poolParentTile.tileSize);

                    // 충돌 타일이 충돌 가능할 경우
                    var bumpTile = poolParentTile.GetTile(bumpTilePoint);
                    if (bumpTile != null)
                    {
                        // 좌측 이동으로 충돌
                        if (currentVelocity.x < 0f)
                        {
                            // 좌측 이동은 우측면에 충돌
                            if (bumpTile.footHoldRight != null && bumpTile.footHoldRight.activeSelf)
                            {
                                // 충돌이 일어나는 좌표 계산
                                var bumpX = bumpTilePoint.x + poolParentTile.tileSize * 0.5f;

                                // 충돌 좌표가 이동 경로 안에 있으면 충돌 체크 성공
                                if (bumpX >= targetX + diff && bumpX <= curX + diff)
                                {
                                    targetX = bumpX - diff;
                                    currentVelocity.x = 0f;
                                    break;
                                }
                            }
                        }
                        // 우측 이동으로 충돌
                        else if (currentVelocity.x > 0f)
                        {
                            // 우측 이동은 좌측면에 충돌
                            if (bumpTile.footHoldLeft != null && bumpTile.footHoldLeft.activeSelf)
                            {
                                // 충돌이 일어나는 좌표 계산
                                var bumpX = bumpTilePoint.x - poolParentTile.tileSize * 0.5f;

                                // 충돌 좌표가 이동 경로 안에 있으면 충돌 체크 성공
                                if (bumpX <= targetX + diff && bumpX >= curX + diff)
                                {
                                    targetX = bumpX - diff;
                                    currentVelocity.x = 0f;
                                    break;
                                }
                            }
                        }
                    }

                    // 검사 끝
                    if (bottomY == topY)
                    {
                        break;
                    }

                    // 체크 좌표 증가
                    bottomY = Mathf.Min(bottomY + poolParentTile.tileSize, topY);
                }
            }

            // 좌표 이동
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

            // 아래로 이동할 때만 지형 충돌 체크
            if (currentVelocity.y < 0f && myCollider != null && poolParentTile != null)
            {
                // 발이 땅에 붙어있으면 이동 불가
                if (!isFootHold)
                {
                    // 좌측 끝에서 우측 끝까지 타일 검사
                    if (poolParentTile.tileSize > 0f)
                    {
                        // 좌측 우측 끝 좌표 계산
                        var leftX = curX - myCollider.left + 0.5f; // 벽에 딱 붙으면 붙은 벽이 체크되므로 살짝 밖부터
                        var rightX = curX + myCollider.right - 0.5f; // 벽에 딱 붙으면 붙은 벽이 체크되므로 살짝 안까지

                        // 좌표 증가하면서 순환
                        while (leftX <= rightX)
                        {
                            // 충돌하는 타일 좌표
                            var bumpTilePoint = GameHelper.UIPoint2TilePoint(new Vector2(leftX, targetY), poolParentTile.tileSize);

                            // 충돌 타일이 충돌 가능할 경우
                            var bumpTile = poolParentTile.GetTile(bumpTilePoint);
                            if (bumpTile != null)
                            {
                                // 하강 이동은 윗쪽면에 충돌
                                if (bumpTile.footHoldTop != null && bumpTile.footHoldTop.activeSelf)
                                {
                                    // 충돌이 일어나는 좌표 계산
                                    var bumpY = bumpTilePoint.y + poolParentTile.tileSize * 0.5f;

                                    // 충돌 좌표가 이동 경로 안에 있으면 충돌 체크 성공
                                    if (bumpY >= targetY && bumpY <= curY)
                                    {
                                        targetY = bumpY;
                                        isFootHold = true;
                                        currentVelocity.y = 0f;
                                        break;
                                    }
                                }
                            }

                            // 검사 끝
                            if (leftX == rightX)
                            {
                                break;
                            }

                            // 체크 좌표 증가
                            leftX = Mathf.Min(leftX + poolParentTile.tileSize, rightX);
                        }
                    }
                }
            }
            // 위로 이동할 때는 발이 땅에서 떨어진 것으로 체크
            else if (currentVelocity.y > 0f)
            {
                isFootHold = false;
            }

            // 좌표 이동
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
