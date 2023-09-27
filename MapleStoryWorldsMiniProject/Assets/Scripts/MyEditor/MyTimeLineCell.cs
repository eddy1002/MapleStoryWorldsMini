using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimeLineCell
{
    #region PublicMember
    public bool isMake = false;
    public Vector2 tilePoint = Vector2.zero;
    public MyTimeLineCell prevCell = null;
    public MyTimeLineCell nextCell = null;
    #endregion

    #region Public
    public MyTimeLineCell(bool isMake, Vector2 tilePoint)
    {
        this.isMake = isMake;
        this.tilePoint = tilePoint;
    }
    #endregion
}
