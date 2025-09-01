using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly Random random = new Random();
    private static readonly string[] lotIds = { "51103802", "51102485", "39991936" };

    static async Task Main(string[] args)
    {
        Console.WriteLine($"🔹 Начинаем поднятие лотов: {DateTime.Now:HH:mm:ss}");
        
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

            foreach (var lotId in lotIds)
            {
                try
                {
                    Console.WriteLine($"🔹 Поднимаем лот #{lotId}");
                    
                    var content = new StringContent($"id={lotId}&game_id=106&node_id=288", 
                        Encoding.UTF8, "application/x-www-form-urlencoded");
                    
                    var response = await client.PostAsync("https://funpay.com/lots/raise", content);
                    var responseText = await response.Content.ReadAsStringAsync();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"✅ Лот #{lotId} поднят!");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Лот #{lotId}: ошибка {response.StatusCode}");
                    }
                    
                    // Случайная задержка между лотами
                    var delay = random.Next(120, 180);
                    Console.WriteLine($"⏳ Ожидаем {delay} секунд...");
                    await Task.Delay(TimeSpan.FromSeconds(delay));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Ошибка лота #{lotId}: {ex.Message}");
                }
            }
        }
        
        Console.WriteLine("✅ Все лоты обработаны!");
    }
}
