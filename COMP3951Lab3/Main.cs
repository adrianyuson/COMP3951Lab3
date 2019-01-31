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

/// <summary>
/// Purpose: A simple directory browser where you can also execute files.
/// Author: Adrian Yuson
/// Date: January 30, 2019
/// </summary>
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
                closeBrowserToolStripMenuItem.Enabled = false;
                viewToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        // Display the current directory's files and folder in the listView
        private void showDirectory(String directory)
        {
            try
            {
                showMainUI();

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
                Directory.SetCurrentDirectory(directory);
                currentPathTextField.Text = "Path: " + Directory.GetCurrentDirectory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        // Show the dialog box
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

        // Open folder or execute file upon double click
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string targetDirectory = Path.GetFileName(listView1.SelectedItems[0].Text);

                if (targetDirectory.Contains("..."))
                {
                    if (Directory.GetParent(Directory.GetCurrentDirectory().ToString()) != null)
                    {
                        listView1.Items.Clear();
                        showDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).ToString());
                    }
                }
                else if (Directory.Exists(targetDirectory))
                {
                    listView1.Items.Clear();
                    showDirectory(targetDirectory);
                }
                else
                {
                    System.Diagnostics.Process.Start(listView1.SelectedItems[0].Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        // Add icons in png files to our imageList
        private void addImages()
        {
            var imageList = new ImageList();
            imageList.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\directory-icon.png"));
            imageList.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\file-icon.png"));
            listView1.LargeImageList = imageList;
            listView1.SmallImageList = imageList;
            listView1.StateImageList = imageList;
        }
        
        // Display the progressBar status
        private void progressBarIncrement()
        {
            progressBar.Increment(-100);
            progressBar.Increment(100);
        }

        // Hide the main UI
        private void hideMainUI()
        {
            listView1.Hide();
            selectionLabel.Hide();
            currentPathTextField.Hide();
            closeBrowserToolStripMenuItem.Enabled = false;
            openBrowserToolStripMenuItem.Enabled = true;
            viewToolStripMenuItem.Enabled = false;
        }

        // Show the main UI
        private void showMainUI()
        {
            listView1.Show();
            selectionLabel.Show();
            currentPathTextField.Show();
            closeBrowserToolStripMenuItem.Enabled = true;
            openBrowserToolStripMenuItem.Enabled = false;
            viewToolStripMenuItem.Enabled = true;
        }
    }
}
