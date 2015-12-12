
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlphaMap : MonoBehaviour
{
    public Material mat;

    //Basically only applies image effect
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        /*
        int width = src.width;
        int height = src.height;
        Texture2D alphaMask = new Texture2D(width, height);

        //calculate alpha mask
        Color[] pixels = new Color[width * height];
        bool[] black_or_white = new bool[width * height];


        //get list of all light triangles
        List<Triangle> triangles = new List<Triangle>();

        foreach (LightSource l in LightSource.lightSources)
        {
            triangles.AddRange(l.triangles);
        }

        //transform triangles into screen space
        for(int i = 0; i < triangles.Count; i++)
        {
            Triangle screenSpaceTriangle;
            screenSpaceTriangle.point0 = Camera.main.WorldToScreenPoint(triangles[i].point0);
            screenSpaceTriangle.point1 = Camera.main.WorldToScreenPoint(triangles[i].point1);
            screenSpaceTriangle.point2 = Camera.main.WorldToScreenPoint(triangles[i].point2);

            triangles[i] = screenSpaceTriangle;
        }
        bool bla = false;
        //check for each pixel whether it is inside of a triangle (yes -> white, no -> black)
        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle t = triangles[i];
            float[] pointsX = { t.point0.x, t.point1.x, t.point2.x };
            float[] pointsY = { t.point0.y, t.point1.y, t.point2.y };
            int minX = Mathf.RoundToInt(Mathf.Min(pointsX));
            int maxX = Mathf.RoundToInt(Mathf.Max(pointsX));
            int minY = Mathf.RoundToInt(Mathf.Min(pointsY));
            int maxY = Mathf.RoundToInt(Mathf.Max(pointsY));

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    black_or_white[y * width + x] = t.isPointInside(new Vector2(x, y)); 
                }
            }
        }

        for (int i = 0; i < black_or_white.Length; i++)
        {
            alphaMask.SetPixel(i % width, i / width, black_or_white[i] ? Color.white : Color.black);
        }

        alphaMask.Apply();

        mat.SetTexture("_AlphaMask", alphaMask);*/
        Graphics.Blit(src, dest, mat);
    }
}