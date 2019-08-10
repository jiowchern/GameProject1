using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

using UnityEngine.UI;

public class MapEntity : MonoBehaviour {
    private RectTransform _Rect;

    public Color Wall;

    public Color Actor;

    private RawImage _Image;


    private Vector2 _Target;
    private Vector2 _Current;

    void Awake()
    {
        _Rect = GetComponent<RectTransform>();
        _Image = GetComponent<UnityEngine.UI.RawImage>();
    }
    // Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (_Target != _Current)
        {
            //var dist = Vector2.Distance(_Target, _Current);
            var delta = UnityEngine.Time.deltaTime;
            var vector = (_Target - _Current);
            _Current += vector * delta ;
            /*if ((_Target - _Current).normalized != vector.normalized)
            {
                _Current = _Target;
            }*/
            var center = _Current;
            _Rect.anchoredPosition = center;
        }
	    
    }

    public void SetPosition(Vector2 center)
    {
        _Target = center;
        _Current = center;
        _Rect.anchoredPosition = center;        
    }

    public void UpdatePosition(Vector2 center)
    {
        _Target = center;
    }



    public void SetSize(Vector2 size)
    {
        _Rect.sizeDelta = size;
    }

    public void SetColor(ENTITY type)
    {
        if (EntityData.IsWall(type))
        {
            _Image.color = Wall;
        }
        else if (EntityData.IsActor(type))
        {
            _Image.color = Actor;
        }

    }

    public void UpdateDirection(float direction)
    {
        _Rect.rotation = Quaternion.Euler(0,0,direction +90 );
    }
}
