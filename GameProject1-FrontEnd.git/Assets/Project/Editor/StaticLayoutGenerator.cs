using System;
using System.Linq;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Regulus.CustomType;

using UnityEditor;

using Regulus.Extension;
using Regulus.Project.GameProject1.Data;

using RegulusVector2 = Regulus.CustomType.Vector2;

public class StaticLayoutGenerator : EditorWindow
{
    private UnityEngine.Object _DrawTarget;

    private string _Name;

    [MenuItem("Regulus/GameProject1/StaticLayoutGenerator")]
    public static void Open()
    {
        var wnd = EditorWindow.GetWindow(typeof(StaticLayoutGenerator));
        wnd.Show();
    }

    public StaticLayoutGenerator()
    {
        
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        _DrawTarget = EditorGUILayout.ObjectField( _DrawTarget , typeof(DrawMapMesh), true);
        if (GUILayout.Button("Build"))
        {
            
            var polygons = _Build();
            if(_DrawTarget != null)
                _Draw(_DrawTarget as DrawMapMesh, polygons);

            
        }

        EditorGUILayout.EndVertical();
    }

    

    private List<Polygon> _Build()
    {
        var polygons = new List<Polygon>();
        var meshs = GameObject.FindObjectsOfType<MeshCollider>();
        foreach (var meshCollider in meshs)
        {
            var mesh = meshCollider.sharedMesh;
            
            
            var positions = new List<Vector3>();
            foreach (var vector in mesh.vertices)
            {
                var position = _Conversion(meshCollider.transform, vector);
                positions.Add(position);
            }

            
            _BuildComponment(meshCollider.gameObject, positions);

            var polygon = new Polygon(_To2D(positions));
            polygons.Add(polygon);
        }


        var boxColliders = GameObject.FindObjectsOfType<BoxCollider>();

        foreach (var boxCollider in boxColliders)
        {
            
            polygons.Add(new Polygon(_To2D(_BuildPolygon(boxCollider))));
        }


        return polygons;
    }

    private static void _BuildComponment(GameObject obj, IEnumerable<Vector3> positions)
    {
        var mark = obj.GetComponent<EntityLayoutMark>();
        if (mark != null)
        {
            if (mark.Name != ENTITY.STATIC)
                return;
        }
        else
        {
            mark = obj.AddComponent<EntityLayoutMark>();
            mark.Name = ENTITY.STATIC;
        }
        
        var staticLayout = obj.QueryComponent<StaticLayoutMark>();
        staticLayout.Static = mark;
        staticLayout.Polygon = positions.ToArray();
    }

