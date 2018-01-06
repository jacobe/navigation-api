using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace NavigationApi.Test.Persistance.CosmosDb
{
    public static class LocalEmulatorHelper
    {
        public static IDocumentClient CreateDocumentClient()
        {
            return new DocumentClient(
                new Uri("https://localhost:8081"),
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        }
    }
}