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
    public partial class AddEditVehicle : Form

    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;


        private readonly FancyRidesEntities fancyRidesEntities;

        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            Text = "Add Vehicle";
            lblTitle.Text = "Add Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            fancyRidesEntities = new FancyRidesEntities();
            
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            Text = "Edit Vehicle";
            lblTitle.Text = "Edit Vehicle";
            PopulateFields(carToEdit);
            isEditMode = true;
            _manageVehicleListing = manageVehicleListing;
            fancyRidesEntities = new FancyRidesEntities();
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.ID.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicensePlate.Text = car.LicensePlateNumber;
        }


        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                //Validation Properties
                var make = tbMake.Text;
                var model = tbModel.Text;
                var year = tbYear.Text;
                var isValid = true;
                var errorMessage = "";

                //Form validation checks for invalid user responses
                if (string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(year))
                {
                    MessageBox.Show("Please provide the make, model and year.");
                }
                else
                {
                    //Add/Edit functionality

                    if (isEditMode)
                    {
                        //Edit vehicle code here
                        var id = int.Parse(lblId.Text);
                        var car = fancyRidesEntities.TypesOfCars.FirstOrDefault(q => q.ID == id);
                        car.Model = tbModel.Text;
                        car.Make = tbModel.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLicensePlate.Text;
                    }
                    else
                    {
                        //Add vehicle code here
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = tbLicensePlate.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text)
                        };

                        fancyRidesEntities.TypesOfCars.Add(newCar);
                    }

                    //At the end, we want to save changes, populate the grid, pop up a messagebox stating success, then close
                    fancyRidesEntities.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation successful.\n\r");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; -throw ends the program so we can comment this out or delete it
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
