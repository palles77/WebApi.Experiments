using System.ComponentModel.DataAnnotations;

namespace LpApi_20210506.Common
{
    public class RequiredIfAttribute : RequiredAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance, null);
            
            if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue.ToString()) && 
                Equals(propertyValue.ToString(), DesiredValue.ToString()))
            {
                ValidationResult result = base.IsValid(value, context);
                return result;
            }

            return ValidationResult.Success;
        }
    }
}
