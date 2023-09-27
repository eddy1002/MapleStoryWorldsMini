using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    #region Public
    /// <summary>
    /// 스크린 좌표를 UI 좌표로 반환
    /// </summary>
    /// <param name="mainCam">메인 카메라</param>
    /// <param name="canvas">캔버스</param>
    /// <param name="scene">플레이씬</param>
    /// <param name="screenPoint">스크린 좌표</param>
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
    /// float 값을 tileSize를 적용하여 타일값으로 반환
    /// </summary>
    /// <param name="value">값</param>
    /// <param name="tileSize">타일 크기</param>
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
    /// UI 좌표를 타일 좌표로 반환
    /// </summary>
    /// <param name="uiPoint">UI 좌표</param>
    /// <param name="tileSize">타일 좌표</param>
    /// <returns></returns>
    public static Vector2 UIPoint2TilePoint(Vector2 uiPoint, float tileSize)
    {
        return new Vector2(GetTileValue(uiPoint.x, tileSize), GetTileValue(uiPoint.y, tileSize));
    }

    /// <summary>
    /// UI 좌표가 Scene 안에 있는지 반환
    /// </summary>
    /// <param name="uiPoint">UI 좌표</param>
    /// <param name="canvas">캔버스</param>
    /// <param name="scene">씬</param>
    /// <returns>안에 있으면 true</returns>
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
