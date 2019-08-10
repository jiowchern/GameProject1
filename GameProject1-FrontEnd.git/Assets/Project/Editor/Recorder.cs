using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Regulus.Extension;
using Regulus.Project.GameProject1.Data;

using UnityEditor;
using UnityEditor.Animations;

public class Recorder : EditorWindow {
    [MenuItem("Regulus/GameProject1/Recorder")]
    public static void Open()
    {
        var wnd = EditorWindow.GetWindow(typeof(Recorder));
        wnd.Show();
    }

    public Recorder()
    {
        _State = "";
    }

    Object _Ani;

    private string _Layer = "Base Layer";

    private string _State;

    private float _Interval = 1.0f / 30.0f;

    private Object _Drawer;

    private float _BeginTime;

    private float _EndTime;

    private Object _Skill;

    private Object _Clip;

    private Object _Player;

    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        var pervAni = _Ani;
        var pervClip = _Clip;

        _Player = EditorGUILayout.ObjectField("Player", _Player, typeof(GameObject), true);
        _Ani = EditorGUILayout.ObjectField("Animator", _Ani, typeof(Animator) , true);

        
        _Clip = EditorGUILayout.ObjectField("AnimationClip", _Clip, typeof(AnimationClip), true);
        _Layer = EditorGUILayout.TextField("Layer", _Layer);
        _State = EditorGUILayout.TextField("State", _State);
        _BeginTime = EditorGUILayout.FloatField("BeginTime", _BeginTime);
        _EndTime = EditorGUILayout.FloatField("EndTime", _EndTime);
        _Interval = EditorGUILayout.FloatField("Interval", _Interval);
        _Drawer = EditorGUILayout.ObjectField("Drawer", _Drawer, typeof(DeterminationDrawer), true);
        _Skill = EditorGUILayout.ObjectField("Skill", _Skill, typeof(SkillExportMark), true);


        if (GUILayout.Button("Go Animator"))
        {
            _Catche();
        }
        if (GUILayout.Button("Go Clip"))
        {
            _Record(_Clip as AnimationClip);
        }
        EditorGUILayout.EndVertical();

        if (_Ani != pervAni)
        {

            _UpdateTime(_Ani as Animator);
        }

        if(pervClip != _Clip)
        {
            _UpdateTime(_Clip as AnimationClip);
        }
    }

    private void _Record(AnimationClip clip)
    {
        var data = _FindPoints(clip, _Interval);

        _Record(data);
    }

    private void _UpdateTime(Animator anim)
    {
        if (anim == null)
            return;        
        
        var ac = anim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;        
        if (ac == null)
        {
            return;
        }
        var state = _FindLayer(ac, _Layer);
        if (state == null)
            return;
        var clip = _FindClips(state, _State);
        _UpdateTime(clip);
        
    }

    private void _UpdateTime(AnimationClip clip)
    {
        if (clip == null)
            return;
        _BeginTime = 0;
        _EndTime = clip.length;
    }

    private void _Catche()
    {
        var anim = _Ani as Animator;
        var ac = anim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        if(ac != null)
        {
            var state = _FindLayer(ac, _Layer);
            var clip = _FindClips(state , _State);
            _Record(clip);
        }        
    }
    
    private void _Record(SkillData data)
    {
        var drawer = _Drawer as DeterminationDrawer;
        if (drawer != null)
        {
            drawer.Left = (from l in data.Lefts select new UnityEngine.Vector2(l.X, l.Y )).ToArray();
            drawer.Right = (from r in data.Rights select new UnityEngine.Vector2(r.X, r.Y)).ToArray();
            drawer.Total = data.Total;
            drawer.Begin = data.Begin;
            drawer.End = data.End;
        }

        var skillExportMark = _Skill as SkillExportMark;
        if (skillExportMark != null)
        {
            skillExportMark.Data = data;
        }

    }

    private SkillData _FindPoints( AnimationClip clip, float interval)
    {
        var data = new SkillData();         
        
        var go = _Player as GameObject;
        
        var marks = go.GetComponentsInChildren<DeterminationExportMark>(); 
        var markLeft = (from m in marks where m.Part == DeterminationExportMark.PART.LEFT select m).Single(); 
        var markRight = (from m in marks where m.Part == DeterminationExportMark.PART.RIGHT select m).Single();
        var markRoot = (from m in marks where m.Part == DeterminationExportMark.PART.ROOT select m).Single();
        var len = clip.length;
        
        List<Vector2> left = new List<Vector2>();
        List<Vector2> right = new List<Vector2>();
        List<Translate> root = new List<Translate>();

        float eulerAngle = go.transform.rotation.eulerAngles.y;
        var rootPosition = new Vector2(go.transform.position.x, go.transform.position.z);
        var basePosition = markRoot.Position; 
        Debug.Log("basePosition  : " + basePosition);
        for (var i = 0.0f ; i < len; i += interval) 
        {           
            clip.SampleAnimation(go, i);
            
            var currentposition = markRoot.Position - basePosition ;            
            var position = currentposition;
            var y = go.transform.rotation.eulerAngles.y;
            var t = new Translate()
            {
                Position = new Regulus.CustomType.Vector2(position.x, position.y),
                Rotation = eulerAngle - y
            };
            
            root.Add(t);
            eulerAngle = y;
            if (i >= _BeginTime && i <= _EndTime)
            {
                left.Add(markLeft.Position - rootPosition  );

                right.Add(markRight.Position - rootPosition );
            }
        }
        data.Lefts = (from l in left select new Regulus.CustomType.Vector2(l.x, l.y)).ToArray();
        data.Rights = (from r in right select new Regulus.CustomType.Vector2(r.x, r.y)).ToArray();
        data.Roots = root.ToArray();
        data.Total = len;
        data.Begin = _BeginTime;
        data.End = _EndTime;

        


        return data;
    }

    private AnimationClip _FindClips(AnimatorStateMachine state, string state_name)
    {
        foreach (var child in state.states)
        {
            
            Debug.Log("Clip name " + child.state.name);
            if (child.state.name == state_name)
            {
                return child.state.motion as AnimationClip;
            }
        }

        return null;
        return
            (from child in state.states where child.state.name == state_name select child.state.motion as AnimationClip)
                .SingleOrDefault();
    }

    private UnityEditor.Animations.AnimatorStateMachine _FindLayer(AnimatorController animator_controller, string layer)
    {
        foreach (var l in animator_controller.layers)
        {
            Debug.Log("Find Layer "+ l.name);
            if (l.name == layer)
                return l.stateMachine;            
        }
        return null;

        //return (from l in animator_controller.layers where l.name == layer select l.stateMachine).First();
    }

    
    
}

