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

    
    public void Apparait(Animaux animal, Terrains terrain) // Un animal apparait sur un terrain du potager
    {
        //terrain.ListeAnimaux.Add(animal); // Ajoute l'animal dans la list<Animaux> présents sur le terrain
        
        Random rnd = new Random();
        int posx  = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de l'animal choisi aléatoirement
        int posy = rnd.Next(0, terrain.Colonnes);
        animal.posX = posx; //On ajoute les coordonnées en attributs de la classe Animal
        animal.posY = posy;
        
        Console.WriteLine($"Un {animal.NomA} est apparut sur votre Terrain");
        Console.WriteLine($"Il est sur cette position : Ligne = {posx}, Colonne = {posy}");   

        if(terrain.grille[posx,posy] != null && animal is AnimauxNuisible)
        {
            Console.WriteLine("Votre plante est en danger !");

            animal.Nuire(terrain);
        }

        if(terrain.grille[posx,posy] != null && animal is AnimauxUtiles)
        {
            Console.WriteLine("Votre plante n'est pas en danger");

            animal.Aider(terrain);
        }
    }
}
