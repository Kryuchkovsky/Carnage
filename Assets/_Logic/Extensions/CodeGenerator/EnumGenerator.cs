using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Logic.Extensions.CodeGenerator
{
    public static class EnumGenerator
    {
        public static void GenerateAll<T>(string enumCreatePath, string targetEnumName) where T : Object
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            var enumsToBeAdded = new string[guids.Length];

            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                enumsToBeAdded[i] = asset.name;
            }

            Generate(enumCreatePath, targetEnumName, enumsToBeAdded);
        }

        private static void Generate(string enumCreationPath, string targetEnumName, IReadOnlyList<string> enumsToBeAdded)
        {
            if (!Directory.Exists(enumCreationPath))
            {
                Directory.CreateDirectory(enumCreationPath);
            }

            var filePathAndName = $"{enumCreationPath}{targetEnumName}.cs";
            var oldEnums = GetCurrentEnums(filePathAndName);
            var oldEnumsExist = oldEnums != null && oldEnums.Values.Count > 0;
            var highestValue = oldEnumsExist ? oldEnums.Values.Max() + 1 : 0;

            using (var streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("public enum " + targetEnumName + "\r\n" + "{");
                //streamWriter.WriteLine("{");

                for (var index = 0; index < enumsToBeAdded.Count; index++)
                {
                    var enumString = enumsToBeAdded[index];

                    if (oldEnumsExist && oldEnums.TryGetValue(enumString, out var oldEnumNumber))
                    {
                        streamWriter.WriteLine("    " + enumString + " = " + oldEnumNumber + ",");
                    }
                    else
                    {
                        var newEnumNumber = highestValue + index;
                        streamWriter.WriteLine("    " + enumString + " = " + newEnumNumber + ",");
                    }
                }

                streamWriter.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        private static Dictionary<string, int> GetCurrentEnums(string filePathAndName)
        {
            if (!File.Exists(filePathAndName)) return null;

            var enums = new Dictionary<string, int>();
            var lines = File.ReadAllLines(filePathAndName);

            foreach (var line in lines)
            {
                if (!line.Contains("=")) continue;

                var enumName = line.Split('=')[0].Trim();
                var enumNumber = line.Split('=')[1].Trim().TrimEnd(',');
                enums.Add(enumName, int.Parse(enumNumber));
            }

            return enums;
        }
    }
}