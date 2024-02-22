using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace _GameLogic.Extensions
{
    public static class ExtensionMethods
    {
        public static void SetLayerRecursively(this Transform parent, int layer)
        {
            parent.gameObject.layer = layer;
 
            for (int i = 0, count = parent.childCount; i < count; i++)
            {
                parent.GetChild(i).SetLayerRecursively(layer);
            }
        }
        
        public static string ConvertArabicNumberToRomanNumber(int number)
        {
            if (number < 0 || number > 3999) throw new ArgumentOutOfRangeException(nameof(number), "insert value between 1 and 3999");
            if (number < 1) return string.Empty;            
            if (number >= 1000) return "M" + ConvertArabicNumberToRomanNumber(number - 1000);
            if (number >= 900) return "CM" + ConvertArabicNumberToRomanNumber(number - 900); 
            if (number >= 500) return "D" + ConvertArabicNumberToRomanNumber(number - 500);
            if (number >= 400) return "CD" + ConvertArabicNumberToRomanNumber(number - 400);
            if (number >= 100) return "C" + ConvertArabicNumberToRomanNumber(number - 100);            
            if (number >= 90) return "XC" + ConvertArabicNumberToRomanNumber(number - 90);
            if (number >= 50) return "L" + ConvertArabicNumberToRomanNumber(number - 50);
            if (number >= 40) return "XL" + ConvertArabicNumberToRomanNumber(number - 40);
            if (number >= 10) return "X" + ConvertArabicNumberToRomanNumber(number - 10);
            if (number >= 9) return "IX" + ConvertArabicNumberToRomanNumber(number - 9);
            if (number >= 5) return "V" + ConvertArabicNumberToRomanNumber(number - 5);
            if (number >= 4) return "IV" + ConvertArabicNumberToRomanNumber(number - 4);
            if (number >= 1) return "I" + ConvertArabicNumberToRomanNumber(number - 1);
            throw new ArgumentOutOfRangeException("Impossible state reached");
        }
        
        public static string ConvertIntToLetter(int value)
        {
            var result = String.Empty;
            
            while (--value >= 0)
            {
                result = (char)('A' + value % 26 ) + result;
                value /= 26;
            }
            
            return result;
        }
        
        public static float GetDistanceBetweenClosestPointsOfColliders(Collider collider1, Collider collider2)
        {
            var closestPoint1 = collider1.ClosestPoint(collider2.transform.position);
            var closetsPoint2 = collider2.ClosestPoint(collider1.transform.position);
            return (closestPoint1 - closetsPoint2).magnitude;
        }

        public static string GetScriptFolderPathByType<T>()
        {
            var scriptName = $"{typeof(T).Name}.cs";
            var res = Directory.GetFiles(Application.dataPath, scriptName, SearchOption.AllDirectories);
            
            if (res.Length == 0)
            {
                Debug.LogError($"The script with name {scriptName} isn't found");
                return null;
            }
            
            var path = res[0].Replace(scriptName, "").Replace("\\", "/");
            return path;
        }
    }
}