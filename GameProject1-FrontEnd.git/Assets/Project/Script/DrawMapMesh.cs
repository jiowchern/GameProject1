using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Regulus.CustomType;

public class DrawMapMesh : MonoBehaviour
{

	private List<Polygon> _Polygons;

	public DrawMapMesh()
	{
		_Polygons = new List<Polygon>();
	}
	void Start () {
	
	}
		
	void Update () {
	
	}

	public void Set(List<Polygon> polygons)
	{
		_Polygons.Clear();
		foreach (var polygon in polygons)
		{
			_Polygons.Add(polygon.Clone());
		}
	}

	void OnDrawGizmos()
	{
		var position = transform.position;
		foreach (var polygon in _Polygons)
		{
			var edges = polygon.Points;

			for (int i = 0; i < edges.Length - 1; i++)
			{
				var p1 = new Vector3(edges[i].X , 0 , edges[i].Y);
				var p2 = new Vector3(edges[i+1].X, 0, edges[i+1].Y);
				Gizmos.DrawLine(position + p1, position + p2);                
			}

			{
				var p1 = new Vector3(edges[edges.Length - 1].X, 0, edges[edges.Length - 1].Y);
				var p2 = new Vector3(edges[0].X, 0, edges[0].Y);
				Gizmos.DrawLine(position + p1, position + p2);
			}
			

		}
			
		
	}

	
}
