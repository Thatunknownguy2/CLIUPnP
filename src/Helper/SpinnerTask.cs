using System;
using System.Threading;
using System.Threading.Tasks;

namespace CLIPnP.Helper
{
    internal class SpinnerTask
    {
        public SpinnerTask(string text, Action action)
        { 
            var todoTask = new Task(action);
            char[] loadingChars = new char[] { '/', '-', '\\', '|' };

            todoTask.Start();
            while (true)
            {
                bool taskComplete = false;

                foreach (char c in loadingChars)
                {
                    Console.Write($"\r[{c}] {text}");
                    Thread.Sleep(300);

                    if (todoTask.IsCompleted)
                    {
                        taskComplete = true;
                        Console.WriteLine();
                        break;
                    }
                }

                if (taskComplete)
                    break;
            }
        }
    }
}
