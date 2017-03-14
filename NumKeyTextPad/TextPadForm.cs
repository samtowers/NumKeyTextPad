using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumKeyTextPad
{
	public partial class TextPadForm : Form
	{
		TextPad textPad = new TextPad();

		public TextPadForm()
		{
			InitializeComponent();
			SetupForm();
		}

		private void SetupForm()
		{
			/** Setup Form **/
			this.FormBorderStyle = FormBorderStyle.None;
			this.Opacity = 0.7;
			this.BackColor = Color.Yellow;

			/** Setup TextPad textbox **/
			// Update the textPad size with the form size.
			this.SizeChanged += delegate (object sender, EventArgs args) {
				textPad.Size = this.Size;
			};
			this.Controls.Add(textPad);
		}
	}
}
