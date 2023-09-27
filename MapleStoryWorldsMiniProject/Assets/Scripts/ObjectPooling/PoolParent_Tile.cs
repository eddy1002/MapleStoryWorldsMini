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
	/// 해당 타일 좌표에 타일을 생성
	/// </summary>
	/// <param name="tilePoint">타일 좌표</param>
	/// <returns>생성에 성공하면 true</returns>
	public bool CreateTile(Vector2 tilePoint)
	{
		// 타일이 이미 존재하는지 체크
		var tile = GetTile(tilePoint);

		// 없다면 새로 생성 후 반환
		if (tile == null)
		{
			tile = CreateChild<PoolChild_Tile>(0);
			if (tile != null)
			{
				// 타일맵에 추가
				tileMapDic.Add(tilePoint, tile);
				// 타일 위치 설정
				tile.SetTilePoint(tilePoint);

				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 해당 타일 좌표에 타일이 있다면 반환한다
	/// </summary>
	/// <param name="tilePoint">타일 좌표</param>
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
	/// 해당 타일 좌표에 타일이 있다면 제거
	/// </summary>
	/// <param name="tilePoint">타일 좌표</param>
	/// <returns>제거에 성공하면 true</returns>
	public bool RemoveTile(Vector2 tilePoint)
	{
		// 타일이 이미 존재하는지 체크
		var tile = GetTile(tilePoint);

		// 존재하면 제거
		if (tile != null)
		{
			// 타일맵에서 제거
			if (tileMapDic.ContainsKey(tilePoint))
			{
				tileMapDic.Remove(tilePoint);
			}

			// 풀 해제
			RemoveChild(tile);

			return true;
		}

		return false;
	}
	#endregion
}
