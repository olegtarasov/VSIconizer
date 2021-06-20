//#define USE_DATA_GRID_VIEW // Disabled because when using the DataGridView and editing the color-text cell this causes a stack-overflow/infinite-loop in Visual Studio's Win32 Message Pump for reasons I don't understand.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VSIconizer.Core;

namespace VSIconizer
{
    using GdiColor  = System.Drawing.Color;
    using WpfColor  = System.Windows.Media.Color;
    using ColorDict = IReadOnlyDictionary<string,System.Windows.Media.Color>; // Key = Tool Tab Text, Value = Color

    public partial class IconizerOptionsControl : UserControl
    {
        private IIconizerOptionPage parentPage;

#if USE_DATA_GRID_VIEW
        private readonly BindingList<TabColorEditorRow> tabColorBindingList = new BindingList<TabColorEditorRow>();
        private readonly BindingSource tabColorBindingSource;
#endif

        private readonly TextBox tabColorsCsvTxt = new TextBox();

        private const int _horizontalMarginRowIdx = 1;
        private const int _verticalMarginRowIdx   = 2;
        private const int _iconTextSpacingRowIdx  = 3;
        private const int _rotateIconsRowIdx      = 4;
        private const int _tabColorsCheckRowIdx   = 5;
        private const int _tabColorsEditorRowIdx  = 6;
        
        private static readonly float _layoutRowHeight = 27F;

        public IconizerOptionsControl()
        {
            this.InitializeComponent();

#if !USE_DATA_GRID_VIEW // HACK: Quick-and-dirty replacement for now:
            Size tabColorsEditorSize = this.tabColorsEditor.Size;
            Control tabColorsEditorParent = this.tabColorsEditor.Parent;
            tabColorsEditorParent.Controls.Remove(this.tabColorsEditor);
            tabColorsEditorParent.Controls.Add(this.tabColorsCsvTxt);

            this.tabColorsCsvTxt.Multiline = true;
            this.tabColorsCsvTxt.AcceptsReturn = true;
            this.tabColorsCsvTxt.TabIndex = this.tabColorsEditor.TabIndex;
            this.tabColorsCsvTxt.Font = new Font(family: FontFamily.GenericMonospace, emSize: this.Font.Size);
            this.tabColorsCsvTxt.Size = tabColorsEditorSize;
            this.tabColorsCsvTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tabColorsCsvTxt.KeyPress += this.TabColorsCsvTxt_KeyPress;
            this.layout.SetCellPosition(this.tabColorsCsvTxt, new TableLayoutPanelCellPosition(column: 1, row: _tabColorsEditorRowIdx));
#endif

            this.suppressUserChange = true;
            try
            {
#if USE_DATA_GRID_VIEW
                // Tab Colors Editor:
                {
                    this.tabColorBindingSource = new BindingSource(dataSource: this.tabColorBindingList, dataMember: null);

                    this.tabColorsEditor.AutoGenerateColumns = false; // <-- Set this *BEFORE* `.DataSource`, derp.
                    this.tabColorsEditor.DataSource = this.tabColorBindingSource;

                    this.tabColorsEditor.CellContentClick += this.TabColorsEditor_CellContentClick;
                    this.tabColorsEditor.CellFormatting   += this.TabColorsEditor_CellFormatting;
                    this.tabColorsEditor.CellValueChanged += this.OnUserChange; // > "If the value is successfully committed, the CellValueChanged event occurs."

                    this.colValid     .DataPropertyName = nameof(TabColorEditorRow.IsValid);
                    this.colTabText   .DataPropertyName = nameof(TabColorEditorRow.TabText);
                    this.colColorValue.DataPropertyName = nameof(TabColorEditorRow.ColorText);
                    this.colColorBtn  .DataPropertyName = nameof(TabColorEditorRow.WpfColor);
                }
#endif

                this.modeCmb.ValueMember   = nameof(VSIconizerModeComboBoxItem.Value);
                this.modeCmb.DisplayMember = nameof(VSIconizerModeComboBoxItem.DisplayText);
                this.modeCmb.DataSource    = VSIconizerModeComboBoxItem.Items;

                this.modeCmb        .SelectedValueChanged += this.OnUserChange;
                this.tHMargin       .ValueChanged         += this.OnUserChange;
                this.tVMargin       .ValueChanged         += this.OnUserChange;
                this.iconTextSpacing.ValueChanged         += this.OnUserChange;
                this.rotateChk      .CheckedChanged       += this.OnUserChange;
                this.useTabColorsChk.CheckedChanged       += this.OnUserChange;

                // Trigger initial appearance:
                UpdateTableLayout(this.layout, forMode: this.SelectedMode, useTabColors: this.useTabColorsChk.Checked);
            }
            finally
            {
                this.suppressUserChange = false;
            }
        }

#if !USE_DATA_GRID_VIEW
        private void TabColorsCsvTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Return)
            {
                this.OnUserChange(sender, e);
            }
        }
