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
		// ��ȿ ���̵� üũ
		if (id < 0 || id >= pathList.Count)
		{
			return null;
		}

		// ���� Ǯ���� �˻�
		for (int index = 0; index < transform.childCount; index++)
		{
			var child = transform.GetChild(index);
			// null üũ �� �̻�� üũ
			if (child != null && !child.gameObject.activeSelf && child.TryGetComponent<T>(out var poolChild))
			{
				// ���̵� üũ
				if (poolChild.id == id)
				{
					poolChild.InitChild();
					return poolChild;
				}
			}
		}

		// �ű� ����
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
