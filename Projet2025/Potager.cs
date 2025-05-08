public class Potager
{
    //public string Nom { get; }
    public List<Terrains> ListeTerrains { get; }


    // Construteur
    public Potager()
    {
        //Nom = nom;
        ListeTerrains = new List<Terrains>();
    }

    // Actions du joueur sur son potager

    public void AjouterTerrain(Terrains terrain) //Demander le type et la taille !
    {
        ListeTerrains.Add(terrain);
        Console.WriteLine($"Un terrain de {terrain.Type} et de capacité {terrain.Capacite} a été ajouté.");
    }

    public void Arroser(int indexTerrain, double quantEau) // Choix du terrain à arroser en fonction de son indice dans la liste de terrains de potager
    {
        Terrains terrain = ListeTerrains[indexTerrain];
        terrain.NivEau +=  quantEau - quantEau*terrain.Absorption;

        foreach (Plantes plante in terrain.ListePlantes)
        {
            plante.eauRecu = terrain.NivEau;
        }

    }

    // Affiche l'état complet du potager (tous les terrains)
    public void AfficherEtat()
    {
        int index = 0;

        Console.WriteLine($"\n=== État du potager ===");

        foreach (var terrain in ListeTerrains)
        {
            Console.WriteLine($"Terrain numéro {index} de type {terrain.Type}.");
            
            terrain.AfficherConsole();

            index ++;
            
            foreach (var plante in terrain.ListePlantes)
            {
                Console.WriteLine($"- {plante.Nom} ({plante.coordX},{plante.coordY}) : ");
                
                //Etat de santé
                if (plante.estMalade)
                    Console.Write("⚠️ atteint de la maladie de ..., ");    //donner le nom de la maladie
                else
                    Console.Write("En bonne santé, ");

                //Croissance
                if (!plante.estMature)
                    Console.Write($"croissance de {plante.croissanceActuelle}, ");
                else
                    Console.Write($"croissance terminée (plante mature), ");
                
                Console.Write($"besoin en eau : {plante.eauRecu/plante.BesoinEau}, "); //SI ratio négatif -> arroser

                //Nb de fruits donnés
                Console.Write($"nombre de fruits : {plante.nbFruitsActuel}. \n");

            }
        }
    }

    /*
    // Test simule le passage d'un tour (ex : une semaine)
    public void PasserUnTour()
    {
        Console.WriteLine($"\n--- Tour de jeu pour {Nom} ---");
        foreach (var terrain in ListeTerrains)
            foreach (var plante in terrain.ListePlantes)
                plante.Pousser();
    }*/
}
