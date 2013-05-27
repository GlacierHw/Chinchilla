using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Chinchilla
{
    /// <summary>
    /// Command info class
    /// </summary>
    public class CommandInfo
    {
        public string command {get; set;}
        public int delayTime { get; set;}

        public CommandInfo(string command, int delayTime)
        {
            this.command = command;
            this.delayTime = delayTime;
        }

        public CommandInfo()
        {

        }
    }

    /// <summary>
    /// Execute command class
    /// </summary>
    public class Executecmd
    {
        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public static void ExecuteCommandSync(object command, int delayTime)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                if (delayTime > 0)
                    Thread.Sleep(delayTime);
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
                proc.WaitForExit();
            }
            catch (Exception objException)
            {
                // Log the exception
                Console.WriteLine(objException.Message);
            }
        }

        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public static void ExecuteCommandSync(object command, out string result)
        {
            try
            {
      
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

               
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
               
                procStartInfo.CreateNoWindow = true;
            
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
               
                result = proc.StandardOutput.ReadToEnd();
                proc.Dispose();
                // Display the command output.
                //Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // Log the exception
                Console.WriteLine(objException.Message);
                result = String.Empty;
            }
        }

        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public static void ExecuteCommandSync(object commandInfo)
        {
            try
            {
                CommandInfo c = (CommandInfo)commandInfo;
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + c.command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                Thread.Sleep(c.delayTime);
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
                //proc.WaitForExit();
            }
            catch (Exception objException)
            {
                // Log the exception
                Console.WriteLine(objException.Message);
            }
        }

        /// <summary>
        /// Execute the command Asynchronously.
        /// </summary>
        /// <param name="command">string command.</param>
        public static void ExecuteCommandAsync(string command,int delayTime)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                //Make the thread as background thread.
                objThread.IsBackground = true;
                //Set the Priority of the thread.
                objThread.Priority = ThreadPriority.AboveNormal;
                //Start the thread.
                objThread.Start(new CommandInfo(command,delayTime));
              
            }
            catch (ThreadStartException objException)
            {
                // Log the exception
                Console.WriteLine(objException.Message);
            }
            catch (ThreadAbortException objException)
            {
                // Log the exception
                Console.WriteLine(objException.Message);
            }
            catch (Exception objException)
            {
                // Log the exception
                Console.WriteLine(objException.Message);
            }
            
        }
    }
}

