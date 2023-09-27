using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParent_Tile : PoolParent
{
	#region PublicMember
	public float tileSize = 100f;
	public GamePlayer gamePlayer;
	public readonly Dictionary<Vector2, PoolChild_Tile> tileMapDic = new Dictionary<Vector2, PoolChild_Tile>();
	#endregion

	#region Public
	/// <summary>
	/// �ش� Ÿ�� ��ǥ�� Ÿ���� ����
	/// </summary>
	/// <param name="tilePoint">Ÿ�� ��ǥ</param>
	/// <returns>������ �����ϸ� true</returns>
	public bool CreateTile(Vector2 tilePoint)
	{
		// Ÿ���� �̹� �����ϴ��� üũ
		var tile = GetTile(tilePoint);

		// ���ٸ� ���� ���� �� ��ȯ
		if (tile == null)
		{
			tile = CreateChild<PoolChild_Tile>(0);
			if (tile != null)
			{
				// Ÿ�ϸʿ� �߰�
				tileMapDic.Add(tilePoint, tile);
				// Ÿ�� ��ġ ����
				tile.SetTilePoint(tilePoint);

				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// �ش� Ÿ�� ��ǥ�� Ÿ���� �ִٸ� ��ȯ�Ѵ�
	/// </summary>
	/// <param name="tilePoint">Ÿ�� ��ǥ</param>
	/// <returns></returns>
	public PoolChild_Tile GetTile(Vector2 tilePoint)
	{
		if (tileMapDic.TryGetValue(tilePoint, out var tile))
		{
			return tile;
		}
		return null;
	}

	/// <summary>
	/// �ش� Ÿ�� ��ǥ�� Ÿ���� �ִٸ� ����
	/// </summary>
	/// <param name="tilePoint">Ÿ�� ��ǥ</param>
	/// <returns>���ſ� �����ϸ� true</returns>
	public bool RemoveTile(Vector2 tilePoint)
	{
		// Ÿ���� �̹� �����ϴ��� üũ
		var tile = GetTile(tilePoint);

		// �����ϸ� ����
		if (tile != null)
		{
			// Ÿ�ϸʿ��� ����
			if (tileMapDic.ContainsKey(tilePoint))
			{
				tileMapDic.Remove(tilePoint);
			}

			// Ǯ ����
			RemoveChild(tile);

			return true;
		}

		return false;
	}
	#endregion
}
