namespace Cosmoser.PingAnMeetingRequest.Outlook2007
{
    partial class PingAnMeetingRequestFormRegion : Microsoft.Office.Tools.Outlook.ImportedFormRegionBase
    {
        private Microsoft.Office.Interop.Outlook._DDocSiteControl _DocSiteControl1;
        private Microsoft.Office.Interop.Outlook.OlkFrameHeader olkFrameHeader;
        private Microsoft.Office.Interop.Outlook.OlkTextBox olkTextBox1;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel1;
        private Microsoft.Office.Interop.Outlook.OlkTextBox olkTextBox2;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel2;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel3;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel4;
        private Microsoft.Office.Interop.Outlook.OlkCheckBox olkCheckBox1;
        private Microsoft.Office.Interop.Outlook.OlkTimeControl olkStartTimeControl;
        private Microsoft.Office.Interop.Outlook.OlkDateControl olkStartDateControl;
        private Microsoft.Office.Interop.Outlook.OlkTimeControl olkEndTimeControl;
        private Microsoft.Office.Interop.Outlook.OlkDateControl olkEndDateControl;
        private Microsoft.Office.Interop.Outlook.OlkInfoBar olkInfoBar1;

        public PingAnMeetingRequestFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.FormRegionShowing += new System.EventHandler(this.PingAnMeetingRequestFormRegion_FormRegionShowing);
            this.FormRegionClosed += new System.EventHandler(this.PingAnMeetingRequestFormRegion_FormRegionClosed);
        }

        protected override void InitializeControls()
        {
            this._DocSiteControl1 = (Microsoft.Office.Interop.Outlook._DDocSiteControl)GetFormRegionControl("_DocSiteControl1");
            this.olkFrameHeader = (Microsoft.Office.Interop.Outlook.OlkFrameHeader)GetFormRegionControl("OlkFrameHeader");
            this.olkTextBox1 = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("OlkTextBox1");
            this.olkLabel1 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel1");
            this.olkTextBox2 = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("OlkTextBox2");
            this.olkLabel2 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel2");
            this.olkLabel3 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel3");
            this.olkLabel4 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel4");
            this.olkCheckBox1 = (Microsoft.Office.Interop.Outlook.OlkCheckBox)GetFormRegionControl("OlkCheckBox1");
            this.olkStartTimeControl = (Microsoft.Office.Interop.Outlook.OlkTimeControl)GetFormRegionControl("OlkStartTimeControl");
            this.olkStartDateControl = (Microsoft.Office.Interop.Outlook.OlkDateControl)GetFormRegionControl("OlkStartDateControl");
            this.olkEndTimeControl = (Microsoft.Office.Interop.Outlook.OlkTimeControl)GetFormRegionControl("OlkEndTimeControl");
            this.olkEndDateControl = (Microsoft.Office.Interop.Outlook.OlkDateControl)GetFormRegionControl("OlkEndDateControl");
            this.olkInfoBar1 = (Microsoft.Office.Interop.Outlook.OlkInfoBar)GetFormRegionControl("OlkInfoBar1");

        }

        public partial class PingAnMeetingRequestFormRegionFactory : Microsoft.Office.Tools.Outlook.IFormRegionFactory
        {
            public event Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler FormRegionInitializing;

            private Microsoft.Office.Tools.Outlook.FormRegionManifest _Manifest;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PingAnMeetingRequestFormRegionFactory()
            {
                this._Manifest = Globals.Factory.CreateFormRegionManifest();
                this.InitializeManifest();
                this.FormRegionInitializing += new Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler(this.PingAnMeetingRequestFormRegionFactory_FormRegionInitializing);
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public Microsoft.Office.Tools.Outlook.FormRegionManifest Manifest
            {
                get
                {
                    return this._Manifest;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.IFormRegion Microsoft.Office.Tools.Outlook.IFormRegionFactory.CreateFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            {
                PingAnMeetingRequestFormRegion form = new PingAnMeetingRequestFormRegion(formRegion);
                form.Factory = this;
                return form;
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            byte[] Microsoft.Office.Tools.Outlook.IFormRegionFactory.GetFormRegionStorage(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PingAnMeetingRequestFormRegion));
                return (byte[])resources.GetObject("PingAnMeetingRequestFormRegion");
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            bool Microsoft.Office.Tools.Outlook.IFormRegionFactory.IsDisplayedForItem(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                if (this.FormRegionInitializing != null)
                {
                    Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs cancelArgs = Globals.Factory.CreateFormRegionInitializingEventArgs(outlookItem, formRegionMode, formRegionSize, false);
                    this.FormRegionInitializing(this, cancelArgs);
                    return !cancelArgs.Cancel;
                }
                else
                {
                    return true;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.FormRegionKindConstants Microsoft.Office.Tools.Outlook.IFormRegionFactory.Kind
            {
                get
                {
                    return Microsoft.Office.Tools.Outlook.FormRegionKindConstants.Ofs;
                }
            }
        }
    }

    partial class WindowFormRegionCollection
    {
        internal PingAnMeetingRequestFormRegion PingAnMeetingRequestFormRegion
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.GetType() == typeof(PingAnMeetingRequestFormRegion))
                        return (PingAnMeetingRequestFormRegion)item;
                }
                return null;
            }
        }
    }
}
