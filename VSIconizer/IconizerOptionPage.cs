using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace VSIconizer
{
    [Guid("7758E0A8-93E9-4D2C-9553-8B5262604D88")]
    public class IconizerOptionPage : DialogPage
    {
        private readonly IconizerOptionsControl _control = new IconizerOptionsControl();

        public int? VerticalMargin { get; set; }
        public int? HorizontalMargin { get; set; }

        public int NewVerticalMargin { get; set; }
        public int NewHorizontalMargin { get; set; }

        public event EventHandler OptionsUpdated;

        protected override IWin32Window Window => _control;

        protected override void OnApply(PageApplyEventArgs e)
        {
            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                VerticalMargin = NewVerticalMargin;
                HorizontalMargin = NewHorizontalMargin;
                OnOptionsUpdated();
            }
            SaveSettingsToStorage();
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            _control.Initialize(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            NewHorizontalMargin = HorizontalMargin.Value;
            NewVerticalMargin = VerticalMargin.Value;
            SaveSettingsToStorage();
        }

        protected virtual void OnOptionsUpdated()
        {
            OptionsUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}