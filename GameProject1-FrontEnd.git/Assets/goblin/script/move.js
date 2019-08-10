
 private var move:float = 20;
 private var stop:boolean = false;	
 private var blend:float;
 private var delay:float = 0;
 var AddRunSpeed:float = 1;
 var AddWalkSpeed:float = 1;

function Move ()
{
	if ( Input.GetKey(KeyCode.UpArrow) )
		    {  	
		    	move *= 1.015F;
		      	var add:int;
		    	
		    	if ( move > 300 )
		    		{
		    			GetComponent.<Animation>().CrossFade("run");
		    			add = 20*AddRunSpeed;
		    		}
		    	else
		    		{
		    			GetComponent.<Animation>().Play("walk");
		    			add = 5*AddWalkSpeed;
		    		}
		    			    	
		        var speed:float = Time.deltaTime*add;
		        
		        transform.Translate( 0,0,speed );
		    }

		        
    if ( Input.GetKeyUp(KeyCode.UpArrow))
    	{
			if ( GetComponent.<Animation>().IsPlaying("walk"))
				{	GetComponent.<Animation>().CrossFade("stand",0.3); }
			if ( GetComponent.<Animation>().IsPlaying("run"))
				{	
					GetComponent.<Animation>().CrossFade("stand",0.5);
					stop = true;
				}	
			move = 20;
		}
				
	if (stop == true)
		{	
			var max:float = Time.deltaTime*40;
			blend = Mathf.Lerp(max,0,delay);
			
			if ( blend > 0 )
			{	
				transform.Translate( 0,0,blend );
				delay += 0.025f; 
			}	
			else 
			{	
				stop = false;
				delay = 0.0f;
			}
		}
}

function Update () 
	{
		//print ("again");
		Move();
		 
		if (Input.GetKey(KeyCode.Q))
			{	
				GetComponent.<Animation>().CrossFade("attack01",0.2);
				GetComponent.<Animation>().CrossFadeQueued("stand",0.3);
			}
			
		if (Input.GetKey(KeyCode.W))
			{	
				GetComponent.<Animation>().CrossFade("attack02",0.2);
				GetComponent.<Animation>().CrossFadeQueued("stand",0.3);
			}
			
		if (Input.GetKey(KeyCode.A))
			{	
				GetComponent.<Animation>().CrossFade("drop down",0.2);
				//animation.CrossFadeQueued("idle1",0.5);
			}
	
		if (Input.GetKey(KeyCode.Z))
			{	
				GetComponent.<Animation>().CrossFade("sit up",0.2);
				GetComponent.<Animation>().CrossFadeQueued("stand",0.5);
			}
		
		if (Input.GetKey(KeyCode.S))
			{	
				GetComponent.<Animation>().CrossFade("damage",0.1);
				GetComponent.<Animation>().CrossFadeQueued("stand",0.1);			
			}
			
		if (Input.GetKey(KeyCode.X))
			{	
				GetComponent.<Animation>().CrossFade("dead",0.1);
				//animation.CrossFadeQueued("stand",0.1);			
			}			
			
		if (Input.GetKey(KeyCode.D))
			{	
				GetComponent.<Animation>().CrossFade("stand_vigilance",0.1);
				GetComponent.<Animation>().CrossFadeQueued("stand",0.3);			
			}			
			
		if ( Input.GetKey(KeyCode.LeftArrow))
			{
				transform.Rotate( 0,Time.deltaTime*-100,0);
			}
		
		if (Input.GetKey(KeyCode.RightArrow))
			{
				transform.Rotate(0,Time.deltaTime*100,0);
			}
	}
	
