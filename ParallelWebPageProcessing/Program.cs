using Microsoft.Playwright;
using Serilog;
using System.Diagnostics;

class ParallelWebPageProcessing
{
    static void Main(string[] args)
    {
        int threadCount = Process.GetCurrentProcess().Threads.Count;
        Console.WriteLine($"Número de hilos: {threadCount}");

        List<string> urls = new List<string>
        {
            "https://www.example.com",
            "https://www.chromium.org/Home/",
            "https://www.microsoft.com",
            // Agrega más URLs aquí...
        };

        var results = GetPageTitlesParallel(urls, threadCount).GetAwaiter().GetResult();

        foreach (var result in results)
        {
            Log.Information($"Título de la página: {result.Title}");
            Log.Information($"Contenido del elemento h1: {result.ElementContent}");
            Log.Information("");
        }

        Log.CloseAndFlush();
    }

    static async Task<List<PageResult>> GetPageTitlesParallel(List<string> urls, int maxDegreeOfParallelism)
    {
        var results = new List<PageResult>();

        using var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        var browser = await playwright.Chromium.LaunchAsync();

        var context = browser.NewContextAsync().GetAwaiter().GetResult();

        try
        {
            Parallel.ForEach(urls, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, (url) =>
            {
                var pageResult = new PageResult { Url = url };
                try
                {
                    var page = context.NewPageAsync().GetAwaiter().GetResult();
                    page.GotoAsync(url).GetAwaiter().GetResult();

                    pageResult.Title = page.TitleAsync().GetAwaiter().GetResult();
                    pageResult.ElementContent = GetAndPrintElementContent(page, "h1");

                    Console.WriteLine($"url {url}");
                    Console.WriteLine($"Title: {pageResult.Title}");
                    Console.WriteLine($"Content h1: {pageResult.ElementContent}");

                    page.CloseAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Log.Error($"Error al procesar la URL {url}: {ex.Message}");
                }
                finally
                {
                    results.Add(pageResult);
                }
            });
        }
        catch (AggregateException ex)
        {
            foreach (var innerException in ex.InnerExceptions)
            {
                Log.Error($"Error en el procesamiento en paralelo: {innerException.Message}");
            }
        }
        finally
        {
            browser.CloseAsync().GetAwaiter().GetResult();
        }
        return results;
    }

    static string GetAndPrintElementContent(IPage page, string selector)
    {
        var elementHandle = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
        var content = elementHandle?.InnerTextAsync().GetAwaiter().GetResult();
        content ??= "Elemento no encontrado"; // Valor predeterminado en caso de que elementHandle sea nulo

        Log.Information($"Contenido del elemento {selector}: {content}");
        return content;
    }
}

class PageResult
{
    public string Url { get; set; }
    public string Title { get; set; }
    public string ElementContent { get; set; }
}
