using Microsoft.Extensions.Logging;
using PMO.API.DomainModel;
using PMO.API.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMO.API.Test.Repository
{
    public class ProjectRepoClassFixture:DocumentStoreClassFixture
    {
        public ProjectRepoClassFixture() : base(@".\RavenDBTestDirProj")
        { }
       
    }
}
