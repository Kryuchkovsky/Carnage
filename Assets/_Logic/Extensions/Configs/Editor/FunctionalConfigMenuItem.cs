using System;
using UnityEditor;

namespace _Logic.Extensions.Configs.Editor
{
    public static class FunctionalConfigMenuItem
    {
        [MenuItem("Tools/Configs/Fill configs")]
        public static void FillConfigs()
        {
            try
            {
                ConfigManager.FillConfigs();
            }
            catch (Exception e)
            {
                Console.WriteLine("The configs manager doesn't exist!");
                throw;
            }
        }
        
        [MenuItem("Tools/Configs/Generate new enum types")]
        public static void GenerateDataEnumTypes()
        {
            try
            {
                ConfigManager.GenerateDataEnumTypes();
            }
            catch (Exception e)
            {
                Console.WriteLine("The configs manager doesn't exist!");
                throw;
            }
        }
        
        [MenuItem("Tools/Configs/Regenerate all enum types")]
        public static void RegenerateDataEnumTypes()
        {
            try
            {
                ConfigManager.GenerateDataEnumTypes(false);
            }
            catch (Exception e)
            {
                Console.WriteLine("The configs manager doesn't exist!");
                throw;
            }
        }
    }
}