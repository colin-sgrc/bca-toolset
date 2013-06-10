#region License, Terms and Conditions
//
// BCAAImport: BC Assessment Import Utility
// Written by Colin Dyck (Selkirk Geospatial Research Centre)
// Copyright (c) 2012. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License
// along with this library; If not, see <http://www.gnu.org/licenses/>.
//
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SGRC.BCATools
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Splash : Form
    {
        static Splash _form = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Splash" /> class.
        /// </summary>
        public Splash()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            txtVersion.Text = String.Format("Version {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            base.OnLoad(e);
        }


        /// <summary>
        /// Closes the form.
        /// </summary>
        static public void CloseForm()
        {
            _form.Close();
        }

        /// <summary>
        /// Shows the splash screen.
        /// </summary>
        static public void ShowSplashScreen()
        {
            if (_form != null)
            {
                return;
            }

            _form = new Splash();
            _form.Show();
            Application.DoEvents();

            Thread.Sleep(1500);
            CloseForm();
        }
    }
}
