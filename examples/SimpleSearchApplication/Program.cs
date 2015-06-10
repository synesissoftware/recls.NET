﻿
namespace SimpleSearchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Forms;

	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			System.Threading.Thread.CurrentThread.Name = "UI";

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}