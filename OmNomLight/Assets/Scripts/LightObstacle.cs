using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class LightObstacle : MonoBehaviour
{
    private int lightedCorners = 0;

    //a list containing all objects that block light
    public static List<PolygonCollider2D> obstacles = new List<PolygonCollider2D>();

    // Use this for initialization
    void Awake()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        obstacles.Add(collider);
    }

    void Update()
    {

    }

    public void incrementLightedCorners()
    {

    }
}
