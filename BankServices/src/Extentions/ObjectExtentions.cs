using BankServices.Exceptions;

namespace BankServices.Extentions
{
    public static class ObjectExtensions
    {
        public static bool IsInternalExeption(this object obj)
        {
            return obj.GetType().IsSubclassOf(typeof(BankApplicationException));
        }
    }
}
