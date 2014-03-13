using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eplan.EplAddin.MvDesign.Data
{
    class Tag
    {
        public int Index { get; set; }
        public bool IsEmpty { get; set; }
        public MvFeeder Feeder { get; set; }

        public Tag(int index)
        {
            this.Index = index;
            this.IsEmpty = true;
            this.Feeder = null;
        }
    }
}
