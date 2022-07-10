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
    public partial class ManageVehicleListing : Form
    {
        private readonly FancyRidesEntities fancyRidesEntities;


        public ManageVehicleListing()
        {
            InitializeComponent();
            fancyRidesEntities = new FancyRidesEntities();

            //var cars = fancyRidesEntities.TypesOfCars.ToList();  // displays entire list

            //Select id as CarID, name as CarName from TypesOfCars
            //Specify what you want displayed; aliasing inside LINQ using lambda expression(=>)
            //var cars = fancyRidesEntities.TypesOfCars
            //    .Select(q => new { CarID = q.ID, CarMake = q.Make })
            //    .ToList(); 

        var cars = fancyRidesEntities.TypesOfCars
                    .Select(q => new
                    {
                        Make = q.Make,
                        Model = q.Model,
                        VIN = q.VIN,
                        Year = q.Year,
                        LicensePlateNumber = q.LicensePlateNumber,
                        q.ID
                    }).ToList();

                gvVehicleList.DataSource = cars;

                //Customizing the names of your columns
                gvVehicleList.Columns[4].HeaderText = "License Plate Number"; // instead of LicensePlateNumber
                gvVehicleList.Columns[5].Visible = false;
            }

        private void btnAddCar_Click(object sender, EventArgs e)
            {
                var addEditVehicle = new AddEditVehicle();
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }

        private void btnEditCar_Click(object sender, EventArgs e)
            {
            try
            {

                // Get ID of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["ID"].Value;

                // Query database for record
                var car = fancyRidesEntities.TypesOfCars.FirstOrDefault(q => q.ID == id);

                //Launch AddEditVehicle window with data
                var addEditVehicle = new AddEditVehicle(car);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; -throw ends the program so we can comment this out or delete it
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
            {
            try
            {
                // Get ID of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["ID"].Value;

                // Query database for record
                var car = fancyRidesEntities.TypesOfCars.FirstOrDefault(q => q.ID == id);

                // Delete vehicle from the table
                fancyRidesEntities.TypesOfCars.Remove(car);
                fancyRidesEntities.SaveChanges();

                gvVehicleList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; -throw ends the program so we can comment this out or delete it
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
            {
                PopulateGrid();
            }


        private void PopulateGrid()
            {
                // Select a custom model collection of cars from database
                var cars = fancyRidesEntities.TypesOfCars
                    .Select(q => new
                    {
                        Make = q.Make,
                        Model = q.Model,
                        VIN = q.VIN,
                        Year = q.Year,
                        LicensePlateNumber = q.LicensePlateNumber,
                        q.ID
                    })
                    .ToList();
                gvVehicleList.DataSource = cars;
                gvVehicleList.Columns[4].HeaderText = "License Plate Number";
                //Hide the column for ID. Changed from the hard coded column value to the name, 
                // to make it more dynamic. 
                gvVehicleList.Columns["ID"].Visible = false;
            }
        }

    }

