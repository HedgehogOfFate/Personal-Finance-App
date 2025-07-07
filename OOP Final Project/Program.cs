namespace OOP_Final_Project
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new PersonalFinanceApp());
        }
    }
}