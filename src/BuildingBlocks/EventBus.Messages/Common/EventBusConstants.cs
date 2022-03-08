namespace EventBus.Messages.Common
{
    public class EventBusConstants
    {
        public const string CreateBookQueue = "createdbook-queue";  
        public const string AddImageToBookQueue = "addimagetobook-queue";
        public const string CreateSeededCustomersQueue = "createseededcustomers-queue";
        
        
        public const string CheckBooksExistance = "checkbookexistance-queue";
        public const string GetBooksInfo = "getbooksinfo-queue";
        public const string GetBookInfo = "getbookinfo-queue";
        public const string GetCustomerInfo = "getcustomerinfo-queue";
        public const string GetStockWithBookInfo = "getstockwithbookinfo-queue";
        public const string DeleteStocks = "deletestocks-queue";
        public const string RestoreCachedStock = "restorecachedstock-queue";
        
    }
}