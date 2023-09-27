using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParent : MonoBehaviour
{
    #region PublicMember
    public List<string> pathList = new List<string>();
	#endregion

	#region Public
	public virtual T CreateChild<T>(int id) where T : PoolChild
	{
		// 유효 아이디 체크
		if (id < 0 || id >= pathList.Count)
		{
			return null;
		}

		// 기존 풀에서 검색
		for (int index = 0; index < transform.childCount; index++)
		{
			var child = transform.GetChild(index);
			// null 체크 및 미사용 체크
			if (child != null && !child.gameObject.activeSelf && child.TryGetComponent<T>(out var poolChild))
			{
				// 아이디 체크
				if (poolChild.id == id)
				{
					poolChild.InitChild();
					return poolChild;
				}
			}
		}

		// 신규 생성
		var resource = Resources.Load<GameObject>(pathList[id]);
		if (resource != null)
		{
			var newChild = Instantiate(resource, transform);
			if (newChild != null && newChild.TryGetComponent<T>(out var newPoolChild))
			{
				newPoolChild.InitChild();
				return newPoolChild;
			}
		}

		return null;
	}

	public virtual void RemoveChild<T>(T poolChild) where T : PoolChild
	{
		if (poolChild != null && poolChild.gameObject.activeSelf)
		{
			poolChild.ReleaseChild();
		}
	}
	#endregion
}
