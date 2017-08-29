using CsvHelper.Configuration;

namespace PriceUpdater.Models
{
    public class UpdatedPriceData
    {
        public string Product { get; set; }

        public string CostP1 { get; set; }
    }

    public class UpdatedPriceDataMap : CsvClassMap<UpdatedPriceData>
    {
        public UpdatedPriceDataMap()
        {
            Map(m => m.Product);
            Map(m => m.CostP1);
        }
    }
}
