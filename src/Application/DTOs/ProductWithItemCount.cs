namespace Application.DTOs
{
    public class ProductWithItemCount
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ItemCount { get; set; }  // Count of related items
        public int TotalQuantity { get; set; }  // Total quantity from related items
    }
}