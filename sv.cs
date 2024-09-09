using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Definir o endereço base do servidor
        string baseAddress = "http://localhost:8080/";

        // Criar um HttpListener
        using (HttpListener listener = new HttpListener())
        {
            // Adicionar prefixo de URL onde o servidor vai escutar
            listener.Prefixes.Add(baseAddress);
            listener.Start();
            Console.WriteLine("Servidor iniciado em " + baseAddress);

            while (true)
            {
                // Esperar por uma solicitação de entrada
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                // Criar uma resposta simples usando concatenação de strings
                string responseString = "<html><body><h1>" + "Olá, mundo!" + "</h1></body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                using (var output = response.OutputStream)
                {
                    await output.WriteAsync(buffer, 0, buffer.Length);
                }

                // Fechar a resposta
                response.Close();
            }
        }
    }
}
