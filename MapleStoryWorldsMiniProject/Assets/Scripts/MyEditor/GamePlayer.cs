using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayer : MonoBehaviour
{
    #region PublicMember
    public bool isPlay = false;
    public Button playButton;
	public Image playIcon;
	public System.Action<bool> onPlayChanged = null;
	public Color playColor = Color.blue;
	public Color editorColor = Color.black;
	#endregion

	#region Mono
	private void Awake()
	{
		if (playButton != null)
		{
			playButton.onClick.RemoveAllListeners();
			playButton.onClick.AddListener(ChangePlay);
		}

		isPlay = false;
		RefreshButton();
		onPlayChanged?.Invoke(isPlay);
	}
	#endregion

	#region Private
	private void RefreshButton()
	{
		if (playIcon != null)
		{
			playIcon.color = isPlay ? playColor : editorColor;
		}
	}
	#endregion

	#region Callback
	private void ChangePlay()
	{
		isPlay = !isPlay;
		RefreshButton();
		onPlayChanged?.Invoke(isPlay);
	}
	#endregion
}
