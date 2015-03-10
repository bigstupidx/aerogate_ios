using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using Object = UnityEngine.Object;
using InspectorPlusVar = UISliderColorsInspector.InspectorPlusVar;

[CanEditMultipleObjects]
[CustomEditor(typeof(UISliderColors))]
public class UISliderColorsInspector : Editor {
    public class InspectorPlusVar
    {
        public enum LimitType { None, Max, Min, Range };
        public enum VectorDrawType { None, Direction, Point, PositionHandle, Scale, Rotation };
        public LimitType limitType = LimitType.None;
        public float min = -0.0f;
        public float max = -0.0f;
        public bool progressBar;
        public int iMin = -0;
        public int iMax = -0;
        public bool active = true;
        public string type;
        public string name;
        public string dispName;
        public VectorDrawType vectorDrawType;
        public bool relative = false;
        public bool scale = false;
        public float space = 0.0f;
        public bool[] labelEnabled = new bool[4];
        public string[] label = new string[4];
        public bool[] labelBold = new bool[4];
        public bool[] labelItalic = new bool[4];
        public int[] labelAlign = new int[4];
        public bool[] buttonEnabled = new bool[16];
        public string[] buttonText = new string[16];
        public string[] buttonCallback = new string[16];
        public bool[] buttonCondense = new bool[4];

        public int numSpace = 0;
        public string classType;
        public Vector3 offset = new Vector3(0.5f, 0.5f);
        public bool QuaternionHandle;
	    public bool canWrite = true;
	    public string tooltip;
	    public bool hasTooltip = false;
        public bool toggleStart = false;
        public int toggleSize = 0;
        public int toggleLevel = 0;
        public bool largeTexture;
        public float textureSize;

		public string textFieldDefault;
		public bool textArea;

