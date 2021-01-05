using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : Singleton<GameArea>
{
    [SerializeField] Point TopLeftCornerPoint;
    [SerializeField] Point BottomRightCornerPoint;

    public Vector2 GetRandomPointInWorld()
    {
        Vector2 point;
        point.x = Random.Range(TopLeftCornerPoint.Position.x, BottomRightCornerPoint.Position.x);
        Debug.Log("X from " + TopLeftCornerPoint.Position.x + " to " + BottomRightCornerPoint.Position.x);
        point.y = Random.Range(BottomRightCornerPoint.Position.y, TopLeftCornerPoint.Position.y);
        Debug.Log("Y from " + BottomRightCornerPoint.Position.y + " to " + TopLeftCornerPoint.Position.y);
        return point;
    }
}
