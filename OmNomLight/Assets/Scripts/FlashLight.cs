﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashLight : LightSource {
    public Vector2 direction;
    [Range(0, 360)]
    public float totalAngle;

    protected override List<Vector2> addCustomCorners()
    {
        if (totalAngle == 0 || totalAngle == 360)
            return null;

        List<Vector2> customCorners = new List<Vector2>(2);
        float angleToRotate = totalAngle / 2;
        Vector2 start = transform.position;

        Vector2 leftCorner = Quaternion.AngleAxis(angleToRotate, Vector3.back) * direction;
        leftCorner = start + 100 * leftCorner;

        Vector2 rightCorner = Quaternion.AngleAxis(-angleToRotate, Vector3.back) * direction;
        rightCorner = start + 100 * rightCorner;

        customCorners.Add(leftCorner);
        customCorners.Add(rightCorner);

        return customCorners;
    }

    protected override void filterLines(List<Line> lines)
    {
        float validAngle = totalAngle / 2;
        Vector2 leftCorner = Quaternion.AngleAxis(validAngle, Vector3.back) * direction;
        Line leftLine = new Line();
        leftLine.start = transform.position;
        leftLine.end = ((Vector2)transform.position) + leftCorner;
        Vector2 rightCorner = Quaternion.AngleAxis(-validAngle, Vector3.back) * direction;
        Line rightLine = new Line();
        rightLine.start = transform.position;
        rightLine.end = ((Vector2)transform.position) + rightCorner;

        for (int i = lines.Count - 1; i >= 0; i--)
        {
            Line l = lines[i];
            if (!(l > leftLine && l < rightLine))
            {
               lines.Remove(l);
            }
        }
    }
}
