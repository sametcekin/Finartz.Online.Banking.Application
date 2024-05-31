namespace API.Models.Requests.Accounts;

public class CreateAccountRequest
{
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public decimal Balance { get; set; }
}
