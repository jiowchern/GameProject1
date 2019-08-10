using UnityEngine;
using System.Collections;

public class PointDrawer : MonoBehaviour {
    public Vector2[] Points;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if(Points == null)
	        return;


        var points = Points;
        PointDrawer.Draw(points);
	}

    public static void Draw(Vector2[] points)
    {
        for(var i = 0; i < points.Length - 1; i++)
        {
            PointDrawer._Draw(points, i);
        }

        /*var p1 = new UnityEngine.Vector3(points[points.Length - 1].x, 0, points[points.Length - 1].y);
        var p2 = new UnityEngine.Vector3(points[0].x, 0, points[0].y);
        Debug.DrawLine(p1, p2);*/
    }

    private static void _Draw(Vector2[] points, int i)
    {
        var p1 = new UnityEngine.Vector3(points[i].x, 0, points[i].y);
        var p2 = new UnityEngine.Vector3(points[i + 1].x, 0, points[i + 1].y);
        Debug.DrawLine(p1, p2);
    }
}
