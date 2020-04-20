using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace PMO.API.Test.Repository
{
    [CollectionDefinition("RepoUnitTest")]
    public class RepoTestCollection:ICollectionFixture<DocumentStoreClassFixture>
    {
    }
}
