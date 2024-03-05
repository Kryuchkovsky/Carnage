using System;
using UnityEditor;

namespace _Logic.Extensions.Configs.Editor
{
    public static class FunctionalConfigMenuItem
    {
        [MenuItem("Tools/Fill configs")]
        public static void FillConfigs()
        {
            try
            {
                ConfigsManager.FillConfigs();
            }
            catch (Exception e)
            {
                Console.WriteLine("The configs manager doesn't exist!");
                throw;
            }
        }
        
        [MenuItem("Tools/Generate enum types")]
        public static void GenerateEnumDataTypes()
        {
            try
            {
                ConfigsManager.GenerateDataEnumTypes();
            }
            catch (Exception e)
            {
                Console.WriteLine("The configs manager doesn't exist!");
                throw;
            }
        }
    }
}