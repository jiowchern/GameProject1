using System.CodeDom;

using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;



public class Property : MonoBehaviour
{


    public UnityEngine.UI.Image StrengthBar;
    public UnityEngine.UI.Text StrengthText;

    public UnityEngine.UI.Image HealthBar;
    public UnityEngine.UI.Text HealthText;

    private Client _Client;

    private IPlayerProperys _PlayerProperys;

    // Use this for initialization
    void Start ()
    {

        _Client = Client.Instance;

        if (_Client != null)
        {
            _Client.User.PlayerProperysProvider.Supply += _SetProperty;
        }

    }

    private void _SetProperty(IPlayerProperys obj)
    {
        _PlayerProperys = obj;
    }

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.PlayerProperysProvider.Supply -= _SetProperty;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (_PlayerProperys != null)
	    {
	        StrengthBar.fillAmount = _PlayerProperys.Strength / 3.0f;
	        StrengthText.text = _PlayerProperys.Strength.ToString();

            HealthBar.fillAmount = _PlayerProperys.Health / 10.0f;
            HealthText.text = _PlayerProperys.Health.ToString();
        }
	}
}
