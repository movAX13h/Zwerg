using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Utils
{
    class Log
    {
        public static string LOG_ERROR = "!";
        public static string LOG_WARNING = "?";
        public static string LOG_INFO = "i";
        public static string LOG_DEBUG = "D";
        public static string LOG_TRACE = " ";

        public static TextBox GuiTextBox;

        public Log()
        {
            Debug.WriteLine("[Log] class can not be instantiated");
        }

        public static string Filename;
        private static StreamWriter writer;

        public static void WriteLine(string message)
        {
            WriteLine(LOG_TRACE, message, true);
        }

        public static void WriteLine(string type, string message)
        {
            WriteLine(type, message, true);
        }

        public static void WriteLine(string type, string message, bool append)
        {
            if (Filename == null)
            {
                Debug.WriteLine("[Log] prior to writing logs, a filename must be set");
                return;
            }

            string line = "[" + string.Format("{0:G}", DateTime.Now) + "] [" + type + "] " + message;

            if (GuiTextBox != null)
            {
                GuiTextBox.AppendText(line + Environment.NewLine);
            }

            if (writer == null || !append)
            {
                if (writer != null) writer.Close();
                try
                {
                    writer = new StreamWriter(Filename, false);
                }
                catch
                {
                    MessageBox.Show("Failed writing log file!\nCheck config.");
                    return;
                }
            }

            writer.WriteLine(line);
            writer.Flush();
        }
    }
}
