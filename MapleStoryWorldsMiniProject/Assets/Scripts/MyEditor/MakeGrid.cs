using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGrid : MonoBehaviour
{
    public GamePlayer gamePlayer;
    public RectTransform canvas;
    public RectTransform scene;
    public Transform parent;
    public GameObject lineH;
    public GameObject lineV;
    public Vector2 gridSize;

    // Start is called before the first frame update
    void Start()
    {
        DrawLineH();
        DrawLineV();
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

    private Vector2 GetSceneSize()
    {
        if (canvas != null && scene != null)
		{
            return canvas.sizeDelta + scene.sizeDelta;
        }
        return Vector2.zero;
    }

    private void DrawLineH()
	{
        if (canvas != null && scene != null && parent != null && lineH != null)
        {
            var point = gridSize.y * 0.5f;
            while (point < GetSceneSize().y * 0.5f)
            {
                var up = Instantiate(lineH, parent);
                var down = Instantiate(lineH, parent);

                up.GetComponent<RectTransform>().localPosition = new Vector2(0f, point);
                down.GetComponent<RectTransform>().localPosition = new Vector2(0f, -point);

                up.SetActive(true);
                down.SetActive(true);

                point += gridSize.y;
            }
        }
	}

    private void DrawLineV()
    {
        if (canvas != null && scene != null && parent != null && lineV != null)
        {
            var point = gridSize.x * 0.5f;
            while (point < GetSceneSize().x * 0.5f)
            {
                var left = Instantiate(lineV, parent);
                var right = Instantiate(lineV, parent);

                left.GetComponent<RectTransform>().localPosition = new Vector2(-point, 0f);
                right.GetComponent<RectTransform>().localPosition = new Vector2(point, 0f);

                left.SetActive(true);
                right.SetActive(true);

                point += gridSize.x;
            }
        }
    }

    #region Callback
    public void SetPlayMode(bool isPlay)
    {
        if (parent != null && parent.gameObject.activeSelf != !isPlay)
        {
            parent.gameObject.SetActive(!isPlay);
        }
    }
    #endregion
}
