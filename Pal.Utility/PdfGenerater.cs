using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Utility
{
	public class PdfGenerater
	{
		public void ExecuteCommandSync(object fileName, object command)
		{
			//// create the ProcessStartInfo using "cmd" as the program to be run,
			//// and "/c " as the parameters.
			//// Incidentally, /c tells cmd that we want it to execute the command that follows,
			//// and then exit.

			// Now we create a process, assign its ProcessStartInfo and start it
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			try
			{
				proc.StartInfo.FileName = fileName.ToString();
				proc.StartInfo.Arguments = command.ToString();

				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.UseShellExecute = false;
				// Do not create the black window.
				proc.StartInfo.CreateNoWindow = true;
				// Now start process
				proc.Start();
				// Get the output into a string
				string result = proc.StandardOutput.ReadToEnd();
				proc.WaitForExit();
				// Display the command output.
				Console.WriteLine(result);
			}
			catch (Exception objException)
			{
				// Log the exception
				string strTemp = "Error : while Executing Command [Sync Admin Screen] : " + DateTime.Now + "\n";
				//CommonMethods.write2File(strTemp + objException.Message, ".\\ErrorLog.txt");
			}
			finally
			{
				proc.Close();
			}
		}
	}
}
