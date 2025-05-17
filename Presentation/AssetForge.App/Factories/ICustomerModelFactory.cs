using AssetForge.App.Models.Customer;
using AssetForge.Core.Domain.Customers;

namespace AssetForge.App.Factories
{
    public interface ICustomerModelFactory
    {
        Task<LoginModel> PrepareLoginModelAsync(bool? checkoutAsGuest);

        Task<MultiFactorAuthenticationProviderModel> PrepareMultiFactorAuthenticationProviderModelAsync(MultiFactorAuthenticationProviderModel providerModel, string sysName, bool isLogin = false);

        Task<CustomerInfoModel> PrepareCustomerInfoModelAsync(CustomerInfoModel model, Customer customer,
            bool excludeProperties, string overrideCustomCustomerAttributesXml = "");

        Task<RegisterModel> PrepareRegisterModelAsync(RegisterModel model, bool excludeProperties,
            string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false);
    }
}
