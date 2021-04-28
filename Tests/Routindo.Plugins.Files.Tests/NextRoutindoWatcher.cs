namespace Routindo.Plugins.Files.Tests
{
    using System;
    using Routindo.Contract.Services;
    using Routindo.Contract.Watchers;
    public class NextRoutindoWatcher: IWatcher
    {
        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }
        public WatcherResult Watch()
        {
            throw new NotImplementedException();
        }
    }
}
