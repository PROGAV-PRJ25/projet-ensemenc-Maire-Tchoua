public class Potager
{
    public string Nom { get; }
    public List<Terrains> ListeTerrains { get; }

    // Construteur
    public Potager(string nom)
    {
        Nom = nom;
        ListeTerrains = new List<Terrains>();
    }

    // Permet d'ajouter un nouveau terrain au potager
    public void AjouterTerrain(Terrains terrain)
    {
        ListeTerrains.Add(terrain);
        Console.WriteLine($"[{Nom}] a ajouté un terrain de {terrain.Type} (capacité {terrain.Capacite}.");
    }

    public void Arroser(Terrains terrain)
    {
        //terrain.NivEau +=  -> choisir la quantité
        foreach (Plantes plante in terrain.ListePlantes)
        {
            //augmenter le niveau d'eau de chaque plante
            
        }

    }

    // Affiche l'état complet du potager (tous les terrains)
    public void AfficherEtat()
    {
        Console.WriteLine($"\n=== État du potager {Nom} ===");
        foreach (var terrain in ListeTerrains)
        {
            Console.WriteLine(terrain);
            foreach (var plante in terrain.ListePlantes)
                Console.WriteLine($"  - {plante.Nom} (croissance : {plante.croissanceActuelle})");
        }
        Console.WriteLine("===================================\n");
    }

    // Test simule le passage d'un tour (ex : une semaine)
    public void PasserUnTour()
    {
        Console.WriteLine($"\n--- Tour de jeu pour {Nom} ---");
        foreach (var terrain in ListeTerrains)
            foreach (var plante in terrain.ListePlantes)
                plante.Pousser();
    }
}
