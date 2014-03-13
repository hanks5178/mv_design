using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Eplan.EplAddin.MvDesign.Data
{

    
    public class MvButton: Button
    {
		#region Properties

		public Data.MvProject MvProject { get; set; }
		public Panel PanelArranged { get; set; }
		public Panel PanelUnArranged { get; set; }
		public Data.MvFeeder Info { get; set; }

		private bool _isDragging;
		private readonly Point LocationOnGroupBox;
		private Point _mouseLocationOnButton;
		private Point _cursorPosition;
		private Control _sourceControl;
		private Control _targetControl;

		public bool IsArranged
		{
			get
			{
				if (this.BackColor == Color.Aquamarine || this.BackColor == Color.Yellow)
					return true;
				else
					return false;
			}

			set
			{
				string query = "FeederID = '" + this.Info.FeederID.ToString() + "' AND GroupID = '" + this.Info.GroupID.ToString() + "'";
				DataRow[] rows = this.MvProject.FeederTable.Select(query);
				if (rows.Length < 1)
				{
					MessageBox.Show("Could not Find Feeder: " + this.Info.PanelName1);
					return;
				}

				if (value)
				{
					rows[0]["PanelX"] = this.Info.PanelX;
					this.BackColor = Color.Aquamarine;
				}
				else
				{
					rows[0]["PanelX"] = this.Info.PanelX = -1;
					this.BackColor = Color.Silver;
				}

				this.SetButtonText();
			}
		}


		public bool IsSelected
		{
			get
			{
				if (this.BackColor == Color.Yellow)
					return true;
				else
					return false;
			}
			set
			{
				string query = "FeederID = '" + this.Info.FeederID.ToString() + "' AND GroupID = '" + this.Info.GroupID.ToString() + "'";
				DataRow[] rows = this.MvProject.FeederTable.Select(query);
				if (rows.Length < 1)
				{
					MessageBox.Show("Could not Find Feeder: " + this.Info.PanelName1);
					return;
				}

				if (value)
				{
					rows[0]["IsSelected"] = value;
					this.BackColor = Color.Yellow;
				}
				else
				{
					rows[0]["IsSelected"] = value;
					this.BackColor = Color.Aquamarine;
				}
			}
		}

		#endregion

		#region Constructors

		public MvButton() { }

		public MvButton(DataRow row)
		{
			try
			{
				Info = new MvFeeder(row);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}



		public MvButton(Data.MvProject mvProject, DataRow row)
		{
			try
			{
				this.MvProject = mvProject;

				LocationOnGroupBox = new Point(5, 15);

				Info = new MvFeeder(row);

				SetButtonSize();
				this.BackColor = Color.Aquamarine;
				this.Location = LocationOnGroupBox;

				string value = row["IsSelected"].ToString();

				this.TextAlign = ContentAlignment.TopLeft;

				this.SetButtonText();

				this.MouseDown += new MouseEventHandler(Feeder_MouseDown);
				this.MouseMove += new MouseEventHandler(Feeder_MouseMove);
				this.MouseUp += new MouseEventHandler(Feeder_MouseUp);
				this.MouseDoubleClick += new MouseEventHandler(MvButton_MouseDoubleClick);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}




		#endregion

		#region Methods

		private void SetButtonSize()
		{
			if (MvProject.IsDoubleLayer)
			{
				this.Size = new System.Drawing.Size(90, 80);
			}
			else
			{
				this.Size = new System.Drawing.Size(90, 180);
			}
		}

		void SetButtonText()
		{
			this.Text = this.Info.FeederNo + "\r\n" + this.Info.PanelName1 + "\r\n";
		}

		private Point MoveOnPanelUnArranged(Point location)
		{
			location = new Point(location.X - _mouseLocationOnButton.X, location.Y - _mouseLocationOnButton.Y);
			this.PanelArranged.Controls.Remove(this);
			this.Location = location;
			this.PanelUnArranged.Controls.Add(this);
			this.PanelUnArranged.Refresh();
			this.Capture = true;
			this.BringToFront();
			return location;
		}

		private Point MoveOnPanelArranged(Point location)
		{
			location = PanelArranged.PointToClient(Cursor.Position);
			location = new Point(location.X - _mouseLocationOnButton.X, location.Y - _mouseLocationOnButton.Y + PanelArranged.Height);
			PanelUnArranged.Controls.Remove(this);
			this.Location = location;
			PanelArranged.Controls.Add(this);
			this.Capture = true;
			this.BringToFront();
			this.PanelArranged.Refresh();
			return location;
		}

		private void ShiftPanels()
		{
			PanelTag tag = _sourceControl.Tag as PanelTag;
			int sourceIndex = tag.Index;

			_targetControl = GetTargetControl(this.Location);
			if (_targetControl == null) return;

			tag = _targetControl.Tag as PanelTag;
			int targetIndex = tag.Index;

			if (targetIndex > sourceIndex)
			{
				for (int i = sourceIndex; i < targetIndex; i++)
				{
					GroupBox sourceControl = GetTargetControl(i + 1);
					GroupBox targetControl = GetTargetControl(i);

					FlyToTheTarget(sourceControl, targetControl);
				}
			}
			else if (targetIndex < sourceIndex)
			{
				for (int i = sourceIndex; i > targetIndex; i--)
				{
					GroupBox sourceControl = GetTargetControl(i - 1);
					GroupBox targetControl = GetTargetControl(i);

					FlyToTheTarget(sourceControl, targetControl);
				}
			}

			PlaceFeeder(this, _targetControl);
			return;
		}


		void UnPlaceFeeder(MvButton feeder)
		{
			if (feeder == null) return;

			if (feeder.Parent is GroupBox)
			{
				feeder.Location = new Point(feeder.Parent.Location.X + feeder.Location.X, feeder.Parent.Location.Y + feeder.Location.Y);
				feeder.Parent.Controls.Remove(feeder);
				PanelArranged.Controls.Add(feeder);

				this.IsArranged = false;
			}

			feeder.BringToFront();
		}

		void PlaceFeeder(MvButton feeder, Control control)
		{
			if (feeder == null) return;

			if (control is GroupBox)
			{
				PanelTag tag = control.Tag as PanelTag;
				feeder.Info.PanelX = tag.Index;
				feeder.IsArranged = true;
				this.Location = LocationOnGroupBox;
				control.Controls.Add(feeder);
				feeder.BringToFront();
			}
		}

		private MvButton FlyToTheTarget(Control sourceGroupBox, Control targetControl)
		{
			MvButton sourceFeeder = GetFirstFeederInPanel(sourceGroupBox);
			MvButton targetFeeder = GetFirstFeederInPanel(targetControl);

			if (sourceFeeder == null)
			{
				return targetFeeder;
			}

			if (targetControl is GroupBox)
			{
				Point targetLocation = new Point(targetControl.Location.X + LocationOnGroupBox.X, targetControl.Location.Y + LocationOnGroupBox.Y);
				Point feederLocation = new Point(sourceGroupBox.Location.X + LocationOnGroupBox.X, sourceGroupBox.Location.Y + LocationOnGroupBox.Y);
				sourceGroupBox.Controls.Remove(sourceFeeder);
				sourceFeeder.IsArranged = false;
				this.PanelArranged.Controls.Add(sourceFeeder);

				FlyTo(sourceFeeder, feederLocation, targetLocation, 50);

				PanelTag tag = targetControl.Tag as PanelTag;
				sourceFeeder.Info.PanelX = tag.Index;
				sourceFeeder.IsArranged = true;
				sourceFeeder.Location = LocationOnGroupBox;
				targetControl.Controls.Add(sourceFeeder);
			}
			else
			{
				Point feederLocation = new Point(sourceGroupBox.Location.X + sourceFeeder.Location.X, sourceGroupBox.Location.Y + sourceFeeder.Location.Y);
				Point targetLocation = new Point(sourceGroupBox.Location.X + 50, sourceGroupBox.Location.Y + 15);

				sourceGroupBox.Controls.Remove(sourceFeeder);
				sourceFeeder.IsArranged = false;
				this.PanelArranged.Controls.Add(sourceFeeder);

				FlyTo(sourceFeeder, feederLocation, targetLocation, 25);

				sourceFeeder.Location = targetLocation;
			}

			UnPlaceFeeder(targetFeeder);

			return targetFeeder;
		}



		private void FlyTo(Control control, Point from, Point to, double count)
		{
			double deltaX = (to.X - from.X) / count;
			double deltaY = (to.Y - from.Y) / count;
			control.BringToFront();
			for (int i = 0; i < count; i++)
			{
				double dx = i * deltaX;
				double dy = i * deltaY;
				control.Location = new Point((int)(from.X + dx), (int)(from.Y + dy));
				control.Refresh();
			}

			PanelArranged.Refresh();
		}

		private GroupBox GetTargetControl()
		{
			foreach (Control control in PanelArranged.Controls)
			{
				if (control is GroupBox)
				{
					PanelTag tag = control.Tag as PanelTag;
					if (tag.IsEmpty)
					{
						return control as GroupBox;
					}
				}
			}

			return null;
		}

		private GroupBox GetTargetControl(int index)
		{
			foreach (Control control in PanelArranged.Controls)
			{
				if (control is GroupBox)
				{
					PanelTag tag = control.Tag as PanelTag;
					if (tag.Index == index)
					{
						return control as GroupBox;
					}
				}
			}

			return null;
		}

		private Control GetTargetControl(Point controlLocation)
		{
			if (PanelArranged.ClientRectangle.Contains(PanelArranged.PointToClient(Cursor.Position)))
			{
				foreach (Control control in PanelArranged.Controls)
				{
					Rectangle clientRectangle = new Rectangle(control.Location, control.Size);
					if (control is GroupBox && clientRectangle.Contains(controlLocation))
					{
						return control as GroupBox;
					}
				}
			}

			return null;
		}


		private MvButton GetFirstFeederInPanel(Control control)
		{
			if (control is GroupBox)
			{
				foreach (Control c in control.Controls)
				{
					MvButton button = c as MvButton;
                    if (button != null)
                    {
                        return button;
                    }
				}
			}

			return null;
		}


		public void Locate()
		{
			if (this.Info.PanelX > 0)
			{
				GroupBox g = GetTargetControl(this.Info.PanelX);
				if (g != null)
				{
					PlaceFeeder(this, g);
				}
				else
				{
					MessageBox.Show("Could not find Panel No: " + this.Info.PanelX.ToString());
				}
			}
			else
			{
				GroupBox g = GetTargetControl();
				if (g != null)
				{
					g.Controls.Add(this);
					PanelTag tag = g.Tag as PanelTag;
					tag.IsEmpty = false;
					this.Info.PanelX = tag.Index;
					this.IsArranged = true;
				}
				else
				{
					MessageBox.Show("Could not find Empty Panel");
				}
			}

		}




		#endregion


		#region Event Handler

		void Feeder_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this._cursorPosition = Cursor.Position;

				Button button = sender as Button;
				this._sourceControl = button.Parent;

				_mouseLocationOnButton = new Point(e.X, e.Y);
				this.Capture = true;
				this.IsArranged = false;
				_isDragging = true;
				UnPlaceFeeder(this);
			}
			else if (e.Button == MouseButtons.Right)
			{
				if (this.IsArranged)
				{
					this.IsSelected = !this.IsSelected;
				}
			}
		}



		void Feeder_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			if (_isDragging)
			{
				this.Capture = true;
				this.Location = new Point(this.Location.X + e.X - _mouseLocationOnButton.X, this.Location.Y + e.Y - _mouseLocationOnButton.Y);

				if (PanelArranged.Contains(this))
				{
					Point location = PanelUnArranged.PointToClient(Cursor.Position);
					if (PanelUnArranged.ClientRectangle.Contains(location))
					{
						location = MoveOnPanelUnArranged(location);
					}
				}
				else
				{
					Point location = PanelArranged.PointToClient(Cursor.Position);
					if (PanelArranged.ClientRectangle.Contains(location))
					{
						location = MoveOnPanelArranged(location);
					}
				}
			}
		}


		void Feeder_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			_isDragging = false;
			this.Capture = false;

			if (this.MvProject.IsPanelInsertMode)
			{
				if (_sourceControl is GroupBox)
				{
					ShiftPanels();
				}
				else if (_sourceControl is Panel)
				{
					if (PanelArranged.Contains(this))
					{
						_targetControl = GetTargetControl(this.Location);
						FlyToTheTarget(_targetControl, _sourceControl);
						PlaceFeeder(this, _targetControl);
					}
				}
			}
			else
			{
				if (PanelArranged.Contains(this))
				{
					_targetControl = GetTargetControl(this.Location);
					FlyToTheTarget(_targetControl, _sourceControl);
					PlaceFeeder(this, _targetControl);
				}
			}

			return;
		}


		void MvButton_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.IsArranged)
			{
				this.IsSelected = !this.IsSelected;
			}
		}

		#endregion


    }

    public class PanelTag
    {
        public int Index { get; set; }
        public bool IsEmpty { get; set; }
        public MvButton Feeder { get; set; }

        public PanelTag(int index)
        {
            this.Index = index;
            this.IsEmpty = true;
            this.Feeder = null;
        }
    }
}
