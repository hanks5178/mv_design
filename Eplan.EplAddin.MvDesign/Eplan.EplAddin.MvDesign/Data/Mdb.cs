using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.MasterData;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign.Data
{
	public class Mdb
    {
        public List<string> CategoryList { get; set; }
        public List<string> SubCategoryList { get; set; }

        public DataTable PartTable { get; set; }
		public DataView PartListView { get; set; }

        public string ConnectionString
        {
            get
            {
                PartsService ps = new PartsService();
                string path = PathMap.SubstitutePath(ps.PartsDatabase);
                return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
            }
        }

        public Mdb()
        {
			try
			{
				using (OleDbConnection connection = new OleDbConnection(ConnectionString))
				{
					connection.Open();

					string query = "SELECT partnr, manufacturer, groupsymbolmacro FROM tblPart";
					OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
					PartTable = new DataTable();
					adapter.Fill(PartTable);

					query = "SELECT DISTINCT val FROM tblAttribute";
					query += " where tblAttribute.pos = 11";
					OleDbCommand command = new OleDbCommand(query, connection);

					OleDbDataReader reader = command.ExecuteReader();
					CategoryList = new List<string>();
					while (reader.Read())
					{
						CategoryList.Add(reader["val"].ToString());
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
        }

        public void GetSubCategories(string category)
        {
			try
			{
				using (OleDbConnection connection = new OleDbConnection(ConnectionString))
				{
					connection.Open();

					string query = "SELECT DISTINCT t2.val FROM tblAttribute as t1, tblAttribute as t2";
					query += " where  t1.id = t2.id AND t1.pos = 11 and t1.val = '" + category + "'";
					OleDbCommand command = new OleDbCommand(query, connection);

					OleDbDataReader reader = command.ExecuteReader();
					SubCategoryList = new List<string>();
					while (reader.Read())
					{
						SubCategoryList.Add(reader["val"].ToString());
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
        }

        public DataTable FindParts(string category, string subCategory)
        {
			try
			{
				using (OleDbConnection connection = new OleDbConnection(ConnectionString))
				{
					connection.Open();
					category = category.Replace("\'", "\'\'");
					subCategory = subCategory.Replace("\'", "\'\'");

					string query = "SELECT partnr, manufacturer, groupsymbolmacro FROM tblPart, tblAttribute";
					query += " where  tblPart.id = tblAttribute.id AND tblAttribute.pos = 12 and tblAttribute.val = '" + subCategory + "'";
					OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);

					return dataTable;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
        }

		public DataTable SearchParts(string category, string subCategory, string query)
		{
			try
			{
				using (OleDbConnection connection = new OleDbConnection(this.ConnectionString))
				{
					connection.Open();
					category = category.Replace("\'", "\'\'");
					subCategory = subCategory.Replace("\'", "\'\'");

					string joinQuery = "SELECT tbl1.id FROM tblAttribute tbl1 INNER JOIN tblAttribute tbl2 "
						+ "ON tbl1.id = tbl2.id "
						+ "WHERE tbl1.pos = 11 AND tbl1.val <> tbl2.val "
						+ "AND tbl1.val = '" + category + "' AND tbl2.val = '" + subCategory + "'";

					string selectQuery = "SELECT partnr, description1, manufacturer, graphicmacro, groupsymbolmacro FROM tblPart "
						+ "WHERE tblPart.id IN (" + joinQuery + ")";

					OleDbCommand command = new OleDbCommand(selectQuery, connection);
					OleDbDataAdapter adapter = new OleDbDataAdapter(command);
					DataTable table = new DataTable();
					adapter.Fill(table);
					this.PartListView = table.DefaultView;
					string[] tokens = query.Split(';');
					for (int i=0; i<tokens.Length; i++)
					{
						if (i == 0)
							this.PartListView.RowFilter = "partnr like '%" + tokens[i] + "%' ";
						else
							this.PartListView.RowFilter += "AND partnr like '%" + tokens[i] + "%' ";
					}

					return table;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
		}

		public string[] GetAttribute(string category, string subCategory)
		{
			string[] attributes = new string[3] { string.Empty, string.Empty, string.Empty };
			try
			{
				using (OleDbConnection connection = new OleDbConnection(this.ConnectionString))
				{
					string joinQuery = "SELECT tbl1.id FROM tblAttribute tbl1 INNER JOIN tblAttribute tbl2 "
						+ "ON tbl1.id = tbl2.id "
						+ "WHERE tbl1.pos = 11 AND tbl1.val <> tbl2.val "
						+ "AND tbl1.val = '" + category + "' AND tbl2.val = '" + subCategory + "'";

					string selectQuery = "SELECT productgroup, productsubgroup, producttopgroup FROM tblPart "
						+ "WHERE tblPart.id IN (" + joinQuery + ")";

					connection.Open();
					OleDbCommand command = new OleDbCommand(selectQuery, connection);
					OleDbDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						attributes[0] = reader[0].ToString();
						attributes[1] = reader[1].ToString();
						attributes[2] = reader[2].ToString();

						if (attributes[0].Length > 0 &&
							attributes[1].Length > 0 &&
							attributes[2].Length > 0) return attributes;
					}
				}

				return attributes;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
		}


		public void AddAttribite(MDPart part, string category, string subCategory)
		{
			try
			{
				using (OleDbConnection connection = new OleDbConnection(this.ConnectionString))
				{
					connection.Open();
					category = category.Replace("\'", "\'\'");
					subCategory = subCategory.Replace("\'", "\'\'");

					string selectQuery = "SELECT id, partnr, description1, manufacturer, graphicmacro, groupsymbolmacro FROM tblPart "
						+ "WHERE partnr ='" + part.PartNr + "'";

					OleDbCommand command = new OleDbCommand(selectQuery, connection);
					OleDbDataReader reader = command.ExecuteReader();
					if (!reader.Read())
					{
						MessageBox.Show(part.PartNr + " 부품을 찾을 수 없습니다.");
						return;
					}

					string id = reader[0].ToString();

					command = new OleDbCommand(selectQuery, connection);
					string insertQuery = "INSERT INTO tblAttribute VALUES(1, " + id + ", 11, '" + category + "')";
					command.CommandText = insertQuery;
					command.ExecuteNonQuery();

					insertQuery = "INSERT INTO tblAttribute VALUES(1, " + id + ", 12, '" + subCategory + "')";
					command.CommandText = insertQuery;
					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}


        public void ShowCatergoty(ComboBox comboBox)
        {
            comboBox.DataSource = null;
            comboBox.DataSource = this.CategoryList;
			comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
		}


        public void ShowSubCatergory(string category, ComboBox comboBox)
        {
            this.GetSubCategories(category);
            comboBox.DataSource = null;
            comboBox.DataSource = this.SubCategoryList;
            comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        public void ShowSearchResult(string category, string subCategory, DataGridView dataGridView)
        {
            dataGridView.DataSource = null;
			dataGridView.DataSource = this.FindParts(category, subCategory);
            dataGridView.AllowUserToResizeColumns = true;

            dataGridView.RowHeadersVisible = false;
            dataGridView.Columns[0].Width = 200;
            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 400;
        }


		public void ShowSearchResult(string category, string subCategory, string condition, DataGridView dataGridView)
		{
			dataGridView.DataSource = null;
			dataGridView.DataSource = this.SearchParts(category, subCategory, condition);

			dataGridView.AllowUserToResizeColumns = true;

			dataGridView.RowHeadersVisible = false;
			dataGridView.Columns[0].Width = 200;
			dataGridView.Columns[1].Width = 80;
			dataGridView.Columns[2].Width = 400;
		}
    }
	

	#region MdbFactory

	public class MdbFactory
    {
        private static Mdb mdb = null;
        public static Mdb GetUniqueInstance
        {
            get
            {
                if (mdb == null)
                {
                    mdb = new Mdb();
                    return mdb;
                }
                else
                {
                    return mdb;
                }
            }
        }
    }
	
	#endregion

}
