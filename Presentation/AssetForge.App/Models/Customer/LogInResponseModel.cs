namespace AssetForge.App.Models.Customer
{
    public class LogInResponseModel
    {
        public LogInResponseModel()
        {
            CustomerInfo = new CustomerInfoModel();
        }

        public CustomerInfoModel CustomerInfo { get; set; }

        public bool MultiFactorAuthenticationRequired { get; set; }

        public string Token { get; set; }
    }
}