#endif

        private VSIconizerMode SelectedMode => (VSIconizerMode)this.modeCmb.SelectedValue;

        public void Initialize(IIconizerOptionPage parentPage, VSIconizerConfiguration configuration)
        {
            if (parentPage is null) throw new ArgumentNullException(nameof(parentPage));

            if (this.parentPage != null)
            {
                if(!Object.ReferenceEquals(this.parentPage, parentPage))
                {
                    throw new InvalidOperationException("Already initialized.");
                }
            }

            this.parentPage = parentPage;

            this.PopulateControlsFromConfiguration(configuration);
        }

        public bool IsInitialized => this.parentPage != null;

        public void PopulateControlsFromConfiguration(VSIconizerConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            this.suppressUserChange = true;
            try
            {
                this.modeCmb.SelectedValue = configuration.Mode;

                this.tHMargin       .Value   = new Decimal(configuration.HorizontalSpacing);
                this.tVMargin       .Value   = new Decimal(configuration.VerticalSpacing);
                this.iconTextSpacing.Value   = new Decimal(configuration.IconTextSpacing);
                this.rotateChk      .Checked = configuration.RotateVerticalTabIcons;
                this.useTabColorsChk.Checked = configuration.UseTabColors;

                UpdateTableLayout(this.layout, configuration.Mode, useTabColors: this.useTabColorsChk.Checked);
                this.PopulateTabColorsGrid(configuration.TabColors);
            }
            finally
            {
                this.suppressUserChange = false;
            }
        }

        #region Tab Colors

        private void PopulateTabColorsGrid(ColorDict colors)
        {
#if USE_DATA_GRID_VIEW
            this.tabColorBindingList.Clear();

            foreach(string tabText in colors.Keys.OrderBy(k => k))
            {
                WpfColor wpfColor = colors[tabText];

                TabColorEditorRow row = TabColorEditorRow.FromWpfColor(tabText, wpfColor);
                this.tabColorBindingList.Add(row);
            }
#else
            this.tabColorsCsvTxt.Text = TabColorsSerialization.SerializeTabColorsToCsv(colors);
#endif
        }

        private ColorDict GetTabColorsDict()
        {
#if USE_DATA_GRID_VIEW
            if (this.tabColorBindingList.Count == 0)
            {
                return VSIconizerConfiguration.EmptyTabColors;
            }

            return this.tabColorBindingList
                .Where(row => row.IsValid)
                .Select(row => new KeyValuePair<string,WpfColor>(row.TabText,row.WpfColor))
                .GroupBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase) // Dedupe:
                .Select(grp => grp.First())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
#else
            return TabColorsSerialization.ReadTabColorsCsv(this.tabColorsCsvTxt.Text);
#endif
        }

#if USE_DATA_GRID_VIEW
        // https://stackoverflow.com/questions/3577297/how-to-handle-click-event-in-button-column-in-datagridview
        private void TabColorsEditor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == this.colColorBtn.Index && e.RowIndex >= 0)
            {
                DialogResult result = this.colorDialog.ShowDialog(owner: this);
                if (result == DialogResult.OK)
                {
                    GdiColor gdiColor = this.colorDialog.Color;

                    TabColorEditorRow row = this.tabColorBindingList[e.RowIndex]; // <-- Is this right?
                    row.WpfColor = gdiColor.ToWpfColor();
                }
            }
        }
