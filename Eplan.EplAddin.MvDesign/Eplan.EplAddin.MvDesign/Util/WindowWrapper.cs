using System;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.Util
{
    public class WindowWrapper : IWin32Window
    {
        private IntPtr _hWindow;

        public WindowWrapper(IntPtr handle)
        {
            _hWindow = handle;
        }


        #region IWin32Window Members

        public IntPtr Handle
        {
            get { return _hWindow; }
        }

        #endregion
    }
}
