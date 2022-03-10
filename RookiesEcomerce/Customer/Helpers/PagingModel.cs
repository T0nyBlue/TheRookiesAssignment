namespace Customer.Helpers
{
    public class PagingModel
    {
        public int curentpage { get; set; }
        public int countpage { get; set; }
        public Func<int?, string> generateUrl { get; set; }
    }
}
