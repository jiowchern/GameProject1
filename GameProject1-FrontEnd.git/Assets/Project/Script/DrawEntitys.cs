using UnityEngine;
using System.Collections;
using Regulus.Project.GameProject1.Data;
using Vector2 = Regulus.CustomType.Vector2;

public class DrawEntitys : MonoBehaviour
{

    public TextAsset Source;
    private EntityData[] _Datas;
    // Use this for initialization
	void Start ()
	{
	    var datas = Regulus.Utility.Serialization.Read<EntityData[]>(Source.bytes);
	    _Datas = datas;
	}

    // Update is called once per frame
	void Update () {

	    foreach (var data in _Datas)
	    {
	        var points = data.Mesh.Points;
	        for (var i = 0; i < points.Length - 1; i++)
	        {
	            _Draw(points, i);
	        }

            var p1 = new UnityEngine.Vector3(points[points.Length - 1].X, 0, points[points.Length - 1].Y);
            var p2 = new UnityEngine.Vector3(points[0].X, 0, points[0].Y);
            Debug.DrawLine(p1, p2);

        }
	}

    private static void _Draw(Vector2[] points, int i)
    {
        var p1 = new UnityEngine.Vector3(points[i].X, 0, points[i].Y);
        var p2 = new UnityEngine.Vector3(points[i + 1].X, 0, points[i + 1].Y);
        Debug.DrawLine(p1, p2);
    }
}
