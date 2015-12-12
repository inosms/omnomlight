using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Line
{
    public Vector2 start;
    public Vector2 end;

    public static bool operator < (Line left, Line right)
    {
        Vector2 dirLeft = (left.end - left.start).normalized;
        Vector2 dirRight = (right.end - right.start).normalized;
        Vector2 up = -Vector2.up;
 
        //see: http://stackoverflow.com/questions/14066933/direct-way-of-computing-clockwise-angle-between-2-vectors
        float leftDot = Vector2.Dot(up, dirLeft);
        float leftDet = up.x * dirLeft.y - up.y * dirLeft.x;
        float leftAngle = Mathf.Atan2(leftDet, leftDot);

        float rightDot = Vector2.Dot(up, dirRight);
        float rightDet = up.x * dirRight.y - up.y * dirRight.x;
        float rightAngle = Mathf.Atan2(rightDet, rightDot);

        return leftAngle < rightAngle;
    }

    public static bool operator > (Line left, Line right)
    {
        return right < left;
    }
}

public struct Triangle 
{
    public Vector2 point0, point1, point2;

    public void Draw()
    {
        Debug.DrawLine(point0, point1);
        Debug.DrawLine(point1, point2);
        Debug.DrawLine(point2, point0);
    }
}

public class LineSorter : IComparer<Line> 
{
    int IComparer<Line>.Compare(Line left, Line right)
    {
        if (left < right) return -1;
        else if (right < left) return 1;
        else return 0;
    }
}

public class LightSource : MonoBehaviour 
{
    public static List<LightSource> lightSources = new List<LightSource>();

    public bool isOn = true;
    public bool DrawTriangles = true;
    public bool DrawLines = false;
    public Transform testPoint;

    //stores all corner points of all light obestacles
    private List<Vector2> corners = new List<Vector2>();
    //stores all line to draw for debugging
    private List<Line> lines = new List<Line>();
    //stores all triangles
    private List<Triangle> triangles = new List<Triangle>();

    public float minimumVertexDistance = 0.1f;

    void Awake()
    {
        lightSources.Add(this);
    }

	// Update is called once per frame
	void Update () 
    {
        if (isOn)
        {
            //clear lists
            corners.Clear();
            lines.Clear();
            triangles.Clear();

            calculateLitArea();
        }
        
	}

    void calculateLitArea() 
    {
        //Point of lightsource
        Vector2 pos = transform.position;

        //get all lightobstacles
        List<PolygonCollider2D> obstacles = LightObstacle.obstacles;
        foreach (PolygonCollider2D collider in obstacles)
        {
            //get corners of obstacle
            Vector2[] points = collider.points;
            Transform t = collider.transform;

            //Transform all corners to world space
            for (int i = 0; i < points.Length; i++)
            {
                corners.Add(t.TransformPoint(points[i]));
            }
        }

        //calculate intersection points
        foreach (Vector2 c in corners)
        {
            //create ray to be cast, 0 = counter-clockwise, 1 = corner, 2 = clockwise
            RaycastHit2D hit0 = new RaycastHit2D();
            RaycastHit2D hit1 = new RaycastHit2D();
            RaycastHit2D hit2 = new RaycastHit2D();

            Vector3 dir1 = c - pos;
            Vector3 dir0 = Quaternion.AngleAxis(0.1f, Vector3.back) * dir1;
            Vector3 dir2 = Quaternion.AngleAxis(-0.1f, Vector3.back) * dir1;

            //Cast rays in scene
            Line line0 = new Line();
            if (hit0 = Physics2D.Raycast(pos, dir0))
            {
                line0.start = pos;
                line0.end = hit0.point;
                lines.Add(line0);
            }

            
            if (hit1 = Physics2D.Raycast(pos, dir1))
            {
                Line line1 = new Line();
                line1.start = pos;
                line1.end = hit1.point;

                lines.Add(line1);
            }

            Line line2 = new Line();
            if (hit2 = Physics2D.Raycast(pos, dir2))
            {
                line2.start = pos;
                line2.end = hit2.point;
                lines.Add(line2);
            }
        }

        sortLinesByDirection();

        List<Vector2> polygonVerts = new List<Vector2>();
        //draw lines into scene
        for (int i = 0; i < lines.Count; i++)
        {
            Line l = lines[i];
            
            polygonVerts.Add(l.end);
        }

        polygonVerts = mergeCloseVertices(polygonVerts);

        //Create triangles
        for (int i = 0; i < lines.Count - 1; i++)
        {
            Triangle t = new Triangle();
            t.point0 = pos;
            t.point1 = lines[i].end;
            t.point2 = lines[i + 1].end;

            triangles.Add(t);
        }
        //create last triangle
        Triangle lastT = new Triangle();
        lastT.point0 = pos;
        lastT.point1 = lines[0].end;
        lastT.point2 = lines[lines.Count - 1].end;
        triangles.Add(lastT);

        if (DrawTriangles)
        {
            foreach (Triangle t in triangles)
            {
                t.Draw();
            }
        }

        if (DrawLines)
        {
            for (int i = 0; i < polygonVerts.Count; i++)
            {
                Debug.DrawLine(transform.position, polygonVerts[i], Color.red);
            }
        }

        Debug.Log(isLit(testPoint.position));
    }

    void sortLinesByDirection()
    {
        lines.Sort(new LineSorter());
    }

    List<Vector2> mergeCloseVertices(List<Vector2> vertices)
    {
        List<Vector2> result = new List<Vector2>();
        result.Add(vertices[0]);

        foreach (Vector2 v1 in vertices)
        {
            bool isUnique = true;

            foreach (Vector2 v2 in result)
            {
                //check if another vertex that is basically the same is already in result
                if(Vector2.Distance(v1, v2) <= minimumVertexDistance)
                {
                    isUnique = false;
                }
            }

            //add if unique
            if (isUnique)
            {
                result.Add(v1);
            }
        }

        return result;
    }

    //see http://stackoverflow.com/questions/2049582/how-to-determine-a-point-in-a-2d-triangle
    public bool isLit(Vector2 point)
    {
        if (!isOn || triangles.Count == 0)
            return false;

        foreach (Triangle t in triangles)
        {
            bool b1, b2, b3;

            b1 = sign(point, t.point0, t.point1) < 0.0f;
            b2 = sign(point, t.point1, t.point2) < 0.0f;
            b3 = sign(point, t.point2, t.point0) < 0.0f;

            if ((b1 == b2) && (b2 == b3))
            {
                return true;
            }  
        }

        return false;
    }

    private float sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }


}
