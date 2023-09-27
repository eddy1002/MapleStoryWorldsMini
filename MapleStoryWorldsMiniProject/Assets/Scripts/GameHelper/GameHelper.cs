using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    #region Public
    /// <summary>
    /// ��ũ�� ��ǥ�� UI ��ǥ�� ��ȯ
    /// </summary>
    /// <param name="mainCam">���� ī�޶�</param>
    /// <param name="canvas">ĵ����</param>
    /// <param name="scene">�÷��̾�</param>
    /// <param name="screenPoint">��ũ�� ��ǥ</param>
    /// <returns></returns>
    public static Vector2 ScreenToUIPoint(Camera mainCam, RectTransform canvas, RectTransform scene, Vector2 screenPoint)
    {
        if (mainCam != null && canvas != null)
        {
            Vector3 point = mainCam.ScreenToViewportPoint(screenPoint);

            var posX = (point.x - 0.5f) * canvas.sizeDelta.x;
            var posY = (point.y - 0.5f) * canvas.sizeDelta.y;

            if (scene != null)
            {
                return new Vector2(posX - scene.localPosition.x, posY - scene.localPosition.y);
            }
        }
        return Vector2.zero;
    }

    /// <summary>
    /// float ���� tileSize�� �����Ͽ� Ÿ�ϰ����� ��ȯ
    /// </summary>
    /// <param name="value">��</param>
    /// <param name="tileSize">Ÿ�� ũ��</param>
    /// <returns></returns>
    public static float GetTileValue(float value, float tileSize)
    {
        if (tileSize <= 0f)
        {
            return value;
        }
        return Mathf.RoundToInt(value / tileSize) * tileSize;
    }

    /// <summary>
    /// UI ��ǥ�� Ÿ�� ��ǥ�� ��ȯ
    /// </summary>
    /// <param name="uiPoint">UI ��ǥ</param>
    /// <param name="tileSize">Ÿ�� ��ǥ</param>
    /// <returns></returns>
    public static Vector2 UIPoint2TilePoint(Vector2 uiPoint, float tileSize)
    {
        return new Vector2(GetTileValue(uiPoint.x, tileSize), GetTileValue(uiPoint.y, tileSize));
    }

    /// <summary>
    /// UI ��ǥ�� Scene �ȿ� �ִ��� ��ȯ
    /// </summary>
    /// <param name="uiPoint">UI ��ǥ</param>
    /// <param name="canvas">ĵ����</param>
    /// <param name="scene">��</param>
    /// <returns>�ȿ� ������ true</returns>
    public static bool IsInScene(Vector2 uiPoint, RectTransform canvas, RectTransform scene)
    {
        if (canvas != null && scene != null)
        {
            var halfSizeDelta = (canvas.sizeDelta + scene.sizeDelta) * 0.5f;
            var inX = uiPoint.x >= -halfSizeDelta.x && uiPoint.x <= halfSizeDelta.x;
            var inY = uiPoint.y >= -halfSizeDelta.y && uiPoint.y <= halfSizeDelta.y;
            return inX && inY;
        }
        return false;
    }
    #endregion
}
