using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eplan.EplApi.Base;

namespace Eplan.EplAddin.MvDesign.Data
{
    public class Location
    {
        public static string Root { get { return PathMap.SubstitutePath("$EPLAN_DATA"); } }
        public static string Image { get { return Location.Root + @"\MV_DESIGN\Config\Image\"; } }
        public static string Config { get { return Location.Root + @"\MV_DESIGN\Config\"; } }

        public static string Model(bool isShip)
        {
            if (isShip)
                return Location.Root + @"\MV_DESIGN\SHIP\Model\";
            else
                return Location.Root + @"\MV_DESIGN\LAND\Model\";
        }

        public static string Macro(bool isShip)
        {
            if (isShip)
                return Location.Root + @"\MV_DESIGN\SHIP\Macro\";
            else
                return Location.Root + @"\MV_DESIGN\LAND\Macro\";
        }

        public static string Rating(bool isShip, string modelName)
        {
            return Location.Model(isShip) + modelName + @"\";
        }

        public static string Front(bool isShip, string modelName, string rating)
        {
            return Location.Rating(isShip, modelName) + rating + @"\Front\";
        }
        public static string Rear(bool isShip, string modelName, string rating)
        {
            return Location.Rating(isShip, modelName) + rating + @"\Rear\";
        }

        public static string Single(bool isShip, string modelName, string rating)
        {
            return Location.Rating(isShip, modelName) + rating + @"\Single\";
        }

        public static string Bottom(bool isShip, string modelName, string rating)
        {
            return Location.Rating(isShip, modelName) + rating + @"\Bottom\";
        }

        public static string Door(bool isShip, string modelName, string rating)
        {
            return Location.Rating(isShip, modelName) + rating + @"\Door\";
        }
    }
}
