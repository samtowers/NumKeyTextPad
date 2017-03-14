using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumKeyTextPad
{
	class TextPad : RichTextBox
	{
		public TextPad()
		{
			this.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Yellow;
			this.Font = new Font("Tahoma", 24, FontStyle.Regular);
		}
	}
}
