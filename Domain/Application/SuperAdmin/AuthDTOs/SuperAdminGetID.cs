namespace Domain.Application.SuperAdmin.AuthDTOs
{
    public static class SuperAdminGetID
    {
        private static int _Id;

        public static int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }
}