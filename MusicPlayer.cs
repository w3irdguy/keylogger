using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Bem-vindo ao MusicPlayer 2.0");
        Console.WriteLine("============================");
        Console.WriteLine();

        string[] tracks = { "Cool Vibes - Track 1", "Chill Beats - Track 2", "Electronic Dreams - Track 3" };
        string[] artists = { "DJ Beatmaker", "Synth Master", "The Chillers" };

        while (true)
        {
            Console.Clear();
            DisplayNowPlaying(tracks, artists);
            DisplayMenu();

            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    await PlayTrack(tracks, artists);
                    break;
                case "2":
                    ShowTrackList(tracks, artists);
                    break;
                case "3":
                    Console.WriteLine("Saindo...");
                    return;
                default:
                    Console.WriteLine("Escolha inválida, tente novamente.");
                    break;
            }
        }
    }

    static void DisplayNowPlaying(string[] tracks, string[] artists)
    {
        Random rand = new Random();
        int trackIndex = rand.Next(tracks.Length);
        Console.WriteLine("Agora tocando:");
        Console.WriteLine($"Faixa: {tracks[trackIndex]}");
        Console.WriteLine($"Artista: {artists[trackIndex]}");
        Console.WriteLine();
    }

    static void DisplayMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Reproduzir Faixa");
        Console.WriteLine("2. Mostrar Lista de Faixas");
        Console.WriteLine("3. Sair");
        Console.Write("Escolha uma opção: ");
    }

    static async Task PlayTrack(string[] tracks, string[] artists)
    {
        Random rand = new Random();
        int trackIndex = rand.Next(tracks.Length);
        Console.WriteLine($"Reproduzindo: {tracks[trackIndex]} - {artists[trackIndex]}");
        Console.WriteLine("Simulação de reprodução...");
        await Task.Delay(3000); // Simula o tempo de reprodução
        Console.WriteLine("Faixa concluída.");
        Console.WriteLine();
    }

    static void ShowTrackList(string[] tracks, string[] artists)
    {
        Console.WriteLine("Lista de Faixas:");
        for (int i = 0; i < tracks.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {tracks[i]} - {artists[i]}");
        }
        Console.WriteLine();
    }
}
