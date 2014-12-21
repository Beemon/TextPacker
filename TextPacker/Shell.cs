using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
namespace TextPacker
{
	public class Shell : Form
	{
		private IntPtr thisWindow;
		private IntPtr nextWindow;
		private SoundPlayer sound;
		private IContainer components = null;
		private CheckBox chkEnabled;
		public Shell()
		{
			this.InitializeComponent();
		}
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Shell());
		}
		private void Shell_Load(object sender, EventArgs e)
		{
			this.sound = new SoundPlayer("C:\\Windows\\Media\\recycle.wav");
			this.nextWindow = Win32.SetClipboardViewer(this.thisWindow = base.Handle);
		}
		private void Shell_FormClosed(object sender, FormClosedEventArgs e)
		{
			Win32.ChangeClipboardChain(this.thisWindow, this.nextWindow);
		}
		protected override void WndProc(ref Message message)
		{
			base.WndProc(ref message);
			int msg = message.Msg;
			if (msg != 776)
			{
				if (msg != 781)
				{
					return;
				}
				if (message.WParam == this.nextWindow)
				{
					this.nextWindow = message.LParam;
					return;
				}
				Win32.SendMessage(this.nextWindow, message.Msg, message.WParam, message.LParam);
			}
			else
			{
				if (this.chkEnabled.Checked)
				{
					IDataObject dataObject = Clipboard.GetDataObject();
					if (dataObject.GetDataPresent(DataFormats.Text))
					{
						Clipboard.SetText(this.Pack((string)dataObject.GetData(DataFormats.Text)));
					}
					this.sound.Play();
					return;
				}
			}
		}
		protected string Pack(string text)
		{
			bool flag = false;
			char c = '\0';
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c2 = text[i];
				if (c2 != '\r')
				{
					if (c2 == '\t' || c2 == '\n' || c2 == ' ')
					{
						flag = true;
					}
					else
					{
						if (flag)
						{
							flag = false;
							bool flag2 = c == '_' || char.IsLetterOrDigit(c);
							bool flag3 = c2 == '_' || char.IsLetterOrDigit(c2);
							if (flag2 || flag3)
							{
								stringBuilder.Append(' ');
							}
						}
						stringBuilder.Append(c = c2);
					}
				}
			}
			return stringBuilder.ToString();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.chkEnabled = new CheckBox();
			base.SuspendLayout();
			this.chkEnabled.AutoSize = true;
			this.chkEnabled.Checked = true;
			this.chkEnabled.CheckState = CheckState.Checked;
			this.chkEnabled.Location = new Point(30, 7);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new Size(71, 17);
			this.chkEnabled.TabIndex = 0;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(7f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(131, 31);
			base.Controls.Add(this.chkEnabled);
			this.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Shell";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "TextPacker";
			base.TopMost = true;
			base.FormClosed += new FormClosedEventHandler(this.Shell_FormClosed);
			base.Load += new EventHandler(this.Shell_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
