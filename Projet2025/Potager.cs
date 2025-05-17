public class Potager
{
    //public string Nom { get; }
    public List<Terrains> ListeTerrains { get; }
    public bool urgenceActive = false;

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

    public void ApparaitAnimaux(Animaux animal,Terrains terrain)
    {
        Random rnd = new Random();
        animal.posX  = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de l'obstacle
        animal.posY = rnd.Next(0, terrain.Colonnes);
        
        Console.WriteLine($"Un {animal.NomA} est apparut sur votre Terrain");
        Console.WriteLine($"Il est sur cette position : Ligne = {animal.posX}, Colonne = {animal.posY}"); 

    }
    public void Impacter(Animaux animal, Terrains terrain) // Un animal apparait sur un terrain du potager
    {
       
        if(animal is AnimauxNuisible)
        {
            if(terrain.grille[animal.posX, animal.posY] != null)
            {
                Console.WriteLine("Votre plante est en danger !");
                animal.Nuire(terrain);
            }
            urgenceActive = true; // Déclanche le mode Urgence même si l'animal n'est pas sur une plante 
        }
       

        if(terrain.grille[animal.posX,animal.posY] != null && animal is AnimauxUtiles)
        {
            Console.WriteLine("Votre plante n'est pas en danger");
            animal.Aider(terrain);
        }
    }

    public void ApparaitMaladies(Maladies maladie, Terrains terrain)
    {
        Random rnd = new Random();
        maladie.posX  = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de la maladie
        maladie.posX = rnd.Next(0, terrain.Colonnes);
        Console.WriteLine($"Une maladie est apparue sur cette position : Ligne={maladie.posX}, Colonne={maladie.posX}");
    }
    public void Contaminer(Maladies maladie, Terrains terrain)
    {         
        foreach (Plantes p in terrain.ListePlantes)
        {         
            if(p.coordX == maladie.posX && p.coordY == maladie.posY)
            {
                p.estMalade = true;
                if(maladie is Anthracnose anthracnose)
                    anthracnose.Pourrir(p);
                if(maladie is Pythium pythium)
                    pythium.Affaiblir(p);
            }
        }    
        urgenceActive = true;  // Déclanche le mode urgence  
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


    public void Chasser(AnimauxNuisible animal, Terrains terrain)
    {
        terrain.ListeAnimauxNuisibles.Remove(animal);     // Supprime l'animal de la liste 
        Console.WriteLine($"Vous avez chasser {animal} de votre terrain");  
        urgenceActive = false; 
    }

    public void Traiter(Maladies maladie, Terrains terrain)
    {
        // La maladie est progressivement éradiquée 
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(p.estMalade)
            {
                if(maladie is Anthracnose)
                {
                    p.nbFruitsActuel += 0.2;
                }
                if(maladie is Pythium)
                {
                    p.VitesseCroissance += 0.1;
                }
            }

            maladie.DureeContamination -= 2;
            if(maladie.DureeContamination == 0 )
            {
                terrain.ListeMaladie.Remove(maladie);
                p.estMalade = false;
                Console.WriteLine($"Votre plante ({p.coordX},{p.coordY}) n'est plus malade");
            }
        }

        if(terrain.ListeMaladie == null)
            urgenceActive = false;
    }


}
