using UnityEngine;
using System.Collections;

public class TalkReceiver : MonoBehaviour {

    public UnityEngine.UI.Text Text;

    public UnityEngine.UI.Image Image;

    Regulus.Utility.TimeCounter _Counter;    
    public TalkReceiver()
    {
        _Counter = new Regulus.Utility.TimeCounter();
    }
    // Use this for initialization
    void Start () {
        
        
    }
    void Enable()
    {
        
    }
        // Update is called once per frame
    void Update()
    {
        if (_Counter.Second > 3)
        {
            Image.gameObject.SetActive(false);
            Text.gameObject.SetActive(false);
        }        

    }

    public void Show(string s)
    {
        Text.text = s;
        Image.gameObject.SetActive(true);
        Text.gameObject.SetActive(true);
        _Counter.Reset();
    }
}
