using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    public partial class CustomerEdit
    {
        #region Fields

        // The Customer
        private CustomerEditView customer = new();
        //  The provinces
        private List<LookupView> provinces = new();
        //  The countries
        private List<LookupView> countries = new();
        //  The status lookup
        private List<LookupView> statusLookup = new();

        // close button text
        private string closeButtonText = "Close";
        // close button color
        private Color closeButtonColor = Color.Success;
        
        // disable the save button if customer is not modified or validate (we have errors)
        private bool disableSaveButton => !editContext.IsModified() || !editContext.Validate();
        // the edit context
        private EditContext editContext;
        //  used to store the validation message
        private ValidationMessageStore messageStore;

        /// <summary>
        /// The show dialog
        /// </summary>
        private bool showDialog = false;
        /// <summary>
        /// The dialog message
        /// </summary>
        private string dialogMessage = string.Empty;
        /// <summary>
        /// The dialog completion source
        /// </summary>
        private TaskCompletionSource<bool?> dialogCompletionSource;

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        private async Task ShowDialog()
        {
            dialogMessage = "Do you wish to close the invoice editor?";
            showDialog = true;
        }


        #endregion

        #region Feedback & Error Message
        // The feedback message
        private string feedbackMessage;

        // The error message
        private string errorMessage;

        // has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        // has error
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);

        // error details
        private List<string> errorDetails = new();
        #endregion
        #region Properties

        //  The customer service
        [Inject] protected CustomerService CustomerService { get; set; }
        //  The category lookup service
        [Inject] protected CategoryLookupService CategoryLookupService { get; set; }

        // Injects the NavigationManager dependency.
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        // The dialog service
        [Inject]
        protected IDialogService DialogService { get; set; }

        //  Customer ID used to create or edit a customer
        [Parameter] public int CustomerID { get; set; } = 0;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                //  edit context needs to be setup after data has been initialized
                //  setup of the edit context to make use of the payment type property
                editContext = new EditContext(customer);

                //  set the validation to use the HandleValidationRequest event
                editContext.OnValidationRequested += HandleValidationRequested;

                //  setup the message store to track any validation messages
                messageStore = new ValidationMessageStore(editContext);

                //  this event will fire each time the data in a property has change.
                editContext.OnFieldChanged += EditContext_OnFieldChanged;

                // check to see if we are navigating using a valid customer CustomerID
                //      or arw we going to create a new customer
                if (CustomerID > 0)
                {
                    customer = CustomerService.GetCustomer(CustomerID);
                }

                // lookups
                provinces = CategoryLookupService.GetLookups("Province");
                countries = CategoryLookupService.GetLookups("Country");
                statusLookup = CategoryLookupService.GetLookups("Customer Status");

                await InvokeAsync(StateHasChanged);
            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }

                errorMessage = $"{errorMessage}Unable to search for customer";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        // Handles the validation requested.        
        private void HandleValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            //  clear the message store if there is any existing validation errors.
            messageStore?.Clear();

            //  custom validation logic
            //  first name is required
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                messageStore?.Add(() => customer.FirstName, "First Name is required!");
            }
            //  last name is required
            if (string.IsNullOrWhiteSpace(customer.LastName))
            {
                messageStore?.Add(() => customer.LastName, "Last Name is required!");
            }
            //  phone is required
            if (string.IsNullOrWhiteSpace(customer.Phone))
            {
                messageStore?.Add(() => customer.Phone, "Phone is required!");
            }
            //  email is required
            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                messageStore?.Add(() => customer.Email, "Email is required!");
            }
        }

        // Handles the OnFieldChanged event of the EditContext control.
        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            //  the "editContext.Validate()" should not be needed
            //    but if the "HandleValidationRequested" does not fire on it own
            //    you will need to add it.  
            editContext.Validate();
            closeButtonText = editContext.IsModified() ? "Cancel" : "Close";
            closeButtonColor = editContext.IsModified() ? Color.Warning : Color.Default;
        }

        private void Save()
        {
            try
            {
                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = string.Empty;

                customer = CustomerService.Save(customer);
                feedbackMessage = "Data was successfully saved!";

            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }
                errorMessage = $"{errorMessage}Unable to search for customer";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        private async Task Cancel()
        {
            // Initialize the TaskCompletionSource
            dialogCompletionSource = new TaskCompletionSource<bool?>();
            dialogMessage = "Do you wish to close the customer editor?";
            showDialog = true;
            bool? results = await ShowDialogAsync();
            if ((bool)results)
            {
                NavigationManager.NavigateTo($"/SamplePages/CustomerList");
            }
        }

        /// <summary>
        /// Show dialog as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
        private async Task<bool?> ShowDialogAsync()
        {
            // Initialize the TaskCompletionSource
            dialogCompletionSource = new TaskCompletionSource<bool?>();

            // Wait for the dialog to be closed and return the result
            return await dialogCompletionSource.Task;
        }

        /// <summary>
        /// Simples the dialog result.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        private void SimpleDialogResult(bool? result)
        {
            showDialog = false;
            dialogCompletionSource.SetResult(result);
        }
    }
}
