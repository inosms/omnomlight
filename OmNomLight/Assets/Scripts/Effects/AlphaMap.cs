
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlphaMap : MonoBehaviour
{
    /*public Material mat;

    bool recalculate = true;
    Texture2D alphaMask;
    //Basically only applies image effect
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        foreach(LightSource l in LightSource.lightSources)
        {
            if(l.hasChanged)
            {
                recalculate = true;
            }

            l.hasChanged = false;
        }

        if (recalculate)
        {
            recalculateTexture(src, dest);
            recalculate = false;
        }
        else
        {
            Graphics.Blit(src, dest, mat);
        }
    }

    private void recalculateTexture(RenderTexture src, RenderTexture dest)
    {
        int width = src.width;
        int height = src.height;
        alphaMask = new Texture2D(width, height);

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
        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle screenSpaceTriangle;
            screenSpaceTriangle.point0 = Camera.main.WorldToScreenPoint(triangles[i].point0);
            screenSpaceTriangle.point1 = Camera.main.WorldToScreenPoint(triangles[i].point1);
            screenSpaceTriangle.point2 = Camera.main.WorldToScreenPoint(triangles[i].point2);

            triangles[i] = screenSpaceTriangle;
        }

        //check for each pixel whether it is inside of a triangle (yes -> white, no -> black)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                foreach (Triangle t in triangles)
                {
                    if (t.isPointInside(new Vector2(x, y)))
                    {
                        black_or_white[y * width + x] = true;
                        break;
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
                alphaMask.SetPixel(x, height - y, black_or_white[y * width + x] ? Color.white : Color.black);
        }

        alphaMask.Apply();

        mat.SetTexture("_AlphaMask", alphaMask);
        Graphics.Blit(src, dest, mat);
    }*/
}