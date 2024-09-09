using System;
using System.IO;
using System.Net;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Definir o endereço base do servidor
        string baseAddress = "http://localhost:8080/";

        // Criar um HttpListener
        using (HttpListener listener = new HttpListener())
        {
            // Adicionar prefixo de URL onde o servidor vai escutar
            listener.Prefixes.Add(baseAddress);
            listener.Start();
            Console.WriteLine("sv " + baseAddress);

            while (true)
            {
                // Esperar por uma solicitação de entrada
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                try
                {
                    // Obter o caminho solicitado
                    string relativePath = request.Url.AbsolutePath.TrimStart('/');
                    string fullPath = Path.Combine(@"C:\", relativePath);

                    if (File.Exists(fullPath))
                    {
                        // Ler o arquivo e enviar o conteúdo
                        byte[] fileBytes = File.ReadAllBytes(fullPath);
                        response.ContentType = "application/octet-stream";
                        response.ContentLength64 = fileBytes.Length;

                        using (Stream output = response.OutputStream)
                        {
                            output.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }
                    else if (Directory.Exists(fullPath))
                    {
                        // Listar o conteúdo do diretório
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<html><body><h1>Index of " + request.Url.AbsolutePath + "</h1><ul>");

                        // Adicionar links para diretórios
                        foreach (var dir in Directory.GetDirectories(fullPath))
                        {
                            sb.Append("<li><a href=\"" + Path.GetFileName(dir) + "/\">" + Path.GetFileName(dir) + "</a></li>");
                        }

                        // Adicionar links para arquivos
                        foreach (var file in Directory.GetFiles(fullPath))
                        {
                            sb.Append("<li><a href=\"" + Path.GetFileName(file) + "\">" + Path.GetFileName(file) + "</a></li>");
                        }

                        sb.Append("</ul></body></html>");
                        byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

                        response.ContentLength64 = buffer.Length;
                        using (Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        // Arquivo ou diretório não encontrado
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        byte[] buffer = Encoding.UTF8.GetBytes("404 - Not Found");
                        response.ContentLength64 = buffer.Length;
                        using (Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Erro inesperado
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    byte[] buffer = Encoding.UTF8.GetBytes("500 - Internal Server Error: " + ex.Message);
                    response.ContentLength64 = buffer.Length;
                    using (Stream output = response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }
                }
                finally
                {
                    // Fechar a resposta
                    response.Close();
                }
            }
        }
    }
}
