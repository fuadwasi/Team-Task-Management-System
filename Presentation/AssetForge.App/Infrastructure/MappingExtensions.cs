using AssetForge.App.Models.Api;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Specialized;

namespace AssetForge.App.Infrastructure
{
    public static class MappingExtensions
    {
        public static IList<string> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            foreach (var ms in modelState.Values)
                foreach (var error in ms.Errors)
                    errors.Add(error.ErrorMessage);

            return errors;
        }

        public static NameValueCollection ToNameValueCollection(this List<KeyValueApi> formValues)
        {
            var form = new NameValueCollection();
            if (formValues == null)
                return form;

            foreach (var values in formValues)
            {
                form.Add(values.Key, values.Value);
            }
            return form;
        }
    }
}
