using LabeledGallery.DatabaseSetting;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace LabeledGallery.Utils
{
    public class DocumentStoreHolder : IDisposable
    {
        private readonly IDocumentStore _ravendbStore;
        
        public DocumentStoreHolder(DatabaseSettings ravendbSettings)
        {
            _ravendbStore = CreateRavenDbStore(ravendbSettings);
        }

        private static DocumentStore CreateRavenDbStore(DatabaseSettings databaseSettings)
        {
            var documentStore = new DocumentStore
            {
                Urls = databaseSettings.Urls,
                Database = databaseSettings.DatabaseName
            };
            
            documentStore.Initialize();

            return documentStore;
        }
        
        public IAsyncDocumentSession OpenAsyncSession() => _ravendbStore.OpenAsyncSession();
        
        public void Dispose()
        {
            _ravendbStore?.Certificate?.Dispose();
            _ravendbStore?.Dispose();
        }
    }
}