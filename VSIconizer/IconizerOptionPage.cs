using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

using VSIconizer.Core;

namespace VSIconizer
{
    [Guid("7758E0A8-93E9-4D2C-9553-8B5262604D88")]
    public class IconizerOptionPage : DialogPage
    {
        private readonly IconizerOptionsControl control = new IconizerOptionsControl();

        #region VS-managed Options properties
        // These properties are loaded/saved by VS.
        // See https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-options-page?view=vs-2019

        public string Mode              { get; set; }
        public double HorizontalSpacing { get; set; }
        public double VerticalSpacing   { get; set; }
        public double IconTextSpacing   { get; set; }
        public bool   RotateIcons       { get; set; }

        #endregion

        protected override IWin32Window Window => this.control;

        private VSIconizerConfiguration newestConfiguration;

        public void Apply(VSIconizerConfiguration newConfiguration)
        {
            this.newestConfiguration = newConfiguration ?? throw new ArgumentNullException(nameof(newConfiguration));

            this.OnApply(new PageApplyEventArgs{ ApplyBehavior = ApplyKind.Apply });
        }

        /// <summary>Saves current settings from <see cref="control"/> to the VS profile/registry.</summary>
        protected override void OnApply(PageApplyEventArgs e)
        {
            if (this.newestConfiguration is null)
            {
                return;
            }

            if (e.ApplyBehavior == ApplyKind.Apply)
            {
//              VSIconizerConfiguration desired = this.control.ToVSIconizerConfiguration();
                VSIconizerConfiguration desired = this.newestConfiguration;

                this.Mode              = desired.Mode.ToString();
                this.HorizontalSpacing = desired.HorizontalSpacing;
                this.VerticalSpacing   = desired.VerticalSpacing;
                this.IconTextSpacing   = desired.IconTextSpacing;
                this.RotateIcons       = desired.RotateVerticalTabIcons;

                this.OnOptionsUpdated(desired);
            }

            this.SaveSettingsToStorage();
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            VSIconizerConfiguration current = this.ToVSIconizerConfiguration();

            this.control.Initialize(this, current);
        }

        protected override void OnClosed(EventArgs e)
        {
            this.SaveSettingsToStorage();
        }

        public event EventHandler<OptionsUpdatedEventArgs> OptionsUpdated;

        protected virtual void OnOptionsUpdated(VSIconizerConfiguration newCfg)
        {
            OptionsUpdatedEventArgs e = new OptionsUpdatedEventArgs(newCfg);
            this.OptionsUpdated?.Invoke(this, e);
        }

        /// <summary>Creates a new <see cref="VSIconizerConfiguration"/> from the VS Options properties in this <see cref="IconizerOptionPage"/>.</summary>
        public VSIconizerConfiguration ToVSIconizerConfiguration()
        {
            bool isDefault = (
                String.IsNullOrWhiteSpace(this.Mode) &&
                this.HorizontalSpacing == default    &&
                this.VerticalSpacing   == default    &&
                this.IconTextSpacing   == default    &&
                this.RotateIcons       == default
            );

            if (isDefault)
            {
                return VSIconizerConfiguration.Default;
            }
            else
            {
                return new VSIconizerConfiguration(
                    mode                  : Enum.TryParse(this.Mode, ignoreCase: true, out VSIconizerMode mode) ? mode : VSIconizerMode.IconOnly,
                    horizontalSpacing     : this.HorizontalSpacing,
                    verticalSpacing       : this.VerticalSpacing,
                    iconTextSpacing       : this.IconTextSpacing,
                    rotateVerticalTabIcons: this.RotateIcons
                );
            }
        }
    }

    public class OptionsUpdatedEventArgs : EventArgs
    {
        public OptionsUpdatedEventArgs(VSIconizerConfiguration newCfg)
        {
            this.Configuration = newCfg ?? throw new ArgumentNullException(nameof(newCfg));
        }

        public VSIconizerConfiguration Configuration { get; }
    }
}