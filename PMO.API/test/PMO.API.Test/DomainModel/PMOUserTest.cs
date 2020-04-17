using PMO.API.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace PMO.API.Test.DomainModel
{
    public class PMOUserTest
    {
        [Fact]
        public void GetterSetter()
        {
            var pmoUser = new PMOUser
            {
                EmployeeId = "E001",
                FirstName = "First",
                LastName = "Last",
                Id = "PMOUser/1"
            };
            Assert.Equal("E001", pmoUser.EmployeeId);
            Assert.Equal("First", pmoUser.FirstName);
            Assert.Equal("Last", pmoUser.LastName);
            Assert.Equal("PMOUser/1", pmoUser.Id);
        }
    }
}
