using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSIconizer
{
    public partial class IconizerOptionsControl : UserControl
    {
        private IconizerOptionPage _options;

        public IconizerOptionsControl()
        {
            InitializeComponent();
        }

        internal void Initialize(IconizerOptionPage options)
        {
            _options = options;
            tHMargin.Value = options.HorizontalMargin.GetValueOrDefault(10);
            tVMargin.Value = options.VerticalMargin.GetValueOrDefault(5);
            cbShowText.Checked = options.ShowText.GetValueOrDefault(false);

            UpdateH();
            UpdateV();
            UpdateST();
        }

        private void tHMargin_ValueChanged(object sender, EventArgs e)
        {
            UpdateH();
        }

        private void tVMargin_ValueChanged(object sender, EventArgs e)
        {
            UpdateV();
        }

        private void cbShowText_CheckedChanged(object sender, EventArgs e)
        {
            UpdateST();
        }

        private void tHMargin_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateH();
        }

        private void tVMargin_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateV();
        }

        private void UpdateH() => _options.NewHorizontalMargin = (int)tHMargin.Value;
        private void UpdateV() => _options.NewVerticalMargin = (int)tVMargin.Value;
        private void UpdateST() => _options.NewShowText = cbShowText.Checked;
    }
}
