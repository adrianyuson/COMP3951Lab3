using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP3951Lab3
{
    public partial class Main : Form
    {
        string targetDirectory;

        public Main()
        {
            InitializeComponent();
        }

        private void openBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDialogBox();
        }

        private void DisplayFileSystemInfoAttributes(FileSystemInfo fsi)
        {
            //  Assume that this entry is a file.
            string entryType = "File";

            // Determine if entry is really a directory
            if ((fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                entryType = "Directory";
            }
            //  Show this entry's type, name, and creation date.
            Console.WriteLine("{0} entry {1} was created on {2:D}", entryType, fsi.FullName, fsi.CreationTime);
        }

        private void showCurrentDirectory(String directory)
        {
            string currentDirectoryName = directory;
            string parentDirectoryName = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));
            listView1.Items.Add(parentDirectoryName);
            DisplayFileSystemInfoAttributes(new DirectoryInfo(parentDirectoryName));

            //  Loop through all the immediate subdirectories of C.
            foreach (string entry in Directory.GetDirectories(currentDirectoryName))
            {              
                DisplayFileSystemInfoAttributes(new DirectoryInfo(entry));
                listView1.Items.Add(entry);
            }

            //  Loop through all the files in C.
            foreach (string entry in Directory.GetFiles(currentDirectoryName))
            {
                DisplayFileSystemInfoAttributes(new FileInfo(entry));
                listView1.Items.Add(entry);
            }


            labelCurrentPath.Text = currentDirectoryName;
        }

        public void ShowDialogBox()
        {
            Dialog dialog = new Dialog();
            //Console.WriteLine(dialog.ShowDialog());
            DialogResult = dialog.ShowDialog(this);

            if (DialogResult == DialogResult.Yes)
            {
                listView1.Items.Clear();
                targetDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                showCurrentDirectory(targetDirectory);
            }

            else if (DialogResult == DialogResult.No)
            {
                listView1.Items.Clear();
                targetDirectory = Directory.GetDirectoryRoot(Assembly.GetEntryAssembly().Location);
                showCurrentDirectory(targetDirectory);
            }

            else if (DialogResult == DialogResult.Cancel)
            {
                
            }
            dialog.Dispose();
        }

        private void ListView1_ItemActivate(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the ListView.ItemActivate event.");

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            targetDirectory = Path.GetFileName(listView1.SelectedItems[0].Text);
            Console.WriteLine(targetDirectory);
            if (!targetDirectory.Contains(".")) {
                
                listView1.Items.Clear();
                showCurrentDirectory(targetDirectory);
            }
            else
            {
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].Text);
            }


        }
    }
}
