namespace Cosmoser.PingAnMeetingRequest.Outlook2010
{
    partial class PingAnMeetingRequestFormRegion : Microsoft.Office.Tools.Outlook.ImportedFormRegionBase
    {
        private Microsoft.Office.Interop.Outlook._DDocSiteControl _DocSiteControlComment;
        private Microsoft.Office.Interop.Outlook.OlkFrameHeader olkFrameHeader;
        private Microsoft.Office.Interop.Outlook.OlkTextBox olkTxtSubject;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel1;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel3;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel4;
        private Microsoft.Office.Interop.Outlook.OlkTimeControl olkStartTimeControl;
        private Microsoft.Office.Interop.Outlook.OlkDateControl olkStartDateControl;
        private Microsoft.Office.Interop.Outlook.OlkTimeControl olkEndTimeControl;
        private Microsoft.Office.Interop.Outlook.OlkDateControl olkEndDateControl;
        private Microsoft.Office.Interop.Outlook.OlkLabel label1;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtliji;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtyuyue;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtshipin;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtbendi;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel5;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel6;
        private Microsoft.Office.Interop.Outlook.OlkTextBox txtPeopleCount;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel8;
        private Microsoft.Office.Interop.Outlook.OlkTextBox txtPhone;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel10;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtxsms0;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtxsms4;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtxsms3;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtxsms2;
        private Microsoft.Office.Interop.Outlook.OlkOptionButton obtxsms1;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel11;
        private Microsoft.Office.Interop.Outlook.OlkTextBox olkTxtLocation;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel17;
        private Microsoft.Vbe.Interop.Forms.UserForm frame1;
        private Microsoft.Vbe.Interop.Forms.UserForm frame2;
        private Microsoft.Vbe.Interop.Forms.UserForm frame3;
        private Microsoft.Vbe.Interop.Forms.UserForm frame5;
        private Microsoft.Office.Interop.Outlook.OlkTextBox txtIPCount;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel14;
        private Microsoft.Office.Interop.Outlook.OlkCommandButton btnCanhuilingdao;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel18;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel19;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel20;
        private Microsoft.Office.Interop.Outlook.OlkCommandButton olkbtnMobileTerm;
        private Microsoft.Office.Interop.Outlook._DRecipientControl _RecipientControl1;
        private Microsoft.Office.Interop.Outlook.OlkCommandButton olkCommandButton1;
        private Microsoft.Office.Interop.Outlook.OlkTextBox olkTextBox1;
        private Microsoft.Office.Interop.Outlook.OlkLabel olkLabel21;
        private Microsoft.Office.Interop.Outlook.OlkCommandButton commandButton1;

        public PingAnMeetingRequestFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.FormRegionShowing += new System.EventHandler(this.PingAnMeetingRequestFormRegion_FormRegionShowing);
            this.FormRegionClosed += new System.EventHandler(this.PingAnMeetingRequestFormRegion_FormRegionClosed);
        }

        protected override void InitializeControls()
        {
            this._DocSiteControlComment = (Microsoft.Office.Interop.Outlook._DDocSiteControl)GetFormRegionControl("_DocSiteControlComment");
            this.olkFrameHeader = (Microsoft.Office.Interop.Outlook.OlkFrameHeader)GetFormRegionControl("OlkFrameHeader");
            this.olkTxtSubject = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("OlkTxtSubject");
            this.olkLabel1 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel1");
            this.olkLabel3 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel3");
            this.olkLabel4 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel4");
            this.olkStartTimeControl = (Microsoft.Office.Interop.Outlook.OlkTimeControl)GetFormRegionControl("OlkStartTimeControl");
            this.olkStartDateControl = (Microsoft.Office.Interop.Outlook.OlkDateControl)GetFormRegionControl("OlkStartDateControl");
            this.olkEndTimeControl = (Microsoft.Office.Interop.Outlook.OlkTimeControl)GetFormRegionControl("OlkEndTimeControl");
            this.olkEndDateControl = (Microsoft.Office.Interop.Outlook.OlkDateControl)GetFormRegionControl("OlkEndDateControl");
            this.label1 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("Label1");
            this.obtliji = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtliji");
            this.obtyuyue = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtyuyue");
            this.obtshipin = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtshipin");
            this.obtbendi = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtbendi");
            this.olkLabel5 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel5");
            this.olkLabel6 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel6");
            this.txtPeopleCount = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("txtPeopleCount");
            this.olkLabel8 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel8");
            this.txtPhone = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("txtPhone");
            this.olkLabel10 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel10");
            this.obtxsms0 = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtxsms0");
            this.obtxsms4 = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtxsms4");
            this.obtxsms3 = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtxsms3");
            this.obtxsms2 = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtxsms2");
            this.obtxsms1 = (Microsoft.Office.Interop.Outlook.OlkOptionButton)GetFormRegionControl("obtxsms1");
            this.olkLabel11 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel11");
            this.olkTxtLocation = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("OlkTxtLocation");
            this.olkLabel17 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel17");
            this.frame1 = (Microsoft.Vbe.Interop.Forms.UserForm)GetFormRegionControl("Frame1");
            this.frame2 = (Microsoft.Vbe.Interop.Forms.UserForm)GetFormRegionControl("Frame2");
            this.frame3 = (Microsoft.Vbe.Interop.Forms.UserForm)GetFormRegionControl("Frame3");
            this.frame5 = (Microsoft.Vbe.Interop.Forms.UserForm)GetFormRegionControl("Frame5");
            this.txtIPCount = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("txtIPCount");
            this.olkLabel14 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel14");
            this.btnCanhuilingdao = (Microsoft.Office.Interop.Outlook.OlkCommandButton)GetFormRegionControl("btnCanhuilingdao");
            this.olkLabel18 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel18");
            this.olkLabel19 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel19");
            this.olkLabel20 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel20");
            this.olkbtnMobileTerm = (Microsoft.Office.Interop.Outlook.OlkCommandButton)GetFormRegionControl("OlkbtnMobileTerm");
            this._RecipientControl1 = (Microsoft.Office.Interop.Outlook._DRecipientControl)GetFormRegionControl("_RecipientControl1");
            this.olkCommandButton1 = (Microsoft.Office.Interop.Outlook.OlkCommandButton)GetFormRegionControl("OlkCommandButton1");
            this.olkTextBox1 = (Microsoft.Office.Interop.Outlook.OlkTextBox)GetFormRegionControl("OlkTextBox1");
            this.olkLabel21 = (Microsoft.Office.Interop.Outlook.OlkLabel)GetFormRegionControl("OlkLabel21");
            this.commandButton1 = (Microsoft.Office.Interop.Outlook.OlkCommandButton)GetFormRegionControl("cbSend");

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
