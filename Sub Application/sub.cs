using System;
using System.Windows.Forms;

namespace Sub_Application
{
    class sub
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SubscriberForm());
        }
    }
}
