using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FancyRidesApp
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord();
            addRentalRecord.ShowDialog();
            addRentalRecord.MdiParent = this; 

        //Having created the addRentalRecord object to display, we want to keep this new window inside the parent 'this' which is class 'MainWindow'
        //MainWindow is the MDI container; by placing ShowDialog above the MdiParent declaration,
        //the user cannot perform any actions outside the active window (add rental record) unless you exit the window
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Another alternative to ShowDialog using LINQ
            var OpenForms = Application.OpenForms.Cast<Form>(); // get list of openforms
            var isOpen = OpenForms.Any(q => q.Name == "ManageVehicleListing");
            if (!isOpen)
            {
                var manageVehicleListing = new ManageVehicleListing();
                manageVehicleListing.MdiParent = this;
                manageVehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var manageRentalRecords = new ManageRentalRecords();
            manageRentalRecords.MdiParent = this;
            manageRentalRecords.Show();
        }
    }
}
