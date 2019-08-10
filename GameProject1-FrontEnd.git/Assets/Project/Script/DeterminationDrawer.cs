using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Linq;

using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;
using Regulus.Utility;

using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;

public class DeterminationDrawer : MonoBehaviour {
    public Vector2[] Left;

    public Vector2[] Right;

    public float Total;
    public float Begin;
    public float End;
    

    private float _LastPart;

    private Determination _Determination;

    public Color LineColor = Color.red;
    public Color AllLineColor = Color.blue;

    // Use this for initialization
    void Start ()
    {
        
	    _LastPart = 0;

        var lefts = (from vl in Left select new Regulus.CustomType.Vector2(vl.x, vl.y)).ToArray();
        var right = (from vr in Right select new Regulus.CustomType.Vector2(vr.x, vr.y)).ToArray();
        _Determination = new Determination(lefts , right , Total , Begin , End);
    }
	
	// Update is called once per frame
	void Update ()
	{
       
        
        var angle =- transform.rotation.eulerAngles.y * Mathf.PI / 180.0f;
	    var lefts = (from vl in Left
	                 let pnt = new Regulus.CustomType.Vector2(vl.x, vl.y) 	    
                     select Regulus.CustomType.Polygon.RotatePoint(pnt, new Regulus.CustomType.Vector2(), angle )).ToArray();
        var right = (from vr in Right 
                     let pnt = new Regulus.CustomType.Vector2(vr.x, vr.y)
                     select Regulus.CustomType.Polygon.RotatePoint(pnt, new Regulus.CustomType.Vector2(), angle)).ToArray();

        var center = new Regulus.CustomType.Vector2(transform.position.x, transform.position.z);


#if UNITY_EDITOR
        Regulus.Project.GameProject1.Game.Play.BattleCasterStatus._DrawAll(lefts , right , transform.position, AllLineColor);
#endif
        //_DrawAll();

        var delta  = UnityEngine.Time.deltaTime;
	    Regulus.CustomType.Polygon result = _Determination.Find(_LastPart , delta );
	    if (result != null)
	    {
            
            result.Rotation(angle , new Regulus.CustomType.Vector2());
            result.Offset(center);
#if UNITY_EDITOR
            Regulus.Project.GameProject1.Game.Play.BattleCasterStatus._Draw(result, transform.position.y, LineColor);
#endif
            //_Draw(result);3
        }
	    _LastPart += delta;

	    if (_LastPart > Total)
	    {
	        _LastPart -= Total;
	    }
	}

    private void _DrawAll()
    {
        for (int i = 0; i < Left.Length - 1; i++)
        {
            var pos1 = transform.position + new Vector3(Left[i].x, 0, Left[i].y);
            var pos2 = transform.position + new Vector3(Left[i + 1].x, 0, Left[i + 1].y);
            Debug.DrawLine(pos1, pos2, AllLineColor);
        }

        for (int i = 0; i < Right.Length - 1; i++)
        {
            var pos1 = transform.position + new Vector3(Right[i].x , 0 , Right[i].y)  ;
            var pos2 = transform.position + new Vector3(Right[i+1].x, 0, Right[i+1].y);
            Debug.DrawLine(pos1, pos2, AllLineColor);
        }

    }

    private void _Draw(Polygon result)
    {
        var points = (from p in result.Points select new UnityEngine.Vector3(p.X, 0, p.Y)).ToArray();
        var len = points.Length;
        if (len < 2)
        {
            return ;
        }
        for (int i = 0; i < len - 1; i++)
        {
            var p1 = points[i];
            var p2 = points[i + 1];
            Debug.DrawLine(transform.position + p1, transform.position + p2, LineColor);
        }

        Debug.DrawLine(transform.position + points[len - 1], transform.position + points[0], LineColor);        
    }

    
}