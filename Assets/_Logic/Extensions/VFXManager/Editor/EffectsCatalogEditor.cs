using _GameLogic.Extensions;
using _Logic.Extensions.CodeGenerator;
using UnityEditor;
using UnityEngine;

namespace _Logic.Extensions.VFXManager.Editor
{
    [CustomEditor(typeof(EffectsCatalog))]
    public class EffectsCatalogEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var catalog = (EffectsCatalog)target;

            if (GUILayout.Button("Update effect types"))
            {
                var path = ExtensionMethods.GetScriptFolderPathByType<EffectsCatalog>() + "GeneratedCode/";
                EnumGenerator.GenerateAll<EffectData>(path, "EffectType");
            }
        }
    }
}