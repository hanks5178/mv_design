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
            UInt32 iMainMenu = mainMenu.AddMainMenu("MV 설계", Eplan.EplApi.Gui.Menu.MainMenuName.eMainMenuHelp, "신규", "MvNewAction", "", 1);
			UInt32 menu1 = mainMenu.AddMenuItem("진행", "MvEditAction", "진행", iMainMenu, 2, false, false);
			UInt32 menu2 = mainMenu.AddMenuItem("부품 확인", "MvCheckAction", "부품 확인", iMainMenu, 3, false, false);
			UInt32 menu3 = mainMenu.AddMenuItem("프로그램 정보", "MvAboutAction", "프로그램 정보", iMainMenu, 4, true, false);

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
