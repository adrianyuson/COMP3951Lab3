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
            using (Dialog dialog = new Dialog())
            {
                if (dialog.ShowDialog() == DialogResult.Yes)
                {
                    targetDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    showCurrentDirectory(targetDirectory);
                }

                if (dialog.ShowDialog() == DialogResult.No)
                {
                    targetDirectory = Directory.GetDirectoryRoot(Assembly.GetEntryAssembly().Location);
                    Console.WriteLine(targetDirectory);
                    showCurrentDirectory(targetDirectory);
                }
            }
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
            string parentDirectoryName = "...\\" + Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));
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

    }
}
