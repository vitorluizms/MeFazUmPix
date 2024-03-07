namespace MyWallet.Services
{
    public class HealthService
    {
        public HealthService()
        {
            Console.WriteLine("HealthService created");
        }

        public string GetHealth()
        {
            return "Healthy";
        }

    }
}