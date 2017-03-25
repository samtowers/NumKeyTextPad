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
			//ListenForKeyInput();
		}
		
		private void SetupForm()
		{
			/** Setup Form **/
			this.FormBorderStyle = FormBorderStyle.None;
			this.Opacity = 0.7;
			this.BackColor = Color.Yellow;
			this.ShowInTaskbar = false;
			this.SetMinimized();
			
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

		private void SetMinimized(bool minimized = true)
		{
			this.WindowState = minimized ? FormWindowState.Minimized : FormWindowState.Normal;
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
		}

		private ContextMenu makeNotifyIconContextMenu()
		{
			ContextMenu cm = new ContextMenu();
			MenuItem exitMenuItem = new MenuItem("Exit");
			exitMenuItem.Click += delegate (object sender, EventArgs args) {
				Application.Exit();
			};
			cm.MenuItems.Add(exitMenuItem);
			return cm;
		}
	}
}