    public InspectorPlusVar(LimitType _limitType, float _min, float _max, bool _progressBar, int _iMin, int _iMax, bool _active, string _type, string _name, string _dispName,
                        VectorDrawType _vectorDrawType, bool _relative, bool _scale, float _space, bool[] _labelEnabled, string[] _label, bool[] _labelBold, bool[] _labelItalic, int[] _labelAlign, bool[] _buttonEnabled, string[] _buttonText,
                        string[] _buttonCallback, bool[] buttonCondense, int _numSpace, string _classType, Vector3 _offset, bool _QuaternionHandle, bool _canWrite, string _tooltip, bool _hasTooltip,
                        bool _toggleStart, int _toggleSize, int _toggleLevel, bool _largeTexture, float _textureSize, string _textFieldDefault, bool _textArea)
    {
        limitType = _limitType;
        min = _min;
        max = _max;
        progressBar = _progressBar;
        iMax = _iMax;
        iMin = _iMin;
        active = _active;
        type = _type;
        name = _name;
        dispName = _dispName;
        vectorDrawType = _vectorDrawType;
        relative = _relative;
        scale = _scale;
        space = _space;
        labelEnabled = _labelEnabled;
        label = _label;
        labelBold = _labelBold;
        labelItalic = _labelItalic;
        labelAlign = _labelAlign;
        buttonEnabled = _buttonEnabled;
        buttonText = _buttonText;
        buttonCallback = _buttonCallback;
        numSpace = _numSpace;
        classType = _classType;
        offset = _offset;
        QuaternionHandle = _QuaternionHandle;
        canWrite = _canWrite;
        tooltip = _tooltip;
        hasTooltip = _hasTooltip;
        toggleStart = _toggleStart;
        toggleSize = _toggleSize;
        toggleLevel = _toggleLevel;
        largeTexture = _largeTexture;
        textureSize = _textureSize;
		textFieldDefault = _textFieldDefault;
		textArea = _textArea;
    }
    }	
    SerializedObject so;
	SerializedProperty[] properties;
	new string name;
    string dispName;
	Rect tooltipRect;	
	List<InspectorPlusVar> vars;
	void RefreshVars(){for (int i = 0; i < vars.Count; i += 1) properties[i] = so.FindProperty (vars[i].name);}
	void OnEnable ()
	{
        vars = new List<InspectorPlusVar>();
        so = serializedObject;
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None,0,0,false,0,0,true,"UISprite","sprite","Sprite",InspectorPlusVar.VectorDrawType.None,false,false,0,new System.Boolean[]{false,false,false,false},new System.String[]{"","","",""},new System.Boolean[]{false,false,false,false},new System.Boolean[]{false,false,false,false},new System.Int32[]{0,0,0,0},new System.Boolean[]{false,false,false,false},new System.String[]{"","","",""},new System.String[]{"","","",""},new System.Boolean[]{false,false,false,false},0,"UISliderColors",new Vector3(0.5f,0.5f,0f),false,true,"",false,false,0,0,false,70,"",false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None,0,0,false,0,0,true,"Color[]","colors","Colors",InspectorPlusVar.VectorDrawType.None,false,false,0,new System.Boolean[]{false,false,false,false},new System.String[]{"","","",""},new System.Boolean[]{false,false,false,false},new System.Boolean[]{false,false,false,false},new System.Int32[]{0,0,0,0},new System.Boolean[]{false,false,false,false},new System.String[]{"","","",""},new System.String[]{"","","",""},new System.Boolean[]{false,false,false,false},0,"UISliderColors",new Vector3(0.5f,0.5f,0f),false,true,"",false,false,0,0,false,70,"",false));	
		int count = vars.Count;
		properties = new SerializedProperty[count];
	}
    
	void ArrayGUI (SerializedProperty sp, string name)
	{
		EditorGUIUtility.LookLikeControls (120.0f, 40.0f);
		GUILayout.Space (4.0f);
		EditorGUILayout.BeginVertical ("box", GUILayout.MaxWidth(Screen.width));

		int i = 0;
		int del = -1;

		SerializedProperty array = sp.Copy ();
		SerializedProperty size = null;
		bool first = true;

		while (true) {
			if (sp.propertyPath != name && !sp.propertyPath.StartsWith (name + "."))
				break;

			bool child;
            EditorGUI.indentLevel = sp.depth;

			if (sp.depth == 1 && !first) {
				EditorGUILayout.BeginHorizontal ();

				if (GUILayout.Button ("", "OL Minus", GUILayout.Width (24.0f)))
					del = i;

				child = EditorGUILayout.PropertyField (sp);

				GUI.enabled = i > 0;

				if (GUILayout.Button ("U", "ButtonLeft", GUILayout.Width (24.0f), GUILayout.Height(18.0f)))
					array.MoveArrayElement (i - 1, i);

				GUI.enabled = i < array.arraySize - 1;
                if (GUILayout.Button("D", "ButtonRight", GUILayout.Width(24.0f), GUILayout.Height(18.0f)))
					array.MoveArrayElement (i + 1, i);

				++i;

				GUI.enabled = true;
				EditorGUILayout.EndHorizontal ();
			} else if (sp.depth == 1) {
				first = false;
				size = sp.Copy ();

				EditorGUILayout.BeginHorizontal ();

                if (!size.hasMultipleDifferentValues && GUILayout.Button("", "OL Plus", GUILayout.Width(24.0f)))
					array.arraySize += 1;


				child = EditorGUILayout.PropertyField (sp);

				EditorGUILayout.EndHorizontal ();
			} else {
                child = EditorGUILayout.PropertyField(sp);
			}

			if (!sp.NextVisible (child))
				break;
		}

		sp.Reset ();

		if (del != -1)
			array.DeleteArrayElementAtIndex (del);

		if (array.isExpanded && !size.hasMultipleDifferentValues) {
			EditorGUILayout.BeginHorizontal ();

            if (GUILayout.Button("", "OL Plus", GUILayout.Width(24.0f)))
				array.arraySize += 1;

			GUI.enabled = false;
			EditorGUILayout.PropertyField (array.GetArrayElementAtIndex (array.arraySize - 1), new GUIContent ("" + array.arraySize));
			GUI.enabled = true;

			EditorGUILayout.EndHorizontal ();
		}


        EditorGUI.indentLevel = 0;
		EditorGUILayout.EndVertical ();
		EditorGUIUtility.LookLikeControls (170.0f, 80.0f);
	}
	void PropertyField (SerializedProperty sp, string name)
	{
		if (sp.hasChildren) {
            GUILayout.BeginVertical();
			while (true) {
				if (sp.propertyPath != name && !sp.propertyPath.StartsWith (name + "."))
					break;

				EditorGUI.indentLevel = sp.depth;
                bool child = false;

                if (sp.depth == 0)
                    child = EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
                else
                    child = EditorGUILayout.PropertyField(sp);

				if (!sp.NextVisible (child))
					break;
			}
            EditorGUI.indentLevel = 0;
            GUILayout.EndVertical();
		} else EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
	}
	public override void OnInspectorGUI ()
	{	
		so.Update ();
		RefreshVars();
		
		EditorGUIUtility.LookLikeControls (135.0f, 50.0f);

		for (int i = 0; i < properties.Length; i += 1) 
		{
			InspectorPlusVar v = vars[i];
			
			if (v.active && properties[i] != null) 
			{
				SerializedProperty sp = properties [i];string s = v.type;
							 bool skip = false;
				name = v.name;
                dispName = v.dispName;

				GUI.enabled = v.canWrite;

                GUILayout.BeginHorizontal();

                if (v.toggleLevel != 0)
                   GUILayout.Space(v.toggleLevel * 10.0f);
                
                if (sp.isArray && s != typeof(string).Name){
                    ArrayGUI(sp, name);
                    skip = true;
                }
                if (!skip)
                    PropertyField(sp, name);
                GUILayout.EndHorizontal();
                GUI.enabled = true;
			}
		}
        so.ApplyModifiedProperties (); 
        //NOTE NOTE NOTE: WATERMARK HERE
        //You are free to remove this
        //START REMOVE HERE
        GUILayout.BeginHorizontal();
        GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        GUILayout.FlexibleSpace();
        GUILayout.Label("Created with");
        GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
        if (GUILayout.Button("Inspector++"))
            Application.OpenURL("http://forum.unity3d.com/threads/136727-Inspector-Meh-to-WOW-inspectors");
        GUI.color = new Color(1.0f, 1.0f, 1.0f);
		GUILayout.EndHorizontal();
        //END REMOVE HERE
    }
}