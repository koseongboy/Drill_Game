using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 1. GUI 활성화 상태를 저장
        bool wasEnabled = GUI.enabled;

        // 2. GUI를 비활성화 (읽기 전용 상태로 만듦)
        GUI.enabled = false;

        // 3. 필드를 그림
        EditorGUI.PropertyField(position, property, label, true);

        // 4. GUI 활성화 상태를 복원
        GUI.enabled = wasEnabled;
    }
}