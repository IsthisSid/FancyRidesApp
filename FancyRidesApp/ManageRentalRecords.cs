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
    public partial class ManageRentalRecords : Form
    {
       
        private readonly FancyRidesEntities fancyRidesEntities;
        public ManageRentalRecords()
        {
            InitializeComponent();
            fancyRidesEntities = new FancyRidesEntities();
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void PopulateGrid()
        {
            var records = fancyRidesEntities.FancyRidesRecords.Select(q => new
            {
                Customer = q.CustomerName,
                DateRented = q.DateRented,
                DateReturned = q.DateReturned,
                ID = q.id,
                q.Cost,
                Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model

            }).ToList();

            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateRented"].HeaderText = "Date Rented";
            gvRecordList.Columns["DateReturned"].HeaderText = "Date Returned";
            gvRecordList.Columns["ID"].Visible = false;


        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord();
            addRentalRecord.MdiParent = this.MdiParent;
            addRentalRecord.Show();
            Text = "Add Rental Record";  
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //Get ID of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["ID"].Value;

                //Query database for record
                var record = fancyRidesEntities.FancyRidesRecords.FirstOrDefault(q => q.id == id);

                //Launch add/edit RentalRecord window with data
                var addEditRentalRecord = new AddEditRentalRecord(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
                Text = "Edit Rental Record";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // Get ID of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["ID"].Value;

                // Query database for record
                var record = fancyRidesEntities.FancyRidesRecords.FirstOrDefault(q => q.id == id);

                // Delete vehicle from the table
                fancyRidesEntities.FancyRidesRecords.Remove(record);
                fancyRidesEntities.SaveChanges();

                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; -throw ends the program so we can comment this out or delete it
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
       

    }
}
