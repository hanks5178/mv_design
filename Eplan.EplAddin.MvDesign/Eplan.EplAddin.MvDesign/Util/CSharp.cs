using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Eplan.EplApi.DataModel;
namespace Eplan.EplAddin.MvDesign.Util
{
    public class CSharp
    {
        public static void ReadTemplates(ComboBox comboBox)
        {
            ProjectManager pm = new ProjectManager();
            string path = pm.Paths.Templates;
            new FileBinder(path, "ZW9", comboBox);
        }

		public static void ReadPlotFrames(ComboBox comboBox)
		{
			ProjectManager pm = new ProjectManager();
			string path = pm.Paths.Frames;
			string[] fileNames = Directory.GetFiles(path, "*.FN1");
			comboBox.Items.Clear();
			foreach (string fileName in fileNames)
			{
				comboBox.Items.Add(Path.GetFileNameWithoutExtension(fileName));
			}
		}

		public static void ReadPageMacro(ComboBox comboBox)
		{
			ProjectManager pm = new ProjectManager();
			string path = pm.Paths.Templates;
			new FileBinder(path, "EMP", comboBox);
		}

        public static void BindRatings(string modelName, ComboBox comboBox)
        {
            FolderBinder folderBinder = new FolderBinder(Data.Location.Rating(false, modelName), false, comboBox);
        }

        public static void BindRatings(string modelName, DataGridViewComboBoxColumn column)
        {
            new FolderBinder(Data.Location.Rating(false, modelName), false, column);
        }

        public static void BindComboBox(object dataSource, ComboBox comboBox, string dataMemebr)
        {
            comboBox.DataBindings.Clear();
            comboBox.DataBindings.Add(new Binding("Text", dataSource, dataMemebr, true, DataSourceUpdateMode.OnPropertyChanged));
        }

        public static void BindTextBox(object dataSource, TextBox textBox, string dataMemebr)
        {
            textBox.DataBindings.Clear();
            textBox.DataBindings.Add(new Binding("Text", dataSource, dataMemebr, true, DataSourceUpdateMode.OnPropertyChanged));
        }

        public static void BindNumericUpDown(object dataSource, NumericUpDown numericUpDown, string dataMember)
        {
            numericUpDown.DataBindings.Clear();
            numericUpDown.DataBindings.Add(new Binding("Value", dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged));
        }

		public static void AutoSizeDataGridViewColumn(DataGridView dataGridView)
		{
			int count = dataGridView.Columns.Count;
			int[] width = new int[count];

			dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			for (int i = 0; i < count; i++)
			{
				width[i] = dataGridView.Columns[i].Width;
			}

			dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			for (int i = 0; i < count; i++)
			{
				dataGridView.Columns[i].Width = width[i];
			}
		}

        public static void ShowRowHeaderNumber(object sender, DataGridViewRowPostPaintEventArgs e)
        {
			DataGridView dataGridView = sender as DataGridView;
			if (dataGridView == null) return;

            string rowNumber = (e.RowIndex + 1).ToString();
            while (rowNumber.Length < dataGridView.RowCount.ToString().Length) rowNumber = "0" + rowNumber;

            SizeF size = e.Graphics.MeasureString(rowNumber, dataGridView.DefaultCellStyle.Font);
            if (dataGridView.RowHeadersWidth < (int)(size.Width + 20)) dataGridView.RowHeadersWidth = (int)(size.Width + 20);
            Brush b = SystemBrushes.ControlText;
			e.Graphics.DrawString(rowNumber, dataGridView.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        public static void CopyCell(DataGridView dataGridView)
        {
            if (dataGridView.SelectedCells.Count < 1) return;
            object value = dataGridView.SelectedCells[dataGridView.SelectedCells.Count - 1].Value;
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                cell.Value = value;
            }

            dataGridView.EndEdit();
        }

        public static void FillContinuousData_(DataGridView dataGridView)
        {
            if (dataGridView.SelectedCells.Count < 1) return;
            string value = dataGridView.SelectedCells[dataGridView.SelectedCells.Count - 1].Value.ToString();

            int index = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(value[i]))
                {
                    index = i;
                    break;
                }
            }

            string prefix = value;
            if (index < value.Length)
            {
                prefix = value.Substring(0, index + 1);
            }

            string postfix = string.Empty;
            if (index < value.Length - 1)
            {
                postfix = value.Substring(index + 1);
            }

