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
		// 할당한 것이 없으면 새로 할당
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
		// 좌측 타일 검색
		var leftTilePoint = new Vector2(tilePoint.x - GetParent().tileSize, tilePoint.y);
		var leftTile = GetParent().GetTile(leftTilePoint);

		// 존재하면 세팅
		if (leftTile != null)
		{
			SetFoolHold(leftTile.footHoldRight, !active);
		}

		// 자신 타일 세팅
		if (active)
		{
			SetFoolHold(footHoldLeft, leftTile == null);
		}
	}

	private void SetRightTile(bool active)
	{
		// 우측 타일 검색
		var rightTilePoint = new Vector2(tilePoint.x + GetParent().tileSize, tilePoint.y);
		var rightTile = GetParent().GetTile(rightTilePoint);

		// 존재하면 세팅
		if (rightTile != null)
		{
			SetFoolHold(rightTile.footHoldLeft, !active);
		}

		// 자신 타일 세팅
		if (active)
		{
			SetFoolHold(footHoldRight, rightTile == null);
		}
	}

	private void SetTopTile(bool active)
	{
		// 위측 타일 검색
		var upTilePoint = new Vector2(tilePoint.x, tilePoint.y + GetParent().tileSize);
		var upTile = GetParent().GetTile(upTilePoint);

		// 자신 타일 세팅
		if (active)
		{
			SetFoolHold(footHoldTop, upTile == null);
		}
	}

	private void SetBottomTile(bool active)
	{
		// 아래측 타일 검색
		var bottomTilePoint = new Vector2(tilePoint.x, tilePoint.y - GetParent().tileSize);
		var bottomTile = GetParent().GetTile(bottomTilePoint);

		// 존재하면 세팅
		if (bottomTile != null)
		{
			SetFoolHold(bottomTile.footHoldTop, !active);
		}
	}
	#endregion
}
