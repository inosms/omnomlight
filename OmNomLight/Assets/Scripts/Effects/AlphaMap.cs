
using UnityEngine;
using System.Collections;

public class AlphaMap : MonoBehaviour
{
    public Material mat;

    //Basically only applies image effect
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }
}