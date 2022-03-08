using System.Collections.Generic;

namespace EventBus.Messages.Commands
{
    public class DeleteStocks
    {
        public List<int> StocksIds { get; set; }
        public Dictionary<string, List<int>> EanWithStocks { get; set; }
    }
}