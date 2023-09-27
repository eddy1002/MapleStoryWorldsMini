using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolChild_Tile : PoolChild
{


	#region PublicMember
	public GameObject footHoldParent;
	public GameObject footHoldLeft;
	public GameObject footHoldRight;
	public GameObject footHoldTop;
	public Vector2 tilePoint;
	#endregion

	#region PrivateMember
	private PoolParent_Tile parent = null;
	#endregion

	#region Override
	public override void ReleaseChild()
	{
		base.ReleaseChild();

		SetLeftTile(false);
		SetRightTile(false);
		SetTopTile(false);
		SetBottomTile(false);
	}
	#endregion

	#region Public
	public void SetTilePoint(Vector2 tilePoint)
	{
		if (transform.TryGetComponent<RectTransform>(out var rect))
		{
			this.tilePoint = tilePoint;
			rect.localPosition = tilePoint;

			SetLeftTile(true);
			SetRightTile(true);
			SetTopTile(true);
			SetBottomTile(true);
		}
	}

	public void SetFoolHold(GameObject footHold, bool active)
	{
		if (footHold != null && footHold.activeSelf != active)
		{
			footHold.SetActive(active);
		}
	}
	#endregion

	#region Private
	private PoolParent_Tile GetParent()
	{
		// �Ҵ��� ���� ������ ���� �Ҵ�
		if (parent == null)
		{
			if (transform.parent.TryGetComponent<PoolParent_Tile>(out var poolParent))
			{
				parent = poolParent;
			}
		}

		return parent;
	}

	private void SetLeftTile(bool active)
	{
		// ���� Ÿ�� �˻�
		var leftTilePoint = new Vector2(tilePoint.x - GetParent().tileSize, tilePoint.y);
		var leftTile = GetParent().GetTile(leftTilePoint);

		// �����ϸ� ����
		if (leftTile != null)
		{
			SetFoolHold(leftTile.footHoldRight, !active);
		}

		// �ڽ� Ÿ�� ����
		if (active)
		{
			SetFoolHold(footHoldLeft, leftTile == null);
		}
	}

	private void SetRightTile(bool active)
	{
		// ���� Ÿ�� �˻�
		var rightTilePoint = new Vector2(tilePoint.x + GetParent().tileSize, tilePoint.y);
		var rightTile = GetParent().GetTile(rightTilePoint);

		// �����ϸ� ����
		if (rightTile != null)
		{
			SetFoolHold(rightTile.footHoldLeft, !active);
		}

		// �ڽ� Ÿ�� ����
		if (active)
		{
			SetFoolHold(footHoldRight, rightTile == null);
		}
	}

	private void SetTopTile(bool active)
	{
		// ���� Ÿ�� �˻�
		var upTilePoint = new Vector2(tilePoint.x, tilePoint.y + GetParent().tileSize);
		var upTile = GetParent().GetTile(upTilePoint);

		// �ڽ� Ÿ�� ����
		if (active)
		{
			SetFoolHold(footHoldTop, upTile == null);
		}
	}

	private void SetBottomTile(bool active)
	{
		// �Ʒ��� Ÿ�� �˻�
		var bottomTilePoint = new Vector2(tilePoint.x, tilePoint.y - GetParent().tileSize);
		var bottomTile = GetParent().GetTile(bottomTilePoint);

		// �����ϸ� ����
		if (bottomTile != null)
		{
			SetFoolHold(bottomTile.footHoldTop, !active);
		}
	}
	#endregion
}
