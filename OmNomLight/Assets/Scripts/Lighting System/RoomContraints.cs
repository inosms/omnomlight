using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
public class RoomContraints : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform t = transform;

        Vector2[] corners = GetComponent<PolygonCollider2D>().points;
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = t.TransformPoint(corners[i]);
        }

        LightSource.cornersOfRoom = corners;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
