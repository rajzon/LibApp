using System.Collections.Generic;

namespace EventBus.Messages.Commands
{
    public class RestoreCachedStock
    {
        public string Ean { get; init; }
        public int StockToRestore { get; init; }
    }
}