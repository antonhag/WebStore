using WebStore.Models;

namespace WebStore;

public static class Session
{
    public static Customer? CurrentCustomer { get; set; }
    public static DeliveryOption SelectedDeliveryOption { get; set; }
    public static string? TemporaryShippingAddress { get; set; }
    public static bool IsShippingAddressChanged { get; set; }
    public static Admin? CurrentAdmin { get; set; }

    public static void Logout()
    {
        CurrentCustomer = null;
        CurrentAdmin = null;
    }
}