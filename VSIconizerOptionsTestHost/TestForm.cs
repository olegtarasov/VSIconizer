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
            this.Repopulate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Repopulate();

            QuickAndHorribleCsvUnitTests();
            QuickAndHorribleCsvUnitTests2_RoundTrip();
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

/*

"Error List","#FFFF0000"
"Git Changes ""","#FFFF4500"
Properties,"#FFFFA500"
"Solution Explorer","#FF6495ED"

 */

        private static void QuickAndHorribleCsvUnitTests()
        {
            // Same as the comment above. "Git Changes" has 3 double-quotes after it, which is valid.
            // The empty lines should be ignored.
            const String csvInput = @"

""Error List"",""#FFFF0000""
""Git Changes """""",""#FFFF4500""
Properties,""#FFFFA500""
""Solution Explorer"",""#FF6495ED""


";

            var dict = TabColorsSerialization.ReadTabColorsCsv(csvInput);
            bool ok =
                dict.Count == 4 &&
                dict.ContainsKey("Error List") &&
                dict.ContainsKey("Git Changes \"") &&
                dict.ContainsKey("Properties") &&
                dict.ContainsKey("Solution Explorer");

            if( ok )
            {
                Boolean ok2 =
                    ( dict["Error List"].A == 0xFF && dict["Error List"].R == 0xFF && dict["Error List"].G == 0x00 );

                Boolean ok3 =
                    ( dict["Git Changes \""].A == 0xFF && dict["Git Changes \""].R == 0xFF && dict["Git Changes \""].G == 0x45 );

                if( !ok2 || !ok3 )
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            else
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private static void QuickAndHorribleCsvUnitTests2()
        {
            // Same as the comment above. "Git Changes" has 3 double-quotes after it, which is valid.
            // The empty lines should be ignored.
            const String csvInput = @"

""Error List"",""#FFFF0000""
""Git Changes """""""""",""#FFFF4500""
Properties,""#FFFFA500""
""Solution Explorer"",""#FF6495ED""


";

            var dict = TabColorsSerialization.ReadTabColorsCsv(csvInput);
            bool ok =
                dict.Count == 4 &&
                dict.ContainsKey("Error List") &&
                dict.ContainsKey("Git Changes \"\"") &&
                dict.ContainsKey("Properties") &&
                dict.ContainsKey("Solution Explorer");

            if( ok )
            {
                Boolean ok2 =
                    ( dict["Error List"].A == 0xFF && dict["Error List"].R == 0xFF && dict["Error List"].G == 0x00 );

                Boolean ok3 =
                    ( dict["Git Changes \"\""].A == 0xFF && dict["Git Changes \"\""].R == 0xFF && dict["Git Changes \"\""].G == 0x45 );

                if( !ok2 || !ok3 )
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            else
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private static void QuickAndHorribleCsvUnitTests2_RoundTrip()
        {
            const String csvInput = @"

""Error List"",""#FFFF0000""
""Git Changes """""",""#FFFF4500""
Properties,""#FFFFA500""
""Solution Explorer"",""#FF6495ED""


";

            var dict1 = TabColorsSerialization.ReadTabColorsCsv(csvInput);

            string toCsv1 = TabColorsSerialization.SerializeTabColorsToCsv(dict1);

            var dict2 = TabColorsSerialization.ReadTabColorsCsv(toCsv1);

            string toCsv2 = TabColorsSerialization.SerializeTabColorsToCsv(dict2);

            var dict3 = TabColorsSerialization.ReadTabColorsCsv(toCsv2);

            string toCsv3 = TabColorsSerialization.SerializeTabColorsToCsv(dict3);
            
            //

            bool ok = toCsv1.Equals(toCsv2) && toCsv2.Equals(toCsv3);
            if( !ok )
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}
