namespace Domain.Application.GymSpa.Utilitites
{
    public class Utility
    {
        public static string TypeName<T>()
        {
            return typeof(T).FullName;
        }
    }
}