namespace _Logic.Extensions.Configs
{
    public interface IExpandedConfig
    {
        public void FindAllDataObjects();
        public void GenerateDataEnumTypes(bool useOldValues = true);
        public void UpdateDataTypes();
    }
}