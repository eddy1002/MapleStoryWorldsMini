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
			// ���� ��� ����
			if (isAltDown)
			{
				// ���鿡 ���� �پ��ִٸ� ���� ����
				if (myRigidBody.isFootHold)
				{
                    myRigidBody.SetVelocityY(force.y);
                }
			}

            // ���� �̵� ��� ����
            if (timeLeftDown > 0f)
            {
                // ���� �Է¿��� ���� �̵��� �� ���ſ� �Էµƴٸ� ���� �̵� ���
                if (timeRightDown > 0f && timeRightDown > timeLeftDown)
                {
                    myRigidBody.SetVelocityX(force.x);
                    return;
                }
                // �ƴϸ� ���� �̵� ���
                else
                {
                    myRigidBody.SetVelocityX(-force.x);
                }
            }
            // ���� �̵� ��� ����
            else if (timeRightDown > 0f)
            {
                // ���� �Է¿� ���� ó���� ������ ó�������Ƿ� �ٸ� ó�� ����
                myRigidBody.SetVelocityX(force.x);
            }
			// ���� �̵� ����� ������ ����
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
