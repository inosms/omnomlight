using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Line
{
    public Vector2 start;
    public Vector2 end;

    public static bool operator <(Line left, Line right)
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

    public static bool operator >(Line left, Line right)
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

    public bool isPointInside(Vector2 point)
    {
        bool b1, b2, b3;

        b1 = sign(point, point0, point1) < 0.0f;
        b2 = sign(point, point1, point2) < 0.0f;
        b3 = sign(point, point2, point0) < 0.0f;

        return (b1 == b2) && (b2 == b3);
    }

    private float sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
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

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class LightSource : MonoBehaviour
{
    public static List<LightSource> lightSources = new List<LightSource>();

    public static bool isLit(Vector2 point)
    {
        bool result = false;
        foreach (LightSource l in lightSources)
        {
            if (l.lightsPoint(point))
            {
                result = true;
            }
        }

        return result;
    }

    public bool isOn = true;
    public Light unitylight;

	// for debugging:
	public bool controlWithMouse = false;
    public bool DrawTriangles = true;
    public bool DrawLines = false;

    //stores all triangles
    private List<Triangle> triangles = new List<Triangle>();

    public float minimumVertexDistance = 0.1f;
    public Mesh litAreaMesh;
    public FuseBox fuseBox;

    void Start()
    {
        lightSources.Add(this);
        litAreaMesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0) && controlWithMouse)
        {
            Vector3 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldpoint.z = 0;

            transform.position = worldpoint;
        }
        //clear lists
        triangles.Clear();

        if (((fuseBox && fuseBox.isActivated) || !fuseBox) && isOn)
        {
            calculateLitArea();
            if (unitylight)
                unitylight.enabled = true;
        }
        else
        {
            litAreaMesh.Clear();

            if (unitylight)
                unitylight.enabled = false;
        }
    }

    protected virtual List<Vector2> addCustomCorners()
    {
        return null;
    }

    protected virtual List<Line> filterLines(List<Line> lines)
    {
        return lines;
    }

    private List<Vector2> getCorners()
    {
        List<Vector2> result = new List<Vector2>();
        List<PolygonCollider2D> obstacles = LightObstacle.obstacles;
        foreach (PolygonCollider2D collider in obstacles)
        {
            //get corners of obstacle
            Vector2[] points = collider.points;
            Transform t = collider.transform;

            //Transform all corners to world space
            for (int i = 0; i < points.Length; i++)
            {
                result.Add(t.TransformPoint(points[i]));
            }
        }
        return result;
    }

    void calculateLitArea()
    {
        //Point of lightsource
        Vector2 pos = transform.position;

        //get all lightobstacles
        List<Vector2> corners = getCorners();

        //get custom corners
        List<Vector2> moreCorners = addCustomCorners();
        if (moreCorners != null)
        {
            corners.AddRange(moreCorners);
        }

        //calculate intersection points
        List<Line> lines = new List<Line>();
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
            int layerMask = 1 << LayerMask.NameToLayer("Obstacle");
            Line line0 = new Line();
            if (hit0 = Physics2D.Raycast(pos, dir0, Mathf.Infinity, layerMask))
            {
                line0.start = pos;
                line0.end = hit0.point;
                lines.Add(line0);
            }


            if (hit1 = Physics2D.Raycast(pos, dir1, Mathf.Infinity, layerMask))
            {
                Line line1 = new Line();
                line1.start = pos;
                line1.end = hit1.point;

                lines.Add(line1);

                //count rays to corners
                LightObstacle lo = hit1.collider.GetComponent<LightObstacle>();
                if (lo)
                {
                    lo.incrementLightedCorners();
                }
            }

            Line line2 = new Line();
            if (hit2 = Physics2D.Raycast(pos, dir2, Mathf.Infinity, layerMask))
            {
                line2.start = pos;
                line2.end = hit2.point;
                lines.Add(line2);
            }
        }

        sortLinesByDirection(lines);

        List<Vector2> polygonVerts = new List<Vector2>();
        //draw lines into scene
        for (int i = 0; i < lines.Count; i++)
        {
            Line l = lines[i];

            polygonVerts.Add(l.end);
        }

        if (lines.Count == 0)
        {
            return;
        }

        polygonVerts = mergeCloseVertices(polygonVerts);

        lines = filterLines(lines);

        if (lines.Count == 0)
        {
            return;
        }

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

        generateMesh(triangles, lines);

        if (DrawTriangles)
        {
            foreach (Triangle t in triangles)
            {
                t.Draw();
            }
        }

        if (DrawLines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Debug.DrawLine(lines[i].start, lines[i].end, Color.red);
            }
        }
    }

    void sortLinesByDirection(List<Line> lines)
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
                if (Vector2.Distance(v1, v2) <= minimumVertexDistance)
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

    void generateMesh(List<Triangle> triangles, List<Line> lines)
    {
        litAreaMesh.Clear();

        List<Vector3> vertices = new List<Vector3>(triangles.Count * 2 + 1);
        List<int> trianglesList = new List<int>(triangles.Count * 3 + 1);
        List<Vector2> uvList = new List<Vector2>(triangles.Count * 2 + 1);
        Camera c = Camera.main;

        //add center
        vertices.Add(transform.InverseTransformPoint(transform.position));
        Vector2 uv0 = c.WorldToViewportPoint(transform.position);
        uv0 = new Vector2(1 - uv0.x, 1 - uv0.y);//(what???!)
        uvList.Add(uv0);

        for (int i = 0; i < triangles.Count - 1; i++)
        {
            Triangle t = triangles[i];

            trianglesList.Add(0);

            trianglesList.Add(vertices.Count);
            vertices.Add(transform.InverseTransformPoint(t.point2));
            Vector2 uv1 = c.WorldToViewportPoint(t.point2);
            uv1 = new Vector2(1 - uv1.x, 1 - uv1.y);
            uvList.Add(uv1);

            trianglesList.Add(vertices.Count);
            vertices.Add(transform.InverseTransformPoint(t.point1));
            Vector2 uv2 = c.WorldToViewportPoint(t.point1);
            uv2 = new Vector2(1 - uv2.x, 1 - uv2.y);
            uvList.Add(uv2);
        }
        //add last triangle
        trianglesList.AddRange(new int[] { 2, vertices.Count - 2, 0 });

        litAreaMesh.SetVertices(vertices);
        litAreaMesh.SetTriangles(trianglesList, 0);
        litAreaMesh.SetUVs(0, uvList);

        litAreaMesh.RecalculateNormals();
    }

    private float longestLineLength(List<Line> lines)
    {
        float result = 0;

        foreach (Line l in lines)
        {
            float dist = Vector2.Distance(l.start, l.end);
            if (dist > result)
                result = dist;
        }

        return result;
    }

    //see http://stackoverflow.com/questions/2049582/how-to-determine-a-point-in-a-2d-triangle
    private bool lightsPoint(Vector2 point)
    {
        if (!isOn || triangles.Count == 0)
            return false;

        foreach (Triangle t in triangles)
        {
            if (t.isPointInside(point))
            {
                return true;
            }
        }

        return false;
    }

    public void SetIsOn(bool isOn)
    {

        this.isOn = isOn;
    }

}
