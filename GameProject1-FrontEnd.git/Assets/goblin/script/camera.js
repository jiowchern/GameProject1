
function Update () 
	{

			
		if ( Input.GetKey(KeyCode.Delete))
			{
				transform.Rotate( 0,Time.deltaTime*60,0);
			}
		
		if (Input.GetKey(KeyCode.PageDown))
			{
				transform.Rotate(0,Time.deltaTime*-60,0);
			}
	}
	
