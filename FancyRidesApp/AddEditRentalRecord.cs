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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;

        //Declare an object that represents our data model; 
        private readonly FancyRidesEntities fancyRidesEntities; //Establishes an instance of a connection to the database through the declaration of this property

        public AddEditRentalRecord()
        {
            InitializeComponent();
            Text = "Add Rental Record";
            lblTitle.Text = "Add Rental Record";
            isEditMode = false;
            fancyRidesEntities = new FancyRidesEntities();
        }

        public AddEditRentalRecord(FancyRidesRecord recordToEdit)
        {
            InitializeComponent();
            Text = "Edit Rental Record";
            lblTitle.Text = "Edit Rental Record";
            
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                fancyRidesEntities = new FancyRidesEntities();
                PopulateFields(recordToEdit);

            }
        }

        private void PopulateFields(FancyRidesRecord recordToEdit)
        {
            lblRecordID.Text = recordToEdit.id.ToString();
            tbCustomerName.Text = recordToEdit.CustomerName;
            datePickedRented.Value = (DateTime)recordToEdit.DateRented;
            datePickedReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            //Exception Handling - place all code in try catch block
            try
            {
                //Properties:
                var customerName = tbCustomerName.Text;
                var dateRented = datePickedRented.Value;
                var dateReturned = datePickedReturned.Value;
                var cost = Convert.ToDouble(tbCost.Text);
                var carType = comboBoxItemSelected.Text;

                var isValid = true;
                var errorMessage = "";

                //Form validation checks for invalid user responses
                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Error: Please enter missing data.\n\r"; // note '\n\r' is a line break in a string
                }

                if (dateRented > dateReturned)
                {
                    isValid = false;
                    errorMessage += "Error: Invalid date selection.\n\r";
                }

                //If all responses are valid, new customer record will be saved onto database, and pop up will display all information
                //Else pop up will display all error messages
                if (isValid)

                {
                    //Declare an object of the record to be added
                    var rentalRecord = new FancyRidesRecord();
                    if (isEditMode)
                    {
                        //If EditMode == true, then retrieve the record from the database and
                        //place the result in the record object
                        var id = int.Parse(lblRecordID.Text);
                        rentalRecord = fancyRidesEntities.FancyRidesRecords.FirstOrDefault(q => q.id == id);
                    }

                    //Populate record object with values from the form
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateRented;
                    rentalRecord.DateReturned = dateReturned;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)comboBoxItemSelected.SelectedValue;

                    //If not EditMode then we want to perform the step of adding the rental object to the database
                    if (!isEditMode)
                        fancyRidesEntities.FancyRidesRecords.Add(rentalRecord);

                    //Save changes made to the entity
                    fancyRidesEntities.SaveChanges();

                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                        $"Date Rented: {dateRented}\n\r" +
                        $"Date Returned: {dateReturned}\n\r" +
                        $"Vehicle Selected: {carType}\n\r" +
                        $"Cost: ${cost}\n\r" +
                        "Thank you for your submission.");

                    Close();

                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; -throw ends the program so we can comment this out or delete it
            }
        }

        //Form Load Event Block:
        private void Form1_Load(object sender, EventArgs e)
        {
            //Run a query that retrieves the types of cars and binds them to the list of items available inside our ComboBox
            //var cars = fancyRidesEntities.TypesOfCars.ToList(); 

            var cars = fancyRidesEntities.TypesOfCars
                .Select(q => new 
                { 
                    Id = q.ID, Name = q.Make + " " + q.Model 
                }).ToList();
            comboBoxItemSelected.DisplayMember = "Name"; //Display the names
            comboBoxItemSelected.ValueMember = "ID"; //Store the ids
            comboBoxItemSelected.DataSource = cars; //List of cars with Name and id should be the source for the list of items coming into this combo box
        }
    }
}
