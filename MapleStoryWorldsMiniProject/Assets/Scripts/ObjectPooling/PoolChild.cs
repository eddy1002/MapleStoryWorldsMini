using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolChild : MonoBehaviour
{
	#region PublicMember
	public int id = 0;
	#endregion

	#region Public
	public virtual PoolChild InitChild()
	{
		// 활성화
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		return this;
	}

	public virtual void ReleaseChild()
	{
		// 비활성화
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
	}
	#endregion
}