#endif

        private void TabColorsEditor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != this.colColorBtn.Index) return;

            // https://stackoverflow.com/questions/338935/binding-to-datagridview-is-there-a-way-to-bind-the-background-color-of-a-cel
            DataGridViewRow row = this.tabColorsEditor.Rows[e.RowIndex];
            
            if (row.DataBoundItem is TabColorEditorRow data)
            {
                DataGridViewButtonCell cell = (DataGridViewButtonCell)row.Cells[e.ColumnIndex];

                if (data.IsValid)
                {
                    cell.FlatStyle = FlatStyle.Flat;
                    cell.Style.BackColor = data.GdiColor;
                }
                else
                {
                    cell.FlatStyle = FlatStyle.System;
                    cell.Style.BackColor = GdiColor.Empty;
                }
            }
        }

        #endregion

        /// <summary>Creates a new <see cref="VSIconizerConfiguration"/> from the WinForms controls.</summary>
        public VSIconizerConfiguration GetVSIconizerConfiguration()
        {
            return new VSIconizerConfiguration(
                mode                  : (VSIconizerMode)this.modeCmb.SelectedValue,
                horizontalSpacing     : (double)this.tHMargin       .Value,
                verticalSpacing       : (double)this.tVMargin       .Value,
                iconTextSpacing       : (double)this.iconTextSpacing.Value,
                rotateVerticalTabIcons: this.rotateChk.Checked,
                useTabColors          : this.useTabColorsChk.Checked,
                tabColors             : this.GetTabColorsDict()
            );
        }

        private bool suppressUserChange;

        private void OnUserChange(object _, EventArgs e)
        {
            if (this.suppressUserChange) return;

            UpdateTableLayout(this.layout, forMode: this.SelectedMode, useTabColors: this.useTabColorsChk.Checked);

            VSIconizerConfiguration newCfg = this.GetVSIconizerConfiguration();
            this.parentPage.Apply(newCfg);
        }

        private static void UpdateTableLayout(TableLayoutPanel layout, VSIconizerMode forMode, bool useTabColors) // This method is static so we don't unintentionally access other controls directly.
        {
            if (forMode == VSIconizerMode.Default)
            {
                // Hide everything except the Mode selector:
                for (int i = _horizontalMarginRowIdx; i < layout.RowCount; i++)
                {
                    layout.RowStyles[i].SizeType = SizeType.Absolute;
                    layout.RowStyles[i].Height   = 0;
                }
            }
            else
            {
                // Visibility is based on Mode and other settings:
                layout.RowStyles[ _horizontalMarginRowIdx ].Height = _layoutRowHeight;
                layout.RowStyles[ _verticalMarginRowIdx   ].Height = _layoutRowHeight;

                layout.RowStyles[ _iconTextSpacingRowIdx  ].Height = (forMode == VSIconizerMode.IconAndText ) ? _layoutRowHeight : 0;
                layout.RowStyles[ _rotateIconsRowIdx      ].Height = forMode.ShowIcon() ? _layoutRowHeight : 0;
                layout.RowStyles[ _tabColorsCheckRowIdx   ].Height = _layoutRowHeight;

                if(useTabColors)
                {
                    layout.RowStyles[ _tabColorsEditorRowIdx  ].SizeType = SizeType.AutoSize;
                }
                else
                {
                    layout.RowStyles[ _tabColorsEditorRowIdx  ].SizeType = SizeType.Absolute;
                    layout.RowStyles[ _tabColorsEditorRowIdx  ].Height = 0;
                }
            }

            // Show/hide controls based on their rows heights:
            foreach( Control ctrl in layout.Controls)
            {
                var pos = layout.GetPositionFromControl(ctrl);
                var row = layout.RowStyles[pos.Row];
                ctrl.Visible = row.Height != 0 || row.SizeType == SizeType.AutoSize;
            }
        }
    }
}
