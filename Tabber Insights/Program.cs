namespace Tabber_Insights
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string localAppData =
            Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);
            string userFilePath
              = Path.Combine(localAppData, "Tabber");

            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);

            //if it's not already there, 
            //copy the file from the deployment location to the folder
            string sourceFilePath = Path.Combine(
              System.Windows.Forms.Application.StartupPath, "TabberInsightsData.txt");
            string destFilePath = Path.Combine(userFilePath, "TabberInsightsData.txt");
            if (!File.Exists(destFilePath))
                File.Copy(sourceFilePath, destFilePath);

            //SetProcessDPIAware();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            if (Environment.OSVersion.Version.Major >= 6)
            SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TabberInsights());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}