using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _GameLogic.Extensions;
using UnityEditor;
using Object = UnityEngine.Object;

namespace _Logic.Extensions.CodeGenerator
{
    public static class EnumGenerator
    {
        public static void GenerateEnumValues<TObject, TEnumType>() where TObject : Object where TEnumType : Enum
        {
            var enumFilePath = ExtraMethods.GetScriptPath<TEnumType>(true);
            var enumName = typeof(TEnumType).Name;
            var objName = typeof(TObject).Name;
            var objectGuids = AssetDatabase.FindAssets("t:" + objName);
            var enumsToBeAdded = new string[objectGuids.Length];

            for (var i = 0; i < objectGuids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(objectGuids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                enumsToBeAdded[i] = asset.name;
            }
            
            var oldEnums = GetCurrentEnums(enumFilePath);
            var oldEnumsExist = oldEnums != null && oldEnums.Values.Count > 0;
            var highestValue = oldEnumsExist ? oldEnums.Values.Max() + 1 : 0;

            using (var streamWriter = new StreamWriter(enumFilePath))
            {
                streamWriter.WriteLine("public enum " + enumName + "\r\n" + "{");

                for (var index = 0; index < enumsToBeAdded.Length; index++)
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