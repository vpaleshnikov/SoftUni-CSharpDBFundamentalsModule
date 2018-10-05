namespace P03_SalesStartUp
{
    using P03_SalesDatabase;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new SalesContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}
