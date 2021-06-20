using System;
using System.Windows.Forms;
using System.Reflection;

using VSIconizer.Core;

namespace VSIconizer
{
    public partial class IconizerOptionsControl : UserControl
    {
        private IconizerOptionPage parentPage;

        public IconizerOptionsControl()
        {
            this.InitializeComponent();

            this.modeCmb.ValueMember   = nameof(VSIconizerModeComboBoxItem.Value);
            this.modeCmb.DisplayMember = nameof(VSIconizerModeComboBoxItem.DisplayText);
            this.modeCmb.DataSource    = VSIconizerModeComboBoxItem.Items;

            this.modeCmb        .SelectedValueChanged += this.ModeCmb_SelectedValueChanged;
            this.tHMargin       .ValueChanged         += this.OnUserChange;
            this.tVMargin       .ValueChanged         += this.OnUserChange;
            this.iconTextSpacing.ValueChanged         += this.OnUserChange;
            this.rotateChk      .CheckedChanged       += this.OnUserChange;
        }

        private void ModeCmb_SelectedValueChanged(object sender, EventArgs e)
        {
            VSIconizerMode currentMode = (VSIconizerMode)this.modeCmb.SelectedValue;

            this.lblHMargin        .Visible = (currentMode != VSIconizerMode.Default);
            this.tHMargin          .Visible = (currentMode != VSIconizerMode.Default);
                                   
            this.lblVMargin        .Visible = (currentMode != VSIconizerMode.Default);
            this.tVMargin          .Visible = (currentMode != VSIconizerMode.Default);

            this.iconTextSpacingLbl.Visible = (currentMode == VSIconizerMode.IconAndText);
            this.iconTextSpacing   .Visible = (currentMode == VSIconizerMode.IconAndText);

            this.layout            .RowStyles[3].Height = (currentMode == VSIconizerMode.IconAndText) ? 27F : 0;

            this.rotateChk         .Visible = (currentMode.ShowIcon());

            //

            this.OnUserChange(sender, e);
        }

        public void Initialize(IconizerOptionPage parentPage, VSIconizerConfiguration configuration)
        {
            this.parentPage = parentPage ?? throw new ArgumentNullException(nameof(parentPage));

            this.PopulateControlsFromConfiguration(configuration);
        }

        private void PopulateControlsFromConfiguration(VSIconizerConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            this.suppressUserChange = true;
            try
            {
                this.modeCmb.SelectedValue = configuration.Mode;

                this.tHMargin       .Value = new Decimal(configuration.HorizontalSpacing);
                this.tVMargin       .Value = new Decimal(configuration.VerticalSpacing);
                this.iconTextSpacing.Value = new Decimal(configuration.IconTextSpacing);
                this.rotateChk    .Checked = configuration.RotateVerticalTabIcons;
            }
            finally
            {
                this.suppressUserChange = false;
            }
        }

        /// <summary>Creates a new <see cref="VSIconizerConfiguration"/> from the WinForms controls.</summary>
        public VSIconizerConfiguration GetVSIconizerConfiguration()
        {
            return new VSIconizerConfiguration(
                mode                  : (VSIconizerMode)this.modeCmb.SelectedValue,
                horizontalSpacing     : (double)this.tHMargin       .Value,
                verticalSpacing       : (double)this.tVMargin       .Value,
                iconTextSpacing       : (double)this.iconTextSpacing.Value,
                rotateVerticalTabIcons: this.rotateChk.Checked
            );
        }

        private bool suppressUserChange;

        private void OnUserChange(object _, EventArgs e)
        {
            if (this.suppressUserChange) return;

            VSIconizerConfiguration newCfg = this.GetVSIconizerConfiguration();
            this.parentPage.Apply(newCfg);
        }
    }
}
