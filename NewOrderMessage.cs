namespace PurchaseWebHook;

public partial class ProcessNewOrder
{
    public record NewOrderMessage(int productId, string productName, int quantity, string customerName, double price);
}