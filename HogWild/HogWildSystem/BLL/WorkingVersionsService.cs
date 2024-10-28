#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HogWildSystem.DAL;
using HogWildSystem.Entities;
using HogWildSystem.ViewModels;

namespace HogWildSystem.BLL
{
    public  class WorkingVersionsService
    {
        private readonly HogWildContext _hogWildContext;

        //  Constructor for the WorkingVersionsService class.
        internal WorkingVersionsService(HogWildContext hogWildContext)
        {
            _hogWildContext = hogWildContext;
        }

        public WorkingVersionsView GetWorkingVersion()
        {
            return _hogWildContext.WorkingVersions
                .Select(x => new WorkingVersionsView
                {
                    VersionID = x.VersionId,
                    Major = x.Major,
                    Minor = x.Minor,
                    Build = x.Build,
                    Revision = x.Revision,
                    AsOfDate = x.AsOfDate,
                    Comments = x.Comments
                }).FirstOrDefault();
        }
    }
}
