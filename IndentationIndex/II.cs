using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace IndentationIndex
{
    public class II : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public II()
        {
        }

        protected override void OnClick()
        {
            Form IIGUI = new IIGUI();
            IIGUI.Show();
            ArcMap.Application.CurrentTool = null;
        }
    }
}
