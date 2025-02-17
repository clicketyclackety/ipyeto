using System;
using System.IO;

using Eto.Forms;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

// Notes : https://putridparrot.com/blog/hosting-ironpython-in-a-c-application/

namespace ipyeto;

class Program
{

	private static ScriptEngine Engine { get; set; }
	private static ScriptScope Scope { get; set; }

	[STAThread]
	static void Main(string[] args)
	{
		Engine = Python.CreateEngine();
		Scope = Engine.CreateScope();

		Engine.Execute(@"
import clr
clr.AddReference('Eto')
# clr.AddReference('Eto.Wpf') # May or may not be necessary
		", Scope);

		var app = new Application(Eto.Platform.Detect);
		var form = new MainForm();

		form.PreLoad += RunPython;
		
		Scope.SetVariable("__form__", form);
		
		app.Run(form);
	}

	private static void RunPython(object sender, EventArgs e)
	{
		Engine.Execute(@$"
from Eto.Forms import Button

control = Button();
control.Text = 'hello!'
__form__.Content = control
			", Scope);	
	}

}
