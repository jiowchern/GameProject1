using UnityEngine;
using System.Collections;

using Regulus.Utility;

public class Console : MonoBehaviour 
    , Regulus.Utility.Console.IInput
     , Regulus.Utility.Console.IViewer
{
    private Regulus.Utility.Console _Console;
    public string Title;
    const int LineCount = 25;
    const int MaxLineCount = 100;

    readonly System.Collections.Generic.Queue<string> _Messages;
    string _LastMessage;
    public string _Input;

    public Command Command
    {
        get { return _Console.Command; }
    }

    public Console()
    {
        _Messages = new System.Collections.Generic.Queue<string>();
        _Input = "";
        _LastMessage = "";
        _ScrollView = Vector2.zero;
        _Console = new Regulus.Utility.Console(this ,this);
        
    }
	// Use this for initialization
	void Start () 
    {
        Regulus.Utility.Log.Instance.RecordEvent += _WriteLine;

    }

    void OnGUI()
    {
        GUILayout.Window(0, new Rect(0, 0, Screen.width / 2, Screen.height) , _WindowHandler, Title); 
        //GUILayout.Window(0, new Rect(0, 0, 500  / 2, 500), _WindowHandler, Title); 
    }
    Vector2 _ScrollView;
    private void _WindowHandler(int id)
    {
        GUILayout.BeginVertical();

        _ScrollView = GUILayout.BeginScrollView(_ScrollView, GUILayout.Width(Screen.width / 2), GUILayout.Height(Screen.height * 0.9f));

        foreach(var message in _Messages)
        {
            GUILayout.Label(message);
        }        

       
        if (_LastMessage.Length > 0)
            GUILayout.Label(_LastMessage);

        GUILayout.EndScrollView();

        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        _Input = GUILayout.TextField(_Input);
        if(GUILayout.Button("Send") && _Input != string.Empty)
        {
            _WriteLine(_Input);
            var args = _Input.Split( new char[] {' '} , System.StringSplitOptions.RemoveEmptyEntries );            
            if (_OutputEvent != null)
                _OutputEvent(args);

            _Input = "";
        }
        GUILayout.EndHorizontal();
    }

    
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    event Regulus.Utility.Console.OnOutput _OutputEvent;
    event Regulus.Utility.Console.OnOutput Regulus.Utility.Console.IInput.OutputEvent
    {
        add { _OutputEvent += value; }
        remove { _OutputEvent -= value; }
    }

    void Regulus.Utility.Console.IViewer.Write(string message)
    {
        _LastMessage += message;
    }

    void Regulus.Utility.Console.IViewer.WriteLine(string message)
    {
        _WriteLine(message);
    }
    private void _WriteLine(string text)
    {
        _Messages.Enqueue(_LastMessage + text);
        if (_Messages.Count > MaxLineCount)
            _Messages.Dequeue();
        _LastMessage = "";

        _ScrollView.y = Mathf.Infinity;
    }

    public void WriteLine(string text)
    {
        var lines = text.Split(new char[] {'\n' , '\r' });
        foreach(var line in lines)
        {
            _WriteLine(text);
        }
        
    }
    
}

