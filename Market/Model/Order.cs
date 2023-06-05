namespace Market.Model
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
