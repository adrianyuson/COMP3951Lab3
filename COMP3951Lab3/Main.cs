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


        public Main()
        {
            try
            {
                InitializeComponent();
                addImages();
                hideMainUI();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }         
        }

        private void openBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDialogBox();
        }

        private void closeBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideMainUI();
        }

        private void showDirectory(String directory)
        {
            showMainUI();
            //Console.WriteLine(Assembly.GetEntryAssembly().Location);
            
            string currentDirectoryName = directory;
            
            string parentDirectoryName = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            listView1.Items.Add("...");

            //  Loop through all the immediate subdirectories of C.
            foreach (string entry in Directory.GetDirectories(currentDirectoryName))
            {              
                listView1.Items.Add(Path.GetFileName(entry), 0);
            }

            //  Loop through all the files in C.
            foreach (string entry in Directory.GetFiles(currentDirectoryName))
            {
                listView1.Items.Add(Path.GetFileName(entry), 1);
            }
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine(currentDirectoryName);
            Directory.SetCurrentDirectory(directory);
            currentPathTextField.Text = currentDirectoryName;
        }

        public void ShowDialogBox()
        {
            string targetDirectory;
            Dialog dialog = new Dialog();
            DialogResult = dialog.ShowDialog(this);

            if (DialogResult == DialogResult.Yes)
            {
                progressBarIncrement();
                listView1.Items.Clear();
                targetDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                showDirectory(targetDirectory);
            }

            else if (DialogResult == DialogResult.No)
            {
                progressBarIncrement();
                listView1.Items.Clear();
                targetDirectory = Path.GetPathRoot(Assembly.GetEntryAssembly().Location);
                showDirectory(targetDirectory);
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
            string targetDirectory = Path.GetFileName(listView1.SelectedItems[0].Text);

            if (targetDirectory.Contains("..."))
            {
                if(Directory.GetCurrentDirectory() != Directory.GetParent(targetDirectory).ToString())
                {
                    listView1.Items.Clear();
                    showDirectory(Directory.GetParent(targetDirectory).ToString());
                }
            }
            else if (!targetDirectory.Contains(".")) {
                listView1.Items.Clear();
                showDirectory(targetDirectory);
            }
            else
            {
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].Text);
            }
        }

        private void tiledefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }

        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void addImages()
        {
            var imageList = new ImageList();
            imageList.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\directory-icon.png"));
            imageList.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\file-icon.png"));
            listView1.LargeImageList = imageList;
            listView1.SmallImageList = imageList;
            listView1.StateImageList = imageList;
        }
        
        private void progressBarIncrement()
        {
            progressBar.Increment(-100);
            progressBar.Increment(100);
        }

        private void hideMainUI()
        {
            listView1.Hide();
            selectionLabel.Hide();
            currentPathTextField.Hide();
        }

        private void showMainUI()
        {
            listView1.Show();
            selectionLabel.Show();
            currentPathTextField.Show();
        }
    }
}
