/*
  CryptokiKeyPrivder - A PKCS#11 Plugin for Keepass
  Copyright (C) 2013 Daniel Pieper <daniel.pieper@implogy.de>

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Net.Pkcs11Interop.Common;
using System.IO;
//using cryptotest;

using KeePass.UI;

using KeePassLib.Keys;
using KeePassLib.Utility;
using KeePass.Plugins;

namespace CryptokiKeyProvider.Forms
{
    public partial class CkpCreationForm : Form
    {
        List<Pkcs11.keyfile> keyfiles;
        public byte[] selected_key;
        private IPluginHost m_host = null;
        //uint[] slotList;

        public CkpCreationForm()
        {
            InitializeComponent();
            
        }

        public CkpCreationForm(IPluginHost m_host)
        {
            this.m_host = m_host;
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            GlobalWindowManager.AddWindow(this);

            string strTitle = "Configure PKCS#11";
            string strDesc = "Protect the database using a smartcard or token.";

            this.Text = strTitle;
            BannerFactory.CreateBannerEx(this, banner,
                Resource.chip, strTitle, strDesc);

            if (CryptokiKeyProvider.pkcs11_conf_lib != null)
            {
                textBox1.Text = CryptokiKeyProvider.pkcs11_conf_lib;
                try
                {
                    keyfiles = Pkcs11.read_allkeyfiles(CryptokiKeyProvider.pkcs11_conf_lib);
                    addKeyfiles2listview();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OnFormClose(Object sender, FormClosingEventArgs e)
        {
            deinit_pkcs11();
        }

        private void deinit_pkcs11()
        {
            try
            {
                Pkcs11.Logout();
                //Pkcs11.CloseSession();

            } catch (Exception) 
            {}
        }


        private void addKeyfiles2listview()
        {
            listView1.Enabled = true;
            foreach (Pkcs11.keyfile keyfile in keyfiles)
            {
                this.listView1.Items.Add(new ListViewItem(new[] { Convert.ToString(keyfile.slotid), keyfile.token_name, keyfile.label }));
            }
            
        }

        private void saveSettings(string lib, string slot, string label)
        {

            string configBase = "CryptokiKeyProvider.";
            string strRef =
                Environment.MachineName + "." +
                Environment.UserDomainName + "." +
                Environment.UserName;
            string pkcs11_conf_lib = configBase + strRef + "pkcs11_library";
            string pkcs11_conf_slot = configBase + strRef + "pkcs11_slot";
            string pkcs11_conf_label = configBase + strRef + "pkcs11_label";

            m_host.CustomConfig.SetString(pkcs11_conf_lib, lib);
            m_host.CustomConfig.SetString(pkcs11_conf_slot, slot);
            m_host.CustomConfig.SetString(pkcs11_conf_label, label);
        }

        private void selectLibrary_Click(object sender, EventArgs e)
        {
 
            OpenFileDialog ofd = new OpenFileDialog();
			PlatformID platformId = System.Environment.OSVersion.Platform;
			if ((platformId == PlatformID.Unix) || (platformId == PlatformID.MacOSX))
            {
                ofd.Filter = "so file (*.so)|*.so|All files (*.*)|*.*";
            }else ofd.Filter = "so file (*.so)|*.so|dll files (*.dll)|*.dll|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = ofd.FileName;

            try
            {
                this.keyfiles = Pkcs11.read_allkeyfiles(ofd.FileName);
                addKeyfiles2listview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void listView_SelectedIndexChange(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 1)
            {
                this.buttonExportKey.Enabled = true;
                this.buttonDeleteKey.Enabled = true;
            }
            else
            {
                this.buttonExportKey.Enabled = false;
                this.buttonDeleteKey.Enabled = false;
            }
        }

        private void buttonDeleteKey_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("sorry, operation not implemented");
			if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("No key selected");
                DialogResult = DialogResult.None;
                return;
            }

            //string selected_label = this.listView1.SelectedItems[0].SubItems[2].Text;
            Pkcs11.keyfile key = keyfiles[this.listView1.SelectedItems[0].Index];
			try{
				Pkcs11.deleteDataObject(key);
			}catch(Exception ex){

			}
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("No key selected");
                DialogResult = DialogResult.None;
                return;
            }

            string selected_label = this.listView1.SelectedItems[0].SubItems[2].Text;
            Pkcs11.keyfile key = keyfiles[this.listView1.SelectedItems[0].Index];
           
            try {
               selected_key = Pkcs11.GetAttributeValue(key.handle, CKA.CKA_VALUE);                          }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            
            //string configBase = "CryptokiKeyProvider.";
            //string strRef =
            //    Environment.MachineName + "." +
            //    Environment.UserDomainName + "." +
             //   Environment.UserName;
            //string pkcs11_conf_lib = configBase + strRef + "pkcs11_library";
            //string pkcs11_conf_slot = configBase + strRef + "pkcs11_slot";
            //string pkcs11_conf_label = configBase + strRef + "pkcs11_label";

            saveSettings(this.textBox1.Text, this.listView1.SelectedItems[0].SubItems[0].Text, selected_label);
            //deinit_pkcs11();
        }

        private void buttonExportKey_Click(object sender, EventArgs e)
        {
            MessageBox.Show("sorry, operation not implemented");
        }

        private void buttonImportKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files (*.*)|*.*";                            
            try
            {
				if (ofd.ShowDialog() == DialogResult.OK){
					byte[] bytes = File.ReadAllBytes(ofd.FileName);
					Pkcs11.createKeyfile(ofd.SafeFileName,bytes);
					Pkcs11.Logout();
					keyfiles = Pkcs11.read_allkeyfiles(CryptokiKeyProvider.pkcs11_conf_lib);
                    addKeyfiles2listview();
				}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            deinit_pkcs11();
        }
    }
}
