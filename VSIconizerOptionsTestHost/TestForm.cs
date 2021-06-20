using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using VSIconizer;
using VSIconizer.Core;

namespace VSIconizerOptionsTestHost
{
    public partial class TestForm : Form, IIconizerOptionPage
	{
//        private static readonly String _testCsv =
//@"""Solution Explorer"",""CornflowerBlue""
//""Error List"",""Red""";

        private static readonly String _testCsv =
@"""Solution Explorer"",""#6495ed""
""Error List"",""#ff0000""";

		public TestForm()
		{
			this.InitializeComponent();

            this.tabColorsCsv.Font = new Font( family: FontFamily.GenericMonospace, emSize: this.tabColorsCsv.Font.Size );
            this.tabColorsCsv.Text = _testCsv;

            this.reloadBtn.Click += this.ReloadBtn_Click;
		}

        private void ReloadBtn_Click(object sender, EventArgs e)
        {
            /*

"Error List","#FFFF0000"
"Git Changes ""","#FFFF4500"
Properties,"#FFFFA500"
"Solution Explorer","#FF6495ED"


            */

            this.Repopulate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Repopulate();
        }

        void IIconizerOptionPage.Apply(VSIconizerConfiguration newConfiguration)
        {
            _ = this.applyList.Items.Add( DateTime.Now );

            this.tabColorsCsv.Text = TabColorsSerialization.SerializeTabColorsToCsv(newConfiguration.TabColors);
        }

        private void Repopulate()
        {
            VSIconizerConfiguration cfg = new VSIconizerConfiguration(
                mode                  : VSIconizerMode.IconAndText,
                horizontalSpacing     : 10,
                verticalSpacing       : 5,
                iconTextSpacing       : 3,
                rotateVerticalTabIcons: true,
                useTabColors          : true,
                tabColors             : TabColorsSerialization.ReadTabColorsCsv(this.tabColorsCsv.Text)
            );

            if (this.iconizerOptionsControl1.IsInitialized)
            {
                this.iconizerOptionsControl1.PopulateControlsFromConfiguration(cfg);
            }
            else
            {
                this.iconizerOptionsControl1.Initialize(parentPage: this, cfg);
            }
        }
    }
}
