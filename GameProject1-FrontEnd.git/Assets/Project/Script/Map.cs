using UnityEngine;
using System.Collections;
using Regulus.Extension;

using Regulus.Project.GameProject1.Data;

public class Map : MonoBehaviour {
	private RectTransform _Rect;
	public  RectTransform Viewport;
	

	public float UnitScale = 1f;

	public GameObject EntitySource;


    private Vector2 _Target;
    private Vector2 _Current;

    // Use this for initialization
    void Start ()
	{
		_Rect = GetComponent<RectTransform>();

	   // _TestInitial();
	}

	private void _TestInitial()
	{
		var rand = Regulus.Utility.Random.Instance;
		
		_Add(new Vector2(rand.NextFloat(-10, 10), rand.NextFloat(-10, 10)),0, rand.NextEnum<Regulus.Project.GameProject1.Data.ENTITY>());
		_Add(new Vector2(rand.NextFloat(-10, 10), rand.NextFloat(-10, 10)),0, rand.NextEnum<Regulus.Project.GameProject1.Data.ENTITY>());
		_Add(new Vector2(rand.NextFloat(-10, 10), rand.NextFloat(-10, 10)),0, rand.NextEnum<Regulus.Project.GameProject1.Data.ENTITY>());
		_Add(new Vector2(rand.NextFloat(-10, 10), rand.NextFloat(-10, 10)),0, rand.NextEnum<Regulus.Project.GameProject1.Data.ENTITY>());
		_Add(new Vector2(rand.NextFloat(-10, 10), rand.NextFloat(-10, 10)),0, rand.NextEnum<Regulus.Project.GameProject1.Data.ENTITY>());
	}

	private MapEntity _Add(Vector2 position ,float degree, Regulus.Project.GameProject1.Data.ENTITY type)
	{

		var entityObject = GameObject.Instantiate(EntitySource);
		var rect = entityObject.GetComponent<RectTransform>();
		rect.SetParent(_Rect);
		rect.localScale = new Vector3(1f , 1f , 1f);

		var entity = entityObject.GetComponent<MapEntity>();
		entity.SetPosition(position);

		entity.SetColor(type);

		var source = Resource.Instance.FindEntity(type);
        
	    var mesh = source.Mesh.Clone();
        mesh.RotationByDegree(degree);
        var bound = mesh.Points.ToRect();        
		entity.SetSize(new Vector2(bound.Width * UnitScale, bound.Height * UnitScale));

		return entity;
	}

	// Update is called once per frame
	void Update ()
    {
	    if (_Target != _Current)
	    {	        
            var delta = UnityEngine.Time.deltaTime;
	        var vector = (_Target - _Current);	        
            _Current += vector * delta ;
            /*if ((_Target - _Current).normalized != unit)
	        {
	            _Current = _Target;
	        }*/
        }


        var position = _Current;
        var scaleX = _Rect.localScale.x;
        var scaleY = _Rect.localScale.y;
        _Rect.anchoredPosition = new Vector2(-position.x * scaleX + Viewport.rect.width / 2, -position.y * scaleY + Viewport.rect.height / 2);
    }


	public void SetScale(float scale)
	{
		//_Rect.sizeDelta = new Vector2(_BaseWidth * scale , _BaseHeight * scale);
		_Rect.localScale = new Vector2(scale, scale);
	}

	public MapEntity Add(IVisible visible)
	{
		return _Add(new Vector2(visible.Position.X, visible.Position.Y), visible.Direction, visible.EntityType);
	}

	public void SetPosision(Regulus.CustomType.Vector2 position)
	{
	    _Target = new Vector2(position.X , position.Y );

        
        /*var scaleX = _Rect.localScale.x;
        var scaleY = _Rect.localScale.y;
        _Rect.anchoredPosition = new Vector2(-position.X * scaleX + Viewport.rect.width / 2, -position.Y * scaleY + Viewport.rect.height / 2);*/
    }
}
 