            int sequence = 0;
            if (!int.TryParse(postfix, out sequence))
            {
                sequence = 1;
            }

            string format = "D" + postfix.Length.ToString();
            for (int i = dataGridView.SelectedCells.Count - 1; i >= 0; i--)
            {
                dataGridView.SelectedCells[i].Value = prefix + sequence.ToString(format);
                sequence++;
            }

            dataGridView.EndEdit();
        }


		public static bool StringHasTrailingNumber(string input, out string prefix, out int number, out string format)
		{
			prefix = string.Empty;
			number = 0;
			format = string.Empty;

			prefix = Regex.Replace(input, "[0-9]*$", string.Empty);
			string postfix = input.Substring(prefix.Length);
			if (postfix.Length < 1)
			{
				format = "D1";
				return false;
			}
			else
			{
				format = "D" + postfix.Length.ToString();
				int.TryParse(postfix, out number);

				return true;
			}
		}

		public static void FillContinuousData(DataGridView dataGridView)
		{
			if (dataGridView.SelectedCells.Count < 1) return;

			string value = dataGridView.SelectedCells[dataGridView.SelectedCells.Count - 1].Value.ToString();
			string prefix;
			int index;
			string format;

			if (!StringHasTrailingNumber(value, out prefix, out index, out format))
			{
				index++;
			}

			for (int i = dataGridView.SelectedCells.Count - 1; i >= 0; i--)
			{
				dataGridView.SelectedCells[i].Value = prefix + index.ToString(format);
				index++;
			}

			dataGridView.EndEdit();
		}


		public static List<string> GetDirectoryList(string path)
		{
			List<string> list = new List<string>();
			if (!Directory.Exists(path)) return list;

			string[] folderNames = Directory.GetDirectories(path);
			foreach (string folderName in folderNames)
			{
				list.Add(Path.GetFileName(folderName));
			}

			list.Sort();
			list.Reverse();

			return list;
		}

		public static void CopyToClipBoard(DataGridView dataGridView)
		{
			DataObject o = dataGridView.GetClipboardContent();
			if (o != null)
			{
				Clipboard.SetDataObject(o);
			}
		}


		public static void PasteFromClipboard(DataGridView dataGridView)
		{
			if (dataGridView.SelectedCells.Count == 0)
			{
				MessageBox.Show("Please select a cell", "Paste",
				MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			DataGridViewCell startCell = GetStartCell(dataGridView);
			Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValues(Clipboard.GetText());

			int iRowIndex = startCell.RowIndex;
			foreach (int rowKey in cbValue.Keys)
			{
				int iColIndex = startCell.ColumnIndex;
				foreach (int cellKey in cbValue[rowKey].Keys)
				{
					if (iColIndex <= dataGridView.Columns.Count - 1 &&
						iRowIndex <= dataGridView.Rows.Count - 1)
					{
						DataGridViewCell cell = dataGridView[iColIndex, iRowIndex];
						cell.Value = cbValue[rowKey][cellKey];
					}
					iColIndex++;
				}
				iRowIndex++;
			}
		}

		private static DataGridViewCell GetStartCell(DataGridView dataGridView)
		{
			if (dataGridView.SelectedCells.Count == 0) return null;

			int rowIndex = dataGridView.Rows.Count - 1;
			int colIndex = dataGridView.Columns.Count - 1;

			foreach (DataGridViewCell dgvCell in dataGridView.SelectedCells)
			{
				if (dgvCell.RowIndex < rowIndex)
					rowIndex = dgvCell.RowIndex;
				if (dgvCell.ColumnIndex < colIndex)
					colIndex = dgvCell.ColumnIndex;
			}

			return dataGridView[colIndex, rowIndex];
		}

		private static Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
		{
			Dictionary<int, Dictionary<int, string>> copyValues = new Dictionary<int, Dictionary<int, string>>();
			String[] lines = clipboardValue.Split('\n');
			for (int i = 0; i <= lines.Length - 1; i++)
			{
				copyValues[i] = new Dictionary<int, string>();
				String[] lineContent = lines[i].Split('\t');
				if (lineContent.Length == 0)
				{
					copyValues[i][0] = string.Empty;
				}
				else
				{
					for (int j = 0; j <= lineContent.Length - 1; j++)
					{
						copyValues[i][j] = lineContent[j];
					}
				}
			}

			return copyValues;
		}
    }
}
