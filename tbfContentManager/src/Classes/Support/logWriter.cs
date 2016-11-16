using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logClass
{
    class logWriter{

        String path;

        public logWriter(String path) {
            this.path = path;    
        }

        public void logFunc(Boolean consoleOutput, String text)
        {
            if (consoleOutput)
            {
                conOut(text);
                logFile(text);
            }
            else
            {
                logFile(text);
            }
        }

        private void conOut(String text)
        {
            DateTime DateTime = DateTime.Now;
            Console.WriteLine("[" + DateTime + "]: " + text);
            Console.ReadKey();
        }

        private void logFile(String text)
        {
            DateTime DateTime = DateTime.Now;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine("[" + DateTime + "]: " + text);
            }
        }
    }
}
