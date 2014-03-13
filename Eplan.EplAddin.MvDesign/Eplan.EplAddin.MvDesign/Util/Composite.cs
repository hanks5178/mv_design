using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.Util
{
    public abstract class Composite : IEquatable<Composite>
    {
        public bool IsVaild { get; set; }
        public Image Icon { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }

        public Composite(string location, string filePath)
        {
            this.Location = location;
            this.Name = Path.GetFileName(filePath);
            this.FullPath = filePath;
            this.Extension = string.Empty;
            this.IsVaild = false;
        }

        public bool Equals(Composite other)
        {
            return this.Name.ToUpper() == other.Name.ToUpper();
        }
    }

    public class IFile : Composite
    {

        public IFile(string location, string filePath, string filter)
            : base(location, filePath)
        {
            this.Extension = Path.GetExtension(filePath).Replace(".", string.Empty).ToUpper();
            this.Name = Path.GetFileName(filePath);
            if (!filter.Contains(this.Extension.ToUpper())) return;

            this.Icon = Image.FromFile(Data.Location.Image + @"\drw_icon.gif");
            this.IsVaild = true;
        }

    }

    public class IFolder : Composite
    {
        public IFolder(string location, string filePath)
            : base(location, filePath)
        {
            if (filePath == "..")
                this.Location = Path.GetDirectoryName(this.Location);
            else
                this.Location += @"\" + this.Name;

            if (!Directory.Exists(this.Location)) return;

            this.Icon = Image.FromFile(Data.Location.Image + @"\dir_icon.gif");
            this.IsVaild = true;
        }
    }

    public class CompositeFactory
    {
        public List<Composite> List { get; set; }

        public CompositeFactory(string location, string filter, bool add_folder = true, bool add_file = true)
        {
            try
            {
                this.List = new List<Composite>();
                if (add_folder)
                {
                    this.AddFolders(location);
                }

                if (add_file)
                {
                    this.AddFiles(location, filter);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }


        private void AddFolders(string location)
        {
            IFolder folder = new IFolder(location, "..");
            if (folder.IsVaild) this.List.Add(folder);

            string[] names = Directory.GetDirectories(location);
            foreach (string name in names)
            {
                folder = new IFolder(location, name);
                if (folder.IsVaild)
                {
                    this.List.Add(folder);
                }
            }
        }

        private void AddFiles(string location, string filter)
        {
            string[] names = Directory.GetFiles(location);
            foreach (string name in names)
            {
                Util.IFile file = new IFile(location, name, filter);
                if (file.IsVaild && !this.List.Contains(file))
                {
                    this.List.Add(file);
                }
            }
        }
    }

    public class FolderBinder
    {
        public FolderBinder(string location, bool include_parent, ComboBox comboBox)
        {
            if (!Directory.Exists(location)) return;
            
            CompositeFactory factory = new CompositeFactory(location, "", true, false);
            if (!include_parent)
            {
                factory.List.Remove(factory.List.Find(item => item.Name == ".."));
            }

            comboBox.DataSource = null;
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "Name";
            comboBox.DataSource = factory.List;
        }

        public FolderBinder(string location, bool include_parent, DataGridViewComboBoxColumn column)
        {
            if (!Directory.Exists(location)) return;

            CompositeFactory factory = new CompositeFactory(location, "", true, false);
            if (!include_parent)
            {
                factory.List.Remove(factory.List.Find(item => item.Name == ".."));
            }

            column.DataSource = null;
            column.DisplayMember = "Name";
            column.ValueMember = "Name";
            column.DataSource = factory.List;
        }
        
        public FolderBinder(string location, bool include_parent, DataGridView dataGridView)
        {
            if (!Directory.Exists(location)) return;

            CompositeFactory factory = new CompositeFactory(location, "", true, false);
            if (!include_parent)
            {
                factory.List.Remove(factory.List.Find(item => item.Name == ".."));
            }

            dataGridView.DataSource = null;
            dataGridView.DataSource = factory.List;
        }
    }

    public class FileBinder
    {
        public FileBinder(string location, string filter, ComboBox comboBox)
        {
            CompositeFactory factory = new CompositeFactory(location, filter, false, true);
            comboBox.DataSource = null;
            comboBox.DisplayMember = "Name";
            comboBox.DataSource = factory.List;
        }
    }

    public class CompositeBinder
    {
        public CompositeBinder(string location, string filter, ComboBox comboBox)
        {
            if (!Directory.Exists(location)) return;

            CompositeFactory factory = new CompositeFactory(location, filter);
            comboBox.DataSource = null;
            comboBox.DisplayMember = "Name";
            comboBox.DataSource = factory.List;
        }

        public CompositeBinder(string location, string filter, DataGridView dataGridView)
        {

            if (!Directory.Exists(location)) return;

            CompositeFactory factory = new CompositeFactory(location, filter);
            dataGridView.DataSource = null;
            dataGridView.DataSource = factory.List;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Visible = false;
            }

            dataGridView.Columns[1].Visible = true;
            dataGridView.Columns[1].Width = 28;
            dataGridView.Columns[2].Visible = true;
            dataGridView.Columns[2].Width = 300;
        }
    }

}
