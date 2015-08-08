using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.OutlookAddinInstaller
{
    public class RegisterInfo
    {
        private string _manifest = string.Empty;
        private string _keyName = string.Empty;
        private string _description = string.Empty;
        private string _friendlyName = string.Empty;

        /// <summary>
        /// this class is the register and unregister helper 
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="keyName"></param>
        /// <param name="description"></param>
        /// <param name="friendlyName"></param>
        public RegisterInfo(string manifest, string keyName, string description, string friendlyName)
        {
            this._manifest = manifest;
            this._keyName = keyName;
            this._description = description;
            this._friendlyName = friendlyName;
        }

        public RegisterInfo(string keyName)
        {
            this._keyName = keyName;
        }


        public string Manifest
        {
            get
            {
                return this._manifest;
            }
            set
            {
                this._manifest = value;
            }
        }

        public string KeyName
        {
            get
            {
                return this._keyName;
            }
            set
            {
                this._keyName = value;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public string FriendlyName
        {
            get
            {
                return this._friendlyName;
            }
            set
            {
                this._friendlyName = value;
            }
        }

    }
}
