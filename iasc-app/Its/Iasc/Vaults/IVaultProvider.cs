
namespace Its.Iasc.Vaults
{
    public interface IVaultProvider
    {
        void Load();
        string GetValue(string key);
    }
}