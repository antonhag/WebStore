using WebStore.Models;

namespace WebStore;

public static class Session
{
    public static Customer? CurrentCustomer { get; set; }
}