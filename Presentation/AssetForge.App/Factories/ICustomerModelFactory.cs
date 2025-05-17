using AssetForge.App.Models.Customer;

namespace AssetForge.App.Factories
{
    public interface ICustomerModelFactory
    {
        Task<LoginModel> PrepareLoginModelAsync(bool? checkoutAsGuest);

        Task<MultiFactorAuthenticationProviderModel> PrepareMultiFactorAuthenticationProviderModelAsync(MultiFactorAuthenticationProviderModel providerModel, string sysName, bool isLogin = false);
    }
}
