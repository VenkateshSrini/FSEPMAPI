using PMO.API.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace PMO.API.Test.Messages
{
    public class UserMsgTest
    {
        [Fact]
        public void GetterSetterTest()
        {
            var userMsg = new UserMsg
            {
                EmployeeId="E001",
                FirstName="F1",
                LastName = "L1"
            };
            Assert.Equal("E001", userMsg.EmployeeId);
            Assert.Equal("F1", userMsg.FirstName);
            Assert.Equal("L1", userMsg.LastName);
        }
    }
}
