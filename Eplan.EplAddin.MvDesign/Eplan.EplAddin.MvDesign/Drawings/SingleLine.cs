using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;
using Eplan.EplApi.DataModel.Graphics;

namespace Eplan.EplAddin.MvDesign.Drawings
{
    public class SingleLine
    {
        public Data.MvProject MvProject { get; set; }

        public int Width = 122;
        public int Height = 300;
        public int StartX = 44;
        public int StartY = 10;


        public SingleLine(string groupNo, string feederNo)
        {


        }

        public bool AddWindowMacro(Page page)
        {


            return false;
        }
    }
}
