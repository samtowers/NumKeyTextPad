using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace NumKeyTextPad
{
	class TextPad : RichTextBox
	{
		public TextPad()
		{
			this.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Black;
			this.ForeColor = Color.LightGray;
			this.BorderStyle = BorderStyle.None;
			this.Font = new Font("Consolas", 20, FontStyle.Regular);

			this.KeyDown += delegate (object sender, KeyEventArgs e) 
			{
				if (e.Control && e.KeyCode == Keys.V)
				{
					((RichTextBox)sender).Paste(DataFormats.GetFormat("Text"));
					e.Handled = true;
				}
			};
		}
	}
}