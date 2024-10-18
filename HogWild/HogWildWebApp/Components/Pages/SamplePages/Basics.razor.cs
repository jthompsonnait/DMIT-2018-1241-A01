using Microsoft.AspNetCore.Components;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    public partial class Basics
    {
        #region Privates
        //  used to display my name
        private string? myName;
        // the odd or even value
        private int oddEvenValue;
        #endregion

        #region Text Boxes
        //  email address
        private string emailText;
        //  password
        private string passwordText;
        //  date 
        private DateTime? dateText = DateTime.Today;
        #endregion

        #region Radio Buttons, Checkboxes & Text Area
        //  selected meal
        private string meal = "breakfast";
        private string[] meals = new string[] { "breakfast", "lunch", "dinner", "snacks" };
        //  used to hold the value of the acceptance
        private bool acceptanceBox;
        // used to hold the value for the message body
        private string messageBody;
        #endregion

        #region List and Sliders

        //  pretend that the following collection is data from a database
        //  The collection is based on a 2 property class called SelectionList
        //  The data for the list will be created in a separate method.
        /// <summary>
        /// Gets or sets a list of SelectionView objects representing various rides.
        /// </summary>
        private List<SelectionView> rides =new List<SelectionView>();

        /// <summary>
        /// Gets or sets the ID of the selected ride from the rides list.
        /// </summary>
        private int myRide;

        /// <summary>
        /// Gets or sets the user's chosen vacation spot as a string.
        /// </summary>
        private string vacationSpot;

        /// <summary>
        /// Gets or sets a list of strings representing various vacation spots.
        /// </summary>
        private List<string> vacationSpots;

        /// <summary>
        /// Gets or sets the review rating.
        /// </summary>
        //  The review rating
        private int reviewRating = 5;
        #endregion

        //  used to display any feedback to the end user.
        private string feedback;



        #region Methods
        // This method is automatically called when the component is initialized.
        protected override async Task OnInitializedAsync()
        {
            // Call the base class OnInitializedAsync method (if any).
            await base.OnInitializedAsync();

            // Call the 'RandomValue' method to perform custom initialization logic.
            RandomValue();

            // Call the 'PopulatedList' method to populate predefined data for the list.
            PopulatedList();
        }
        // Generates a random number between 0 and 25 using the Random class
        // Checks if the generated number is even
        // Sets the 'myName' variable to a message if even, or to null if odd
        private void RandomValue()
        {
            // Create an instance of the Random class to generate random numbers.
            Random rnd = new Random();

            // Generate a random integer between 0 (inclusive) and 25 (exclusive).
            oddEvenValue = rnd.Next(0, 25);

            // Check if the generated number is even.
            if (oddEvenValue % 2 == 0)
            {
                // If the number is even, construct a message with the number and assign it to 'myName'.
                myName = $"James is even {oddEvenValue}";
            }
            else
            {
                // If the number is odd, set 'myName' to null.
                myName = null;
            }

            // Trigger an asynchronous update of the component's state to reflect the changes made.
            InvokeAsync(StateHasChanged);
        }

        //  This method is called when a user submits text input.
        private void TextSubmit()
        {
            // Combine the values of emailText, passwordText, and dateText into a feedback message.
            feedback = $"Email {emailText}; Password {passwordText}; Date {dateText}";

            // Trigger a re-render of the component to reflect the updated feedback.
            InvokeAsync(StateHasChanged);
        }

        // Handle the selection of the loop meal
        private void HandleMealSelection(ChangeEventArgs e)
        {
            meal = e.Value.ToString();
        }

        //  This method is called when a user submits radio, check box and area text.
        private void RadioCheckAreaSubmit()
        {
            // Combine various values and store them in the 'feedback' variable as a formatted string.
            feedback = $"Meal {meal}; Acceptance {acceptanceBox}; Message {messageBody}";

            // Trigger a UI update to reflect the changes made to the 'feedback' variable.
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Populates the 'rides' list and 'vacationSpots' list with predefined data.
        /// </summary>
        private void PopulatedList()
        {
            int i = 1;

            // Create a pretend collection from the database representing different types
            // of transportation (rides).
            rides = new List<SelectionView>();
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Car" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Bus" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Bike" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Motorcycle" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Boat" });
            rides.Add(new SelectionView() { ValueID = i++, DisplayText = "Plane" });

            // Sort the 'rides' list alphabetically based on the 'DisplayText' property.
            rides.Sort((x, y) => x.DisplayText.CompareTo(y.DisplayText));

            // Initialize and populate the 'vacationSpots' list with predefined vacation destinations.
            vacationSpots = new List<string>();
            vacationSpots.Add("California");
            vacationSpots.Add("Caribbean");
            vacationSpots.Add("Cruising");
            vacationSpots.Add("Europe");
            vacationSpots.Add("Florida");
            vacationSpots.Add("Mexico");
        }

        /// <summary>
        /// This method is called when the user submits the list and slider form.
        /// It gathers user selections for 'myRide,' 'vacationSpot,' and 'reviewRating,'
        /// and generates feedback based on these selections.
        /// </summary>
        private void ListSliderSubmit()
        {
            // Generate feedback string incorporating the selected values.
            feedback = $"Ride {myRide}; Vacation {vacationSpot}; Review Rating {reviewRating}";

            // Invoke asynchronous method 'StateHasChanged' to trigger a re-render of the component.
            InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
