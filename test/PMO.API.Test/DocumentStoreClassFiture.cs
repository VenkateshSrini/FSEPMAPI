using Raven.Client.Documents;
using Raven.TestDriver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMO.API.Test
{
    public class DocumentStoreClassFixture : RavenTestDriver
    {
        public DocumentStoreClassFixture()
        {
            prepareDocumentStore();
        }
        public IDocumentStore Store { get; private set; }
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }
        private void prepareDocumentStore()
        {
            ConfigureServer(new TestServerOptions
            {
                DataDirectory = @".\RavenDBTestDir"
            });
            Store = GetDocumentStore();
            

        }
    }
}
