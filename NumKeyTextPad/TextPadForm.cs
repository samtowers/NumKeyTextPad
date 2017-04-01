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
using Gma.UserActivityMonitor;

namespace NumKeyTextPad
{
	public partial class TextPadForm : Form
	{
		TextPad textPad;
		NotifyIcon notifyIcon;
		
		public TextPadForm()
		{
			// Init the VS designer features
			InitializeComponent();
			// Initialize the TextPadForms component properties (declared in the designer)
			this.components = new System.ComponentModel.Container();
			SetupForm();
			ListenForKeyInput();
		}
		
		private void SetupForm()
		{
			/** Setup Form **/
			this.FormBorderStyle = FormBorderStyle.None;
			this.Opacity = 0.7;
			this.BackColor = Color.Yellow;
			this.ShowInTaskbar = false;
			this.SetWindowState(FormWindowState.Minimized);

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
	}
}
