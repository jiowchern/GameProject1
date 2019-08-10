using UnityEngine;
using System.Collections;

//
// アニメーションチェックスクリプト
// 2014/09/13 quad arrow
//

// Require these components when using this script
[RequireComponent(typeof(Animator))]

public class IdleChanger : MonoBehaviour
{
	
	private Animator anim;						// Animatorへの参照
	private AnimatorStateInfo currentState;		// 現在のステート状態を保存する参照
	private AnimatorStateInfo previousState;	// ひとつ前のステート状態を保存する参照


	// Use this for initialization
	void Start ()
	{
		// 各参照の初期化
		anim = GetComponent<Animator> ();
		currentState = anim.GetCurrentAnimatorStateInfo (0);
		previousState = currentState;
//		animator.SetBool("NowStanding",true); //NowStandingパラメータにtrueをセット
	}


	void OnGUI()
	{
//Right pane
		GUI.Box(new Rect(Screen.width - 260 , 40 ,210 ,500), "");

		if(GUI.Button(new Rect(Screen.width - 250 , 50, 90, 20), "Walk_F"))
			anim.SetBool ("Walk_F", true);
		if(GUI.Button(new Rect(Screen.width - 250 , 75 , 90, 20), "Walk_B"))
			anim.SetBool ("Walk_B", true);
		if(GUI.Button(new Rect(Screen.width - 250 , 100 , 90, 20), "Walk_L"))
			anim.SetBool ("Walk_L", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 125 , 90, 20), "Walk_R"))
			anim.SetBool ("Walk_R", true);
		if(GUI.Button(new Rect(Screen.width - 250 , 150 , 90, 20), "StepPears"))
			anim.SetBool ("StepPears", true);
		if(GUI.Button(new Rect(Screen.width - 250 , 175 , 90, 20), "BSSp2UpC"))
			anim.SetBool ("BSSpear2UpperCut", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 200 , 90, 20), "VSlash_01"))
			anim.SetBool ("VSlash_01", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 225 , 90, 20), "V2HSlash"))
			anim.SetBool ("VSlash2HSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 250 , 90, 20), "DashSlash01"))
			anim.SetBool ("DashSlash01", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 275 , 90, 20), "LowHSlash01"))
			anim.SetBool ("LowHSlash_01", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 300 , 90, 20), "FireSwd01"))
			anim.SetBool ("FireSwd01", true);
		if(GUI.Button(new Rect(Screen.width - 250 , 325 , 90, 20), "DScissors"))
			anim.SetBool ("DashScissors", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 350 , 90, 20), "DashSlash01"))
			anim.SetBool ("DashSlash01", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 375 , 90, 20), "StepLowSlash"))
			anim.SetBool ("StepInLowSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 400 , 90, 20), "SPattack_01"))
			anim.SetBool ("SPattack_01", true);
		if(GUI.Button(new Rect(Screen.width - 250 , 425 , 90, 20), "JPRollSlash"))
			anim.SetBool ("JPRollingSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 450 , 90, 20), "DeltaSlash"))
			anim.SetBool ("DeltaSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 475 , 90, 20), "BSpinSlSlash"))
			anim.SetBool ("BackSpinSlantSlash01B", true);	
		if(GUI.Button(new Rect(Screen.width - 250 , 500 , 90, 20), "SpiralAttack"))
			anim.SetBool ("SpiralAttack", true);

		if(GUI.Button(new Rect(Screen.width - 150 , 50 , 90, 20), "Dash_F"))
			anim.SetBool ("Dash_F", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 75 , 90, 20), "Dash_B"))
			anim.SetBool ("Dash_B", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 100 , 90, 20), "Dash_L"))
			anim.SetBool ("Dash_L", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 125 , 90, 20), "Dash_R"))
			anim.SetBool ("Dash_R", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 150 , 90, 20), "StpVSlash1"))
			anim.SetBool ("StepInVSlash01", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 175 , 90, 20), "StpVSlash2"))
			anim.SetBool ("StepInVSlash01_02", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 200 , 90, 20), "StpVSlash2B"))
			anim.SetBool ("StepInVSlash01_02B", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 225 , 90, 20), "StpVSlash3"))
			anim.SetBool ("StepInVSlash01_03", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 250 , 90, 20), "BSpinSlash1"))
			anim.SetBool ("BackSpinSlash01", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 275 , 90, 20), "BSpinSlSlash1"))
			anim.SetBool ("BackSpinSlantSlash01", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 300 , 90, 20), "PearsArrow"))
			anim.SetBool ("PearsingArrow", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 325 , 90, 20), "RevLSlash"))
			anim.SetBool ("RevLowSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 350 , 90, 20), "RevMSlash"))
			anim.SetBool ("RevMHSlashW", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 375 , 90, 20), "RevPearse"))
			anim.SetBool ("RevPearse", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 400 , 90, 20), "RevSlSlash"))
			anim.SetBool ("RevSlantSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 425 , 90, 20), "RevUprSlash"))
			anim.SetBool ("RevUpperSlash", true);	
		if(GUI.Button(new Rect(Screen.width - 150 , 450 , 90, 20), "RSlUprSlash"))
			anim.SetBool ("RSlideUpperSlash", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 475 , 90, 20), "LSlUprSlash"))
			anim.SetBool ("LSlideUpperSlash", true);
		if(GUI.Button(new Rect(Screen.width - 150 , 500 , 90, 20), "QJPVSlash01"))
			anim.SetBool ("QJPVSlash01", true);	
	}
	



}
