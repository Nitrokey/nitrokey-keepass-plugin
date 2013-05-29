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

//using cryptotest;

using KeePass.UI;

using KeePassLib.Keys;
using KeePassLib.Utility;
using KeePass.Plugins;

namespace CryptokiKeyProvider.Forms
{
    public partial class CkpPromtForm : Form
    {
        public string pin;

        public CkpPromtForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            GlobalWindowManager.AddWindow(this);

            string strTitle = "PKCS#11";
            string strDesc = "Enter your pin.";

            this.Text = strTitle;
            BannerFactory.CreateBannerEx(this, banner,
                Resource.chip, strTitle, strDesc);
            this.textBox1.PasswordChar = '*';
            this.textBox1.Select();

        }

        private void OnFormClose(Object sender, FormClosingEventArgs e)
        {
        }


        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.pin = this.textBox1.Text;
        }
    }
}
