using System;
using UnityEditor;

namespace _Logic.Extensions.Configs.Editor
{
    public static class FunctionalConfigMenuItem
    {
        [MenuItem("Tool/Fill configs")]
        public static void GenerateEffectTypes()
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
    }
}