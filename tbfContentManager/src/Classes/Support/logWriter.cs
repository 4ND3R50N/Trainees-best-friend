using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logClass
{
    class LogWriter{
        readonly String path;

        public LogWriter(String path) {
            this.path = path;    
        }

        public void LogFunc(Boolean consoleOutput, String text)
        {
            if (consoleOutput)
            {
                ConOut(text);
                LogFile(text);
            }
            else
            {
                LogFile(text);
            }
        }

        private void ConOut(String text)
        {
            DateTime DateTime = DateTime.Now;
            Console.WriteLine("[" + DateTime + "]: " + text);
            Console.ReadKey();
        }

        private void LogFile(String text)
        {
            DateTime DateTime = DateTime.Now;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine("[" + DateTime + "]: " + text);
            }
        }
    }
}
