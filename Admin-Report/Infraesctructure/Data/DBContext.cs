

namespace Infraesctructure.Data
{
    public static class DBContext
    {
        private static string connectionString =
        @"Data Source=128.17.50.183;Initial Catalog=EpicorITSMApplication;User ID=Indicador;Password=50p0rte2013;Encrypt=False";

        public static string GetConnectionString { get { return connectionString; } }
    }
}
