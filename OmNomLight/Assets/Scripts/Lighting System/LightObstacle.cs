using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class LightObstacle : MonoBehaviour
{
    public Gradient gradient;
    private float LerpSpeed = 10;

    private int lightedCorners = 0;
    private int corners;

    //a list containing all objects that block light
    public static List<PolygonCollider2D> obstacles = new List<PolygonCollider2D>();

    // Use this for initialization
    void Start()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        obstacles.Add(collider);

        corners = collider.points.Length;
    }

    void Update()
    {
        float sample = Mathf.Clamp01(((float)lightedCorners) / corners);
        
        Color color = gradient.Evaluate(sample);

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer)
        {
            renderer.color = Color.Lerp(renderer.color, color, Time.deltaTime * LerpSpeed);
        }

        lightedCorners = 0;
    }

    public void incrementLightedCorners()
    {
        lightedCorners++;
    }
}
