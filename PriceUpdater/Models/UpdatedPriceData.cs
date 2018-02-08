using CsvHelper.Configuration;

namespace PriceUpdater.Models
{
    public class UpdatedPriceData
    {
        public string Product { get; set; }

        public string CostP1 { get; set; }

        public string Markup1 { get; set; }

        public string Markup2 { get; set; }

        public string Markup3 { get; set; }

        public string Markup4 { get; set; }

        public string Sellunit { get; set; }

        public string Barcode { get; set; }

        public string SuppliersCode { get; set; }
    }

    public class UpdatedPriceDataMap : CsvClassMap<UpdatedPriceData>
    {
        public UpdatedPriceDataMap()
        {
            Map(m => m.Product);
            Map(m => m.CostP1);
            Map(m => m.Markup1);
            Map(m => m.Markup2);
            Map(m => m.Markup3);
            Map(m => m.Markup4);
            Map(m => m.Sellunit);
            Map(m => m.Barcode);
            Map(m => m.SuppliersCode);
        }
    }
}
