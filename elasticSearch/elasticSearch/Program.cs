using Nest;

class Program
{
    private static void Main(string[] args)
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("crime_suspects");

        var client = new ElasticClient(settings);

        var searchResponse = client.Search<CrimeSuspect>(s => s
            .Index("crime_suspects")
            .Query(q => q
                .Bool(b => b
                    .Must(
                        mu => mu.Match(m => m
                            .Field(f => f.Alias)
                            .Query("Phantom")
                        )
                    )
                    .MustNot(
                        mn => mn.Match(m => 
                            m.Field(f => f.Name)
                                .Query("Clark")))
                )
            )
        
        );

        
        // Output
        foreach (var suspect in searchResponse.Documents)
            Console.WriteLine($"Name: {suspect.Name},  Alias: {suspect.Alias}, Last Seen: {suspect.LastSeenDate}");
    }

    public class CrimeSuspect
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string CriminalRecord { get; set; }
        public DateTime LastSeenDate { get; set; }
        public string KnownAssociates { get; set; }
        public double DangerLevel { get; set; }
        public double Height { get; set; }
    }
}