    /// <summary>
    /// Retrieves a BoxCollider's world-space vertices
    /// </summary>
    /// <param name="col">BoxCollider to retrieve vertices from</param>
    /// <returns>Collider vertices</returns>
    Vector3[] _BuildPolygon(BoxCollider col)
    {
        Quaternion rot = col.transform.rotation;
        col.transform.rotation = Quaternion.identity;

        Vector3[] vertices = new Vector3[8];
        Vector3 e = col.bounds.extents;

        vertices[0] = RotateAroundPivot(new Vector3(-e.x, e.y, -e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[1] = RotateAroundPivot(new Vector3(e.x, e.y, -e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[2] = RotateAroundPivot(new Vector3(-e.x, -e.y, -e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[3] = RotateAroundPivot(new Vector3(e.x, -e.y, -e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[4] = RotateAroundPivot(new Vector3(-e.x, e.y, e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[5] = RotateAroundPivot(new Vector3(e.x, e.y, e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[6] = RotateAroundPivot(new Vector3(-e.x, -e.y, e.z), col.center, rot.eulerAngles) + col.transform.position;
        vertices[7] = RotateAroundPivot(new Vector3(e.x, -e.y, e.z), col.center, rot.eulerAngles) + col.transform.position;

        col.transform.rotation = rot;


        _BuildComponment(col.gameObject, vertices);

        

        return vertices;
    }


    /// <summary>
    /// Rotates a given Vector3 point around a given pivot by a set of given euler angles
    /// </summary>
    /// <param name="point">Point to rotate</param>
    /// <param name="pivot">Pivot to rotate around</param>
    /// <param name="angles">Rotation euler angles</param>
    /// <returns>Rotated point</returns>
    Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    private void _Draw(DrawMapMesh draw_map_mesh, List<Polygon> polygons)
    {
        draw_map_mesh.Set(polygons);
    }

    private RegulusVector2[] _To2D(IEnumerable<Vector3> positions)
    {        
        
        return FindHull2(from position in  positions select new RegulusVector2(position.x , position.z)).FindHull().ToArray();
    }

    private Vector3 _Conversion(Transform center, Vector3 vector)
    {
        
        
        var pos = vector;
        pos.Scale(center.lossyScale);

        pos = center.rotation * pos;
        pos += center.position;
        
        return pos;
    }

    public static IEnumerable<RegulusVector2> FindHull2(IEnumerable<RegulusVector2> points)
    {
        List<PointToProcess> pointsToProcess = new List<PointToProcess>();

        // convert input points to points we can process
        foreach (RegulusVector2 point in points)
        {
            pointsToProcess.Add(new PointToProcess(point));
        }

        // find a point, with lowest X and lowest Y
        int firstCornerIndex = 0;
        PointToProcess firstCorner = pointsToProcess[0];

        for (int i = 1, n = pointsToProcess.Count; i < n; i++)
        {
            if ((pointsToProcess[i].x < firstCorner.x) ||
                 ((pointsToProcess[i].x == firstCorner.x) && (pointsToProcess[i].y < firstCorner.y)))
            {
                firstCorner = pointsToProcess[i];
                firstCornerIndex = i;
            }
        }

        // remove the just found point
        pointsToProcess.RemoveAt(firstCornerIndex);

        // find K (tangent of line's angle) and distance to the first corner
        for (int i = 0, n = pointsToProcess.Count; i < n; i++)
        {
            float dx = pointsToProcess[i].x - firstCorner.x;
            float dy = pointsToProcess[i].y - firstCorner.y;

            // don't need square root, since it is not important in our case
            pointsToProcess[i].Distance = dx * dx + dy * dy;
            // tangent of lines angle
            pointsToProcess[i].K = (dx == 0) ? float.PositiveInfinity : (float)dy / dx;
        }

        // sort points by angle and distance
        pointsToProcess.Sort();

        List<PointToProcess> convexHullTemp = new List<PointToProcess>();

        // add first corner, which is always on the hull
        convexHullTemp.Add(firstCorner);
        // add another point, which forms a line with lowest slope
        convexHullTemp.Add(pointsToProcess[0]);
        pointsToProcess.RemoveAt(0);

        PointToProcess lastPoint = convexHullTemp[1];
        PointToProcess prevPoint = convexHullTemp[0];

        while (pointsToProcess.Count != 0)
        {
            PointToProcess newPoint = pointsToProcess[0];

            // skip any point, which has the same slope as the last one or
            // has 0 distance to the first point
            if ((newPoint.K == lastPoint.K) || (newPoint.Distance == 0))
            {
                pointsToProcess.RemoveAt(0);
                continue;
            }

            // check if current point is on the left side from two last points
            if ((newPoint.x - prevPoint.x) * (lastPoint.y - newPoint.y) - (lastPoint.x - newPoint.x) * (newPoint.y - prevPoint.y) < 0)
            {
                // add the point to the hull
                convexHullTemp.Add(newPoint);
                // and remove it from the list of points to process
                pointsToProcess.RemoveAt(0);

                prevPoint = lastPoint;
                lastPoint = newPoint;
            }
            else if(convexHullTemp.Count > 2)
            {

                // remove the last point from the hull
                
                convexHullTemp.RemoveAt(convexHullTemp.Count - 1);

                lastPoint = prevPoint;                
                prevPoint = convexHullTemp[convexHullTemp.Count - 2];
            }
            else
            {
                break;
            }
        }

        // convert points back
        List<RegulusVector2> convexHull = new List<RegulusVector2>();

        foreach (PointToProcess pt in convexHullTemp)
        {
            convexHull.Add(pt.ToPoint());
        }

        return convexHull;
    }

    // Internal comparer for sorting points
    private class PointToProcess : IComparable
    {
        public float x;
        public float y;
        public float K;
        public float Distance;

        public PointToProcess(RegulusVector2 point)
        {
            x = point.X;
            y = point.Y;

            K = 0;
            Distance = 0;
        }

        public int CompareTo(object obj)
        {
            PointToProcess another = (PointToProcess)obj;

            return (K < another.K) ? -1 : (K > another.K) ? 1 :
                ((Distance > another.Distance) ? -1 : (Distance < another.Distance) ? 1 : 0);
        }

        public RegulusVector2 ToPoint()
        {
            return new RegulusVector2(x, y);
        }
    }
}
