public class Potager
{
    //public string Nom { get; }
    public List<Terrains> ListeTerrains { get; }


    public double ReserveFraise {get; set;} // Nombre de fruits dans la reserve
    public double ReservePomme {get; set;}
    // Construteur
    public Potager()
    {
        //Nom = nom;
        ListeTerrains = new List<Terrains>();
        ReserveFraise = 0;
        ReservePomme = 0;
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
                if (plante.croissanceActuelle > 1)
                    Console.Write($"croissance terminée (plante mature), ");
                else
                    Console.Write($"croissance de {plante.croissanceActuelle}, ");
                
                //Console.Write($"besoin en eau : {plante.eauRecu/plante.BesoinEau}, ");
                Console.Write($"besoin en eau : {plante.BesoinEau - plante.eauRecu}, "); //SI ratio négatif -> arroser

                //Nb de fruits donnés
                Console.Write($"nombre de fruits : {plante.nbFruitsActuel}. \n");

            }
            Console.WriteLine($"Nombre de fruits dans la réserve : {ReserveFraise} fraises, {ReservePomme} pommes");
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

    public void Recolter(Terrains terrain)
    {   
        double nbrFraiseRecolte = 0;
        double nbrPommeRecolte = 0;

        Console.WriteLine("Quel type de fruits voulez-vous récolter (pomme, fraise) ?");
        string typeFruits = Console.ReadLine()!.ToLower(); 

        foreach( Plantes p in terrain.ListePlantes) 
        {
            if(p.nbFruitsActuel != 0)
            {
                if(p is Fraise && typeFruits =="fraise")
                {
                    nbrFraiseRecolte += p.nbFruitsActuel;
                    p.nbFruitsActuel = 0; 
                }

                if(p is Pomme && typeFruits == "pomme")
                {
                    nbrPommeRecolte += p.nbFruitsActuel;
                    p.nbFruitsActuel = 0; 
                }                    
            }           
        }
        if (nbrFraiseRecolte == 0 && typeFruits == "fraise")
                Console.WriteLine("Il n'y a pas de fraises à récolter sur ce terrain");
        if (nbrPommeRecolte == 0 && typeFruits == "pomme")
            Console.WriteLine("Il n'y a pas de pommes à récolter sur ce terrain");

        Console.WriteLine($"Vous avez récolté {nbrPommeRecolte} pommes et {nbrFraiseRecolte} fraises sur ce terrain");

        ReserveFraise += nbrFraiseRecolte;
        ReservePomme += nbrPommeRecolte;  
    }


}
