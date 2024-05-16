using System.ComponentModel.DataAnnotations;

namespace DocumentManagementSystem.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DocumentDateAttribute : ValidationAttribute
    {
        public DocumentDateAttribute()
        {
            DocumentDate = DateTime.Now;
        }

        public DateTime DocumentDate { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            DateTime val;
            try
            {
                val = (DateTime)value;
            }
            catch (InvalidCastException)
            {
                return false;
            }

            if (val > DocumentDate)
                return false;

            return true;
        }
    }
}
