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
        private readonly FancyRidesEntities fancyRidesEntities;

        
        public AddEditVehicle()
        {
            InitializeComponent();
            Text = "Add Vehicle";
            lblTitle.Text = "Add Vehicle";
            isEditMode = false;
            fancyRidesEntities = new FancyRidesEntities();
        }

        public AddEditVehicle(TypesOfCar carToEdit)
        {
            InitializeComponent();
            Text = "Edit Vehicle";
            lblTitle.Text = "Edit Vehicle";
            PopulateFields(carToEdit);
            isEditMode = true;
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
                    isValid = false;
                    errorMessage += "Error: Please enter make, model, and year.\n\r"; // note '\n\r' is a line break in a string
                }

                //Message box popup once the user completes validation checks
                if (isValid)
                {
                    MessageBox.Show("Operation successful. Click Refresh grid to see changes.\n\r");
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }

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

                    fancyRidesEntities.SaveChanges();
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
                    fancyRidesEntities.SaveChanges();
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
