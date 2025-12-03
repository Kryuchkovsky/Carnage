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
    }
}