using _GameLogic.Extensions;
using _Logic.Extensions.CodeGenerator;
using UnityEditor;

namespace _Logic.Extensions.VFXManager.Editor
{
    public static class EffectTypeGenerationMenuItem
    {
        [MenuItem("Tool/Generate effect types")]
        public static void GenerateEffectTypes()
        {
            var path = ExtensionMethods.GetScriptFolderPathByType<EffectsCatalog>() + "GeneratedCode/";
            EnumGenerator.GenerateAll<EffectData>(path, "EffectType");
        }
    }
}