using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly Random random = new Random();
    private const string LotId = "63886158";

    static async Task Main(string[] args)
    {
        Console.WriteLine($"🔹 Начинаем работу с лотом #{LotId}: {DateTime.Now:HH:mm:ss}");
        
        var cookie = Environment.GetEnvironmentVariable("FUNPAY_COOKIE");
        if (string.IsNullOrEmpty(cookie))
        {
            Console.WriteLine("❌ Ошибка: не найден cookie в переменных окружения");
            return;
        }

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Cookie", cookie);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");

            try
            {
                Console.WriteLine($"🔄 Поднимаем лот #{LotId}...");
                
                var content = new StringContent($"id={LotId}&game_id=106&node_id=288", 
                    Encoding.UTF8, "application/x-www-form-urlencoded");
                
                var response = await client.PostAsync("https://funpay.com/lots/raise", content);
                var responseText = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ Лот #{LotId} успешно поднят! {DateTime.Now:HH:mm:ss}");
                }
                else
                {
                    Console.WriteLine($"❌ Ошибка {response.StatusCode}: {responseText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Критическая ошибка: {ex.Message}");
            }
        }
        
        Console.WriteLine("✅ Лот обработан");
    }
}
