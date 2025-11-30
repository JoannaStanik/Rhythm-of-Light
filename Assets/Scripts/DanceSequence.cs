using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(
    fileName = "NewDanceSequence",
    menuName = "Rhythm/Dance Sequence")]
public class DanceSequence : ScriptableObject
{
    [Header("Informacje o utworze")]
    public string songName;
    public Sprite coverSprite;

    [System.Serializable]
    public struct Step
    {
        public KeyCode key;
        public float delay;
    }

    [Header("Kroki sekwencji (opóŸnienie po poprzednim kroku)")]
    public Step[] steps;

    [Header("Okna czasowe trafienia")]
    public float allowedEarly = 0.12f;
    public float allowedLate = 0.12f;

    [Header("Progi oceny (wartoœci bezwzglêdne)")]
    public float perfectWindow = 0.05f;
    public float goodWindow = 0.10f;
}

#if UNITY_EDITOR

[CustomEditor(typeof(DanceSequence))]
public class DanceSequenceEditor : Editor
{
    SerializedProperty songNameProp;
    SerializedProperty coverSpriteProp;
    SerializedProperty stepsProp;
    SerializedProperty allowedEarlyProp;
    SerializedProperty allowedLateProp;
    SerializedProperty perfectWindowProp;
    SerializedProperty goodWindowProp;

    void OnEnable()
    {
        songNameProp = serializedObject.FindProperty("songName");
        coverSpriteProp = serializedObject.FindProperty("coverSprite");
        stepsProp = serializedObject.FindProperty("steps");
        allowedEarlyProp = serializedObject.FindProperty("allowedEarly");
        allowedLateProp = serializedObject.FindProperty("allowedLate");
        perfectWindowProp = serializedObject.FindProperty("perfectWindow");
        goodWindowProp = serializedObject.FindProperty("goodWindow");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Informacje o utworze", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(songNameProp, new GUIContent("Nazwa piosenki"));
        EditorGUILayout.PropertyField(coverSpriteProp, new GUIContent("Ok³adka"));

        var sprite = coverSpriteProp.objectReferenceValue as Sprite;
        if (sprite != null)
        {
            float size = 128f;
            Rect rect = GUILayoutUtility.GetRect(size, size, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(rect, sprite.texture, null, ScaleMode.ScaleToFit);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Kroki sekwencji", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(stepsProp, true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Parametry czasowe", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(allowedEarlyProp, new GUIContent("Allowed Early"));
        EditorGUILayout.PropertyField(allowedLateProp, new GUIContent("Allowed Late"));
        EditorGUILayout.PropertyField(perfectWindowProp, new GUIContent("Perfect Window"));
        EditorGUILayout.PropertyField(goodWindowProp, new GUIContent("Good Window"));

        serializedObject.ApplyModifiedProperties();
    }
}

public class DanceSequenceEditorWindow : EditorWindow
{
    private DanceSequence sequence;
    private SerializedObject serializedSequence;

    SerializedProperty songNameProp;
    SerializedProperty coverSpriteProp;
    SerializedProperty stepsProp;
    SerializedProperty allowedEarlyProp;
    SerializedProperty allowedLateProp;
    SerializedProperty perfectWindowProp;
    SerializedProperty goodWindowProp;

    [MenuItem("Window/Rhythm/Dance Sequence Editor")]
    public static void OpenWindow()
    {
        var window = GetWindow<DanceSequenceEditorWindow>("Dance Sequence");
        window.Show();
    }

    void SetSequence(DanceSequence seq)
    {
        sequence = seq;
        if (sequence != null)
        {
            serializedSequence = new SerializedObject(sequence);

            songNameProp = serializedSequence.FindProperty("songName");
            coverSpriteProp = serializedSequence.FindProperty("coverSprite");
            stepsProp = serializedSequence.FindProperty("steps");
            allowedEarlyProp = serializedSequence.FindProperty("allowedEarly");
            allowedLateProp = serializedSequence.FindProperty("allowedLate");
            perfectWindowProp = serializedSequence.FindProperty("perfectWindow");
            goodWindowProp = serializedSequence.FindProperty("goodWindow");
        }

        Repaint();
    }

    void OnSelectionChange()
    {
        if (Selection.activeObject is DanceSequence ds)
        {
            SetSequence(ds);
        }
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Dance Sequence Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        var newSeq = (DanceSequence)EditorGUILayout.ObjectField(
            "Sekwencja",
            sequence,
            typeof(DanceSequence),
            false);

        if (newSeq != sequence)
            SetSequence(newSeq);

        if (sequence == null)
        {
            EditorGUILayout.HelpBox(
                "Wybierz asset typu DanceSequence",
                MessageType.Info);
            return;
        }

        serializedSequence.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Informacje o utworze", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(songNameProp, new GUIContent("Nazwa piosenki"));
        EditorGUILayout.PropertyField(coverSpriteProp, new GUIContent("Ok³adka"));

        var sprite = coverSpriteProp.objectReferenceValue as Sprite;
        if (sprite != null)
        {
            float size = 128f;
            Rect rect = GUILayoutUtility.GetRect(size, size, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(rect, sprite.texture, null, ScaleMode.ScaleToFit);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Kroki sekwencji", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(stepsProp, true);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Parametry czasowe", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(allowedEarlyProp, new GUIContent("Allowed Early"));
        EditorGUILayout.PropertyField(allowedLateProp, new GUIContent("Allowed Late"));
        EditorGUILayout.PropertyField(perfectWindowProp, new GUIContent("Perfect Window"));
        EditorGUILayout.PropertyField(goodWindowProp, new GUIContent("Good Window"));

        serializedSequence.ApplyModifiedProperties();
    }
}
#endif
