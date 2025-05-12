public class Potager
{
    //public string Nom { get; }
    public List<Terrains> ListeTerrains { get; }

    //A generaliser une liste pour tout les fruits (stockFruits)
    public double ReserveFraise {get; set;} // Nombre de fruits dans la reserve
    public double ReservePomme {get; set;}

    public List<Plantes> ListeRecolte {get; set;}

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

    public bool Arroser(int indexTerrain, int posX, int posY, double quantEau) // Choix du terrain à arroser en fonction de son indice dans la liste de terrains de potager
    {                                                                 // Arroser une plante spécifique dans le terrain (selon ses coordonnées)
        Terrains terrain = ListeTerrains[indexTerrain];
        //terrain.NivEau +=  quantEau - quantEau*terrain.Absorption; 

        foreach (Plantes plante in terrain.ListePlantes)
        {
            if (plante.coordX == posX && plante.coordY == posY) 
            {
                plante.eauRecu += quantEau;             //on arrose direct la plante
                Console.WriteLine($"{plante.Nom} ({posX},{posY}) du terrain {indexTerrain} arrosée. Son besoin en eau actuel est de {plante.BesoinEau-plante.eauRecu}.");
                return true;
            }
        }
        Console.WriteLine($"La plante n'a pas été trouvée.");
        return false;

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
                
                Console.Write($"besoin en eau : {plante.BesoinEau-plante.eauRecu}, "); //SI ratio négatif -> arroser

                //Nb de fruits donnés
                Console.Write($"nombre de fruits : {plante.nbFruitsActuel}. \n");

            }
        }
    }

    
    public void Apparait(Animaux animal, int indexTerrain) // Un animal apparait sur un terrain du potager
    {
        Terrains terrain = ListeTerrains[indexTerrain];

        //terrain.ListeAnimaux.Add(animal); // Ajoute l'animal dans la list<Animaux> présents sur le terrain

        Random rnd = new Random();
        int posx  = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de l'animal choisi aléatoirement
        int posy = rnd.Next(0, terrain.Colonnes);
        animal.posX = posx; //On ajoute les coordonnées en attributs de la classe Animal
        animal.posY = posy;
        
        Console.WriteLine($"Un {animal.NomA} est apparut sur le terrain numéro {indexTerrain}");
        Console.WriteLine($"Il est sur cette position : Ligne = {posx}, Colonne = {posy}");   

        if(terrain.grille[posx,posy] != null && animal is AnimauxNuisible)
        {
            Console.WriteLine("Votre plante est en danger !");
                        
            //Avant de nuire à la plante, -> mode urgence -> le joueur doit pouvoir empecher cela
            animal.Nuire(terrain);
        }

        if(terrain.grille[posx,posy] != null && animal is AnimauxUtiles)
        {
            Console.WriteLine("Votre plante n'est pas en danger");

            animal.Aider(terrain);
        }
    }

    public void Recolter(Terrains terrain)
    {   
        double nbrFraiseRecolte = 0;
        double nbrPommeRecolte = 0;
        int compt = 0;

        Console.WriteLine("Quel type de fruits voulez-vous récolter (pomme, fraise,...) ?");
        string typeFruits = Console.ReadLine()!.ToLower(); 

        foreach( Plantes p in terrain.ListePlantes) 
        {
            if(p.estMature && p.nbFruitsActuel > 0)
            {
                /*if (p.Nom == typeFruits)
                {
                    for (int i = 0 ; i < p.nbFruitsActuel ; i++)
                    {
                        ListeRecolte.Add(p);
                        compt++;
                    }
                    p.nbFruitsActuel = 0; 
                }*/

                if (p is Fraise && typeFruits =="fraise")
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

        Console.WriteLine($"Vous avez récolté {nbrPommeRecolte} pommes et {nbrFraiseRecolte} fraises sur ce terrain !");

        ReserveFraise += nbrFraiseRecolte;
        ReservePomme += nbrPommeRecolte; 

        //Console.WriteLine($"{compt} {typeFruits} ont été récolté.e.s !");
        Console.WriteLine($"Souhaitez vous consulter votre stock de fruits ? (oui ou non)");
        string reponse = Console.ReadLine().ToLower();
        if (reponse == "oui")
        {
            /*int stockFraise = 0;
            int stockPomme = 0;
            foreach(Plantes p in ListeRecolte)
            {
                if (p is Fraise) stockFraise++;
                if (p is Pomme) stockPomme++;
            }*/
            Console.WriteLine("- Stock de fruits -");
            Console.WriteLine($"Fraises : {ReserveFraise}, Pommes : {ReservePomme}, Total fruits : {ReserveFraise+ReservePomme}");
        }

    }

}


