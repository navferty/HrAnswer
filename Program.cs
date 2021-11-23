using System;
using System.Diagnostics;
using System.IO;

namespace HrAnswer
{
	public static class Program
	{
		private const string TemplatePath = @"template\hr-answer.txt";

		static void Main(string[] args)
		{
			Console.WriteLine("Reply for hr will be placed to clipboard. Type HR name and press enter:");
			var name = Console.ReadLine();

			var file = File.ReadAllText(TemplatePath);
			var message = string.IsNullOrEmpty(name)
				? file.Replace(", %UserName%", string.Empty)
				: file.Replace("%UserName%", name);
			Clipboard.SetText(message);
		}
	}

	public static class Clipboard
	{
		public static void SetText(string text)
		{
			var powershell = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "powershell",
					Arguments = $"-command \"Set-Clipboard -Value \\\"{text}\\\"\""
				}
			};
			powershell.Start();
			powershell.WaitForExit();
		}

		public static string GetText()
		{
			var powershell = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					RedirectStandardOutput = true,
					FileName = "powershell",
					Arguments = "-command \"Get-Clipboard\""
				}
			};

			powershell.Start();
			string text = powershell.StandardOutput.ReadToEnd();
			powershell.StandardOutput.Close();
			powershell.WaitForExit();
			return text.TrimEnd();
		}
	}
}
