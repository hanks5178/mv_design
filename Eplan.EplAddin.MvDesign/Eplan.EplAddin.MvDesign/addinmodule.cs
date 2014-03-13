using System;

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Gui;

namespace Eplan.EplAddin.MvDesign
{
    /// <summary>
    ///   That is an example for a EPLAN addin.  
    ///   Exactly a class must implement the interface Eplan.EplApi.ApplicationFramework.IEplAddIn.  
    ///   An Assembly is identified through this criterion as EPLAN addin!  
    /// </summary>  
    public class AddInModule : IEplAddIn
    {
		private static NLog.Logger _logger;
		public static NLog.Logger GetUniqueLogger()
		{
			if (_logger == null)
			{
				_logger = MyLogManager.MyLogManager.Instance.GetLogger("MvDesign");
			}

			return _logger;
		}

        /// <summary>
        /// The function is called once during registration add-in.
        /// </summary>
        /// <param name="bLoadOnStart"> true: In the next P8 session, add-in will be loaded during initialization</param>
        /// <returns></returns>
        public bool OnRegister(ref System.Boolean bLoadOnStart)
        {
            bLoadOnStart = true;

            return true;
        }
        /// <summary>
        /// The function is called during unregistration the add-in.
        /// </summary>
        /// <returns></returns>
        public bool OnUnregister()
        {
            return true;
        }

        /// <summary>
        /// The function is called during P8 initialization or registration the add-in.  
        /// </summary>
        /// <returns></returns>
        public bool OnInit()
        {
            return true;
        }
        /// <summary>
        /// The function is called during P8 initialization or registration the add-in, when GUI was already initialized and add-in can modify it. 
        /// </summary>
        /// <returns></returns>
        public bool OnInitGui()
        {
            Menu mainMenu = new Menu();
            UInt32 iMainMenu = mainMenu.AddMainMenu("MV ����", Eplan.EplApi.Gui.Menu.MainMenuName.eMainMenuHelp, "�ű�", "MvNewAction", "", 1);
			UInt32 menu1 = mainMenu.AddMenuItem("����", "MvEditAction", "����", iMainMenu, 2, false, false);
			UInt32 menu2 = mainMenu.AddMenuItem("��ǰ Ȯ��", "MvCheckAction", "��ǰ Ȯ��", iMainMenu, 3, false, false);
			UInt32 menu3 = mainMenu.AddMenuItem("���α׷� ����", "MvAboutAction", "���α׷� ����", iMainMenu, 4, true, false);

            return true;
        }
        /// <summary>
        /// This function is called during closing P8 or unregistration the add-in. 
        /// </summary>
        /// <returns></returns>
        public bool OnExit()
        {
            return true;
        }
    }
}
