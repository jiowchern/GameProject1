using UnityEngine;
using System.Collections;
using Regulus.Project.GameProject1.Data;
using System;

public class TalkSender : MonoBehaviour {
    private Client _Client;
    private IEmotion _EmotionSkill;

    
    public UnityEngine.UI.InputField InputField;
    void OnDestroy()
    {
        if (_Client != null)
        {
            
            _Client.User.EmotionControllerProvider.Supply -= _EmotionControllerProvider_Supply;
            _Client.User.EmotionControllerProvider.Unsupply -= _EmotionControllerProvider_Unsupply;
        }
    }
    // Use this for initialization
    void Start () {
        _Client = Client.Instance;

        if (_Client != null)
        {
            _Client.User.EmotionControllerProvider.Supply += _EmotionControllerProvider_Supply;
            _Client.User.EmotionControllerProvider.Unsupply += _EmotionControllerProvider_Unsupply;
        }

	}

    private void _EmotionControllerProvider_Unsupply(IEmotion obj)
    {
        _EmotionSkill = null;
    }

    private void _EmotionControllerProvider_Supply(Regulus.Project.GameProject1.Data.IEmotion obj)
    {
        _EmotionSkill = obj;
    }

    public void Send()
    {
        
    }
    // Update is called once per frame
    void Update ()
    {
	    if(Input.GetKeyUp(KeyCode.Return))
	    {
	        var show = InputField.gameObject.activeSelf;
	        if (!show)
	        {
                InputField.text = "";
                InputField.gameObject.SetActive(true);
                InputField.ActivateInputField();
            }
            else
            {
                if (_EmotionSkill != null && InputField.text.Length > 0)
                {
                    _EmotionSkill.Talk(InputField.text);
                }
                InputField.gameObject.SetActive(false);
            }
        }
	    
	}
}

