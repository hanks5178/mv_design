using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Eplan.EplAddin.MvDesign.Data
{
	public class Symbol: IComparable<Symbol>
	{
		public Function Function { get; set; }
		public string Name { get; set; }
		public double X { get; set; }
		public double Y { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public SymbolReference SymbolReference { get; set; }

		public List<Pin> PinList { get; set; }
	
		public Symbol(Function f)
		{
			this.Function = f;
			this.SymbolReference = f as SymbolReference;

			this.Name = f.Name;
			PointD[] points = f.GetBoundingBox();
			this.Width = points[1].X - points[0].X;
			this.Height = points[1].Y - points[0].Y;
			this.X = points[0].X + Width / 2;
			this.Y = points[0].Y + Height / 2;

			this.PinList = CollectPins(f);
		}

		private List<Pin> CollectPins(Function function)
		{
			List<Pin> list = new List<Pin>();

			System.Drawing.RectangleF r = Util.eplan.GetBoundingBox(function);

			foreach (Placement p in function.Page.AllPlacements)
			{
				if (p != function)
				{
					System.Drawing.Point location = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
					if (r.Contains(location))
					{
						Function f = p as Function;
						if (f == null) continue;

						list.AddRange(f.Pins);
					}
				}
			}
			return list;
		}

		public void MovePlacements(PointD delta)
		{
			System.Drawing.RectangleF r = Util.eplan.GetBoundingBox(this.Function);

			this.Function.Location = new PointD(this.Function.Location.X + delta.X, this.Function.Location.Y + delta.Y);

			foreach (Placement p in this.Function.Page.AllPlacements)
			{
				System.Drawing.Point location = new System.Drawing.Point((int)p.Location.X, (int)p.Location.Y);
				if (r.Contains(location))
				{
					p.Location = new PointD(p.Location.X + delta.X, p.Location.Y + delta.Y);
				}
			}

			return;
		}

		#region IComparable<Symbol> Members

		public int CompareTo(Symbol other)
		{
			if (this.X < other.X)
			{
				return -1;
			}
			else if (this.X == other.X)
			{
				if (this.Y < other.Y)
					return -1;
				else if (this.Y == other.Y)
					return 0;
				else
					return 1;
			}
			else
			{
				return 1;
			}
		}

		#endregion
	}
}
