namespace P01_HospitalDatabase
{
    using P01_HospitalDatabase.Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new HospitalContext())
            {
                DatabaseInitializer.ResetDatabase();
            }
        }
    }
}
