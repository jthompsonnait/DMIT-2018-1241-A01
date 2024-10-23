using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    public partial class WorkingVersions
    {
        #region Fields
        //  This private field holds a reference to the WorkingVersionsView instance.
        private WorkingVersionsView workingVersionsView = new WorkingVersionsView();

        private string feedback = string.Empty;
        #endregion
        #region Properties
        //  This attribute marks the property for dependency injection.
        [Inject]
        //  This property provides access to the WorkingVersionsService service;
        protected WorkingVersionsService WorkingVersionsService { get; set; }
        #endregion

        #region Methods

        private void GetWorkingVersions()
        {
            feedback = string.Empty;
            try
            {
                workingVersionsView = WorkingVersionsService.GetWorkingVersion();
            }
            #region catch all exception
            catch (AggregateException ex)
            {
                foreach (var error in ex.InnerExceptions)
                {
                    feedback = error.Message;
                }
            }
            catch (ArgumentNullException ex)
            {
                feedback = GetInnerException(ex).ToString();
            }

            catch (Exception ex)
            {
                feedback = GetInnerException(ex).ToString();
            }
            #endregion
        }


        public Exception GetInnerException(System.Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
        #endregion
    }
}
