using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Gma.UserActivityMonitor;

namespace NumKeyTextPad
{
	public partial class TextPadForm : Form
	{
		TextPad textPad;
		NotifyIcon notifyIcon;
		public event	Action FormHide;
		string logFile;
		int prevLogHashCode = 0;

		public TextPadForm()
		{
			// Init the VS designer features
			InitializeComponent();
			// Initialize the TextPadForms component properties (declared in the designer)
			this.components = new System.ComponentModel.Container();
			SetupForm();
			SyncToLogFile();
			ListenForKeyInput();
		}
		
		private void SetupForm()
		{
			/** Setup Form **/
			this.FormBorderStyle = FormBorderStyle.None;
			//this.Opacity = 0.7;
			this.BackColor = Color.Yellow;
			this.ShowInTaskbar = false;
			this.SetWindowState(FormWindowState.Minimized);
			this.TopMost = true;

			/** Setup TextPad **/
			textPad = new TextPad();
			// Update the textPad size with the form size.
			this.SizeChanged += delegate (object sender, EventArgs args) {
				textPad.Size = this.Size;
			};
			this.Controls.Add(textPad);
		
			/** Setup NotifyIcon **/
			setupNotifyIcon();
		}

		/**
		 * Keep the text on the form automatically synced to a file. 
		 * If no file has previously been set or the file has otherwise been moved, then
		 * launch a file select dialog.
		 * If no
		 */
		private void SyncToLogFile()
		{
			this.logFile = FetchLogFile();

			if (!string.IsNullOrWhiteSpace(logFile))
			{
				// Update Textpad
				textPad.Text = File.ReadAllText(this.logFile);
				prevLogHashCode = textPad.Text.GetHashCode();
				
				// Log file changes are commited when the dialog is hidden or the form is closed.
				this.FormHide += UpdateLogFile;
				this.FormClosing += delegate (object sender, FormClosingEventArgs args) { UpdateLogFile(); };
			}
		}

		// Save to the current log file.
		// This method assumes a valid log file has been set.
		private void UpdateLogFile()
		{
			// Only update on text changes.
			if (prevLogHashCode != textPad.Text.GetHashCode())
			{
				File.WriteAllText(this.logFile, textPad.Text);
				prevLogHashCode = textPad.Text.GetHashCode();
			}
		}

		// If a log file was previously set, grab and check if it exists. If no log file
		// was previously set or the stored log file does not exist, the ask the user for
		// a new one.
		private string FetchLogFile()
		{
			string file = (string) Properties.Settings.Default["LogFile"];
			if (!File.Exists(file))
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.RestoreDirectory = true;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					file = dialog.FileName;
					Properties.Settings.Default["LogFile"] = file;
					Properties.Settings.Default.Save();
				}
				else
				{
					file = null;
				}
			}

			return file;
		}

		private void setupNotifyIcon()
		{
			/** Presentation of the Icon **/
			notifyIcon = new NotifyIcon(this.components);
			notifyIcon.Icon = SystemIcons.Information;
			notifyIcon.Visible = true;
			notifyIcon.Text = "NumKeyTextPad";

			/** Context Menu **/
			notifyIcon.ContextMenu = makeNotifyIconContextMenu();

			/** Double Click to show form **/
			notifyIcon.DoubleClick += delegate (object sender, EventArgs args)
			{
				this.SetWindowState(FormWindowState.Maximized);
			};
		}

		private ContextMenu makeNotifyIconContextMenu()
		{
			ContextMenu cm = new ContextMenu();

			/** Show Button **/
			MenuItem showMenuItem = new MenuItem("Show");
			showMenuItem.Click += delegate (object sender, EventArgs args) {
				this.SetWindowState(FormWindowState.Maximized);
			};
			cm.MenuItems.Add(showMenuItem);

			/** Hide Button **/
			MenuItem hideMenuItem = new MenuItem("Hide");
			hideMenuItem.Click += delegate (object sender, EventArgs args) {
				this.SetWindowState(FormWindowState.Minimized);
			};
			cm.MenuItems.Add(hideMenuItem);
			
			/** Exit Button **/
			MenuItem exitMenuItem = new MenuItem("Exit");
			exitMenuItem.Click += delegate (object sender, EventArgs args) {
				Application.Exit();
			};
			cm.MenuItems.Add(exitMenuItem);

			return cm;
		}
		
		/**
		 * Listen for the Numpad keyboard events, which will dicate the visibly and positioning 
		 * of the textpad.
		 */
		private void ListenForKeyInput()
		{
			HookManager.KeyDown += HandleKeyPress;
		}

		private void HandleKeyPress(object sender, KeyEventArgs args)
		{
			if (args.KeyCode == Keys.NumPad5)
			{
				args.Handled = true;
				SetWindowState(); 

				if (this.WindowState == FormWindowState.Minimized) FormHide();
			}
		}

		/**
		 * Set the form window state or toggle between Maximized and Minimized.
		 */
		private void SetWindowState(FormWindowState? forceState = null)
		{
			if (forceState.HasValue)
			{
				this.WindowState = forceState.Value;
			}
			else
			{
				this.WindowState = this.WindowState == FormWindowState.Maximized ? FormWindowState.Minimized : FormWindowState.Maximized;
			}
		}

		/** 
		 * This override will supress the window from appearing the ALT-TAB dialog.
		 */
		protected override CreateParams CreateParams
		{
			get
			{
				var Params = base.CreateParams;
				Params.ExStyle |= 0x80;
				return Params;
			}
		}
	}
}
