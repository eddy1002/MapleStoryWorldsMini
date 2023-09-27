using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyController : MonoBehaviour
{
    #region PublicMember
    public GamePlayer gamePlayer;
    public MyRigidBody myRigidBody;
    public Vector2 force = Vector2.zero;

    public bool isActive = false;
    #endregion

    #region PrivateMember
    private bool isAltDown = false;

    private float timeLeftDown = 0f;
    private float timeRightDown = 0f;
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckInputLeftDown();
            CheckInputRightDown();
            CheckInputAltDown();
            ControlPlayer();
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

    #region Private
    private void CheckInputLeftDown()
    {
        if (timeLeftDown == 0f && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            timeLeftDown = Time.realtimeSinceStartup;
        }
        else if (timeLeftDown > 0f && Input.GetKeyUp(KeyCode.LeftArrow))
        {
            timeLeftDown = 0f;
        }
    }

    private void CheckInputRightDown()
    {
        if (timeRightDown == 0f && Input.GetKeyDown(KeyCode.RightArrow))
        {
            timeRightDown = Time.realtimeSinceStartup;
        }
        else if (timeRightDown > 0f && Input.GetKeyUp(KeyCode.RightArrow))
        {
            timeRightDown = 0f;
        }
    }

    private void CheckInputAltDown()
    {
        if (!isAltDown && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isAltDown = true;
        }
        else if (isAltDown && Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isAltDown = false;
        }
    }

    private void ControlPlayer()
	{
        if (myRigidBody != null)
        {
			// 점프 명령 감지
			if (isAltDown)
			{
				// 지면에 발이 붙어있다면 점프 수행
				if (myRigidBody.isFootHold)
				{
                    myRigidBody.SetVelocityY(force.y);
                }
			}

            // 좌측 이동 명령 감지
            if (timeLeftDown > 0f)
            {
                // 동시 입력에서 좌측 이동이 더 과거에 입력됐다면 우측 이동 명령
                if (timeRightDown > 0f && timeRightDown > timeLeftDown)
                {
                    myRigidBody.SetVelocityX(force.x);
                    return;
                }
                // 아니면 좌측 이동 명령
                else
                {
                    myRigidBody.SetVelocityX(-force.x);
                }
            }
            // 우측 이동 명령 감지
            else if (timeRightDown > 0f)
            {
                // 동시 입력에 대한 처리는 위에서 처리했으므로 다른 처리 없음
                myRigidBody.SetVelocityX(force.x);
            }
			// 수평 이동 명령이 없으면 정지
			else
			{
                myRigidBody.SetVelocityX(0f);
            }
        }
	}
    #endregion

    #region Callback
    public void SetPlayMode(bool isPlay)
    {
        isActive = isPlay;
        timeLeftDown = 0f;
        timeRightDown = 0f;
        isAltDown = false;
    }
    #endregion
}
