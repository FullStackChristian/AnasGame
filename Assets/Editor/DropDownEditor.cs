using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoorManager))]
public class DropDownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Draw default GUI first before editing
        DrawDefaultInspector();

        //Code for editing layout
        //base.OnInspectorGUI();

        DoorManager script = (DoorManager)target;

        GUIContent locationArrayList = new GUIContent("NewLocationType");
        script.LocationIdx = EditorGUILayout.Popup(locationArrayList, script.LocationIdx, script.LstLocationtypes.ToArray());

        /*if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }*/
    }
}
