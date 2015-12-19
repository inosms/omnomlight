using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CircularDepthBuffer {
    private struct BufferEntry
    {
        public float startAngle;
        public float endAngle;
        public Vector2[] corners;
        public float depthSquared;

        public BufferEntry(float start, float end, Vector2[] corners, float depthSquared)
        {
            startAngle = start;
            endAngle = end;
            this.corners = corners;
            this.depthSquared = depthSquared;
        }
        
        public bool IsBetween(float angle)
        {
            if(angle < 0)
            {
                angle = -angle % 360;
            }
            else
            {
                angle = angle % 360;
            }
            

            if(startAngle > endAngle)
            {
                return !(angle > endAngle && angle < startAngle);
            }
            else
            {
                return angle > startAngle && angle < endAngle;
            }
        }
    }

    public Vector2 center { get; set; }

    private List<BufferEntry> buffer = new List<BufferEntry>();

    public CircularDepthBuffer(Vector2 center)
    {
        this.center = center;
    }

    public void Clear()
    {
        buffer.Clear();
    }

    public void Fill(List<PolygonCollider2D> objects)
    {
        //sort objects by distance from center
        objects = objects.OrderBy(p => SqrDistanceToCenter(p.transform.position)).ToList();

        foreach (PolygonCollider2D p in objects)
        {
            //create bufferentry
            BufferEntry be = createBufferEntry(p);

            //an object is fully occulded when all of its corners are occluded
            if (!isFullyOccluded(be.corners))
            {
                buffer.Add(be);
            }
            else
            {
                //Debug.Log(p.name + " is fully occluded!");
            }

            //check if object occludes anything fully and remove those objects (should happen fairly rarely)
        }
    }

    public List<Vector2> GetVisibleCorners()
    {
        List<Vector2> result = new List<Vector2>();
        buffer.ForEach(be => result.AddRange(be.corners));
        return result;
    }

    //only check if one whole object is occluded
    private bool isFullyOccluded(Vector2[] corners)
    {
        if(buffer.Count == 0)
        {
            return false;
        }
        
        foreach(Vector2 c in corners)
        {
            bool neverBetween = true;
            foreach(BufferEntry e in buffer)
            {
                //a corner is btween an entry and in front of it
                float cornerAngle = getAngleFromCenter(c);
                if (e.IsBetween(cornerAngle))
                {
                    neverBetween = false;
                    if(SqrDistanceToCenter(c) < e.depthSquared)
                    {
                        return false;
                    }
                }
            }

            if (neverBetween)
            {
                return false;
            }
        }

        return true;
    }

    private float getAngleFromCenter(Vector2 point)
    {
        if(point == center)
        {
            return 0;
        }
        else
        {
            Vector2 up = Vector2.up;
            Vector2 dir = point - center;

            float dot = Vector2.Dot(up, dir);
            float det = up.x * dir.y - up.y * dir.x;
            float result =  -Mathf.Atan2(det, dot) * Mathf.Rad2Deg;

            if(result < 0)
            {
                result = 360 + result;
            }
            return result;
        }
    }

    private float SqrDistanceToCenter(Vector2 point)
    {
        return (point - center).SqrMagnitude();
    }

    private bool IsBetween(float startAngle, float angle, float endAngle)
    {
        if (angle < 0)
        {
            angle = (-angle) % 360;
        }
        else
        {
            angle = angle % 360;
        }


        if (startAngle > endAngle)
        {
            return !(angle > endAngle && angle < startAngle);
        }
        else
        {
            return angle > startAngle && angle < endAngle;
        }
    }

    private BufferEntry createBufferEntry(PolygonCollider2D collider)
    {
        Vector2[] path = collider.GetPath(0);
        float startAngle = 0;
        float endAngle = 0;
        Transform t = collider.transform;

        //TODO check if an object goes around the whole light source
        for (int i = 0; i < path.Length; i++)
        {
            Vector2 pathStart = t.TransformPoint(path[i]);
            Vector2 pathEnd = t.TransformPoint(path[(i + 1) % path.Length]);

            float pathStartAngle = getAngleFromCenter(pathStart);
            float pathEndAngle = getAngleFromCenter(pathEnd);

            //TODO just work with endAngle

            //edges will always take the smaller angle because its a straight line through a circle
            float diff0 = pathEndAngle - pathStartAngle;
            float diff1 = pathStartAngle - pathEndAngle;
            diff0 = diff0 < 0 ? 360 + diff0 : diff0;
            diff1 = diff1 < 0 ? 360 + diff1 : diff1;

            bool clockwise = diff0 < diff1;

            if (clockwise)
            {
                //initialize start and endangle
                if (i == 0)
                {
                    startAngle = pathStartAngle;
                    endAngle = pathEndAngle;
                }
                else
                {
                    endAngle = IsBetween(startAngle, pathEndAngle, endAngle) ? endAngle : pathEndAngle;
                }
                
            }
            else
            {
                //initialize start and endangle
                if (i == 0)
                {
                    endAngle = pathStartAngle;
                    startAngle = pathEndAngle;
                }
                else
                {
                    startAngle = IsBetween(startAngle, pathEndAngle, endAngle) ? startAngle : pathEndAngle;
                }
                
            }

            
        }
        //TODO calculate depth of object
        float depthSquared = ((Vector2) collider.transform.position - center).SqrMagnitude();

        //transform corners to worldspace
        Vector2[] corners = collider.points;
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = t.TransformPoint(corners[i]);
        }

        return new BufferEntry(startAngle, endAngle, corners, depthSquared);
    }
}
