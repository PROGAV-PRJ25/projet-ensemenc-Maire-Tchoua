public class Potager
{
    public List<Terrains> ListeTerrains { get; }    //Liste des terrains présents dans le potager
    private Dictionary<string,double> StockFruits { get; }   //Stockage des fruits récoltés

    double index = 0;
    public bool urgenceActive = false;

    // Construteur
    public Potager()
    {
        ListeTerrains = new List<Terrains>();
        StockFruits = new Dictionary<string, double>();
    }

    // Actions du joueur sur son potager
    
    public void AjouterTerrain(Terrains terrain)
    {
        terrain.numTerrain = index;
        ListeTerrains.Add(terrain);
        Console.WriteLine($"Un terrain de {terrain.Type} et de capacité {terrain.Capacite} a été ajouté.");
        index++;
    }

    //Arrosage d'un terrain complet
    public void ArroserTerrain(int indexTerrain, double quantEau) // Choix du terrain à arroser en fonction de son indice dans la liste de terrains de potager
    {
        Terrains terrain = ListeTerrains[indexTerrain];
        terrain.NivEau +=  quantEau - quantEau*terrain.Absorption;

        foreach (Plantes plante in terrain.ListePlantes)
        {
            plante.eauRecu = terrain.NivEau;
        }
    }
    
    //Arrosage ciblée sur une plante
    public bool ArroserPlante(int indexTerrain, int posX, int posY, double quantEau) // Choix du terrain à arroser en fonction de son indice dans la liste de terrains de potager
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
        Console.WriteLine($"\n=== État du potager ===");

        foreach (var terrain in ListeTerrains)
        {
            Console.WriteLine($"Terrain numéro {terrain.numTerrain} de type {terrain.Type}.");
            
            terrain.AfficherConsole(terrain);
            
            
            foreach (var plante in terrain.ListePlantes)
            {
                if (!(plante is MauvaiseHerbe))
                {
                    Console.WriteLine($"- {plante.Nom} ({plante.coordX},{plante.coordY}) : ");

                    //Etat de santé
                    if (!plante.estMalade)
                        Console.Write("En bonne santé, ");

                    //Croissance
                    if (plante.croissanceActuelle > 1)
                        Console.Write($"croissance terminée (plante mature), ");
                    else
                        Console.Write($"croissance de {plante.croissanceActuelle}, ");

                    Console.Write($"besoin en eau : {plante.BesoinEau - plante.eauRecu}, "); //SI ratio négatif ne pas arroser

                    //Nb de fruits donnés
                    Console.Write($"nombre de fruits : {plante.nbFruitsActuel}. \n");
                }
            }
        }
    }

    public void ApparaitAnimaux(Animaux animal,Terrains terrain)
    {
        Random rnd = new Random();
        animal.posX  = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de l'obstacle
        animal.posY = rnd.Next(0, terrain.Colonnes);
        
        Console.WriteLine($"Un {animal.NomA} {animal.GetSymboleConsole()} est apparut sur le Terrain {terrain.numTerrain}, sa position est : ({animal.posX},{animal.posY})");
    }
    
    public void Impacter(Animaux animal, Terrains terrain) // Un animal apparait sur un terrain du potager
    {
        if (animal is AnimauxNuisible)
        {
            if (terrain.grille[animal.posX, animal.posY] != null)
            {
                Console.WriteLine("Votre plante est en danger !");
                animal.Nuire(terrain);
            }
            terrain.urgenceAnimaux = true; // Déclanche le mode Urgence même si l'animal n'est pas sur une plante 
        }
 
        if (terrain.grille[animal.posX, animal.posY] != null && animal is AnimauxUtiles)
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
        Console.WriteLine($"La maladie {maladie.Nom} est apparue sur le Terrain {terrain.numTerrain}, sa position est : ({maladie.posX},{maladie.posY})");
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
        terrain.urgenceMaladie = true;  // Déclanche le mode urgence  
    }

    public void ApparaitMauvaiseHerbe(Terrains terrain)
    {
        MauvaiseHerbe mauvaiseHerbe = new MauvaiseHerbe();
        Random rnd = new Random();
        int coordX = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de la maladie
        int coordY = rnd.Next(0, terrain.Colonnes);
        terrain.Planter(mauvaiseHerbe, coordX, coordY);
        Console.WriteLine($"Une mauvaise herbe est apparue sur le Terrain {terrain.numTerrain}, sa position est : ({mauvaiseHerbe.coordX},{mauvaiseHerbe.coordY})");
    }

    public void Desherber(Terrains terrain)
    {
        // Crée une copie « gelée » de la liste au début de la méthode
        var plantesAVerifier = terrain.ListePlantes.ToList();

        foreach (Plantes p in plantesAVerifier)
        {
            if (p is MauvaiseHerbe)
            {
                terrain.SupprimerPlante(p.coordX, p.coordY);
            }
        }
        Console.WriteLine($"Terrain numéro {terrain.numTerrain} désherbé !");
        terrain.urgenceMauvaiseHerbe = false; //Urgence traitée
    }

    public void Recolter(Terrains terrain)
    {
        //On s'assure qu'il existe des plantes sur le terrain
        if (terrain.ListePlantes == null)
        {
            Console.WriteLine("Erreur : aucune plante à récolter sur ce terrain.");
            return;
        }

        //On demande le type de fruit à récolté
        Console.WriteLine("Quel type de fruits voulez-vous récolter (pomme, fraise, kiwi, mangue, poire, pasteque) ?");
        string typeFruits = Console.ReadLine()!.ToLower(); //Nom du type du fruit à récolté

        //Calcul de la récolte
        double récolte = 0;
        foreach (Plantes p in terrain.ListePlantes)
        {
            if (p.Nom.ToLower() == typeFruits && p.nbFruitsActuel > 0)
            {
                récolte += p.nbFruitsActuel;
                p.nbFruitsActuel = 0;
            }
        }

        if (récolte == 0)
        {
            Console.WriteLine($"Pas de {typeFruits} à récolter ici.");
            return;
        }
        else
        {
            //Mise à jour de l’entrée dans le dictionnaire si besoin
            if (!StockFruits.ContainsKey(typeFruits))
                StockFruits[typeFruits] = 0;

            //Ajout de la récolte dans le dictionnaire si besoin
            StockFruits[typeFruits] += récolte;

            Console.WriteLine($"Vous avez récolté {récolte} {typeFruits}(s).");
        }

        //Affichage du stock de fruits
        Console.WriteLine($"Souhaitez vous consulter votre stock de fruits ? (oui ou non)");
        string reponse = Console.ReadLine()!.ToLower();
        if (reponse == "oui")
        {
            Console.WriteLine("– Stock de fruits –");
            foreach (var kv in StockFruits)
                Console.WriteLine($"{kv.Key.ToUpper()} : {kv.Value}");
        }
    }

    public void Chasser(AnimauxNuisible animal, Terrains terrain)
    {
        terrain.ListeAnimauxNuisibles.Remove(animal);     // Supprime l'animal de la liste 
        Console.WriteLine($"Vous avez chasser {animal} du terrain {terrain.numTerrain}");  
        if(terrain.ListeAnimauxNuisibles.Count == 0)
            terrain.urgenceAnimaux = false; 
    }

    public void Traiter(Maladies maladie, Terrains terrain)
    {
        maladie.DureeContamination -= 5;  // La maladie est progressivement éradiquée 
        
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(maladie.DureeContamination == 0 )
            {
                terrain.ListeMaladie.Remove(maladie);
                p.estMalade = false;
                Console.WriteLine($"Votre plante ({p.coordX},{p.coordY}) n'est plus malade");
            }  
            else if(p.estMalade)
            {
                if(maladie is Anthracnose)
                {
                    p.nbFruitsActuel += 0.2;
                }
                if(maladie is Pythium)
                {
                    p.VitesseCroissance += 0.1;
                }

                Console.WriteLine("Continuez de traiter votre plante");
            }            
        }
        if(terrain.ListeMaladie.Count == 0)
            terrain.urgenceMaladie = false;
    }

    public void Couvrir(Terrains terrain)
    {
        terrain.NivEau -= 20;
        Console.WriteLine($"Le niveau d'eau du terrain {terrain.numTerrain} diminue");
        if(terrain.NivEau < terrain.CapaciteEauMax) 
        {
            Console.WriteLine($"Le terrain {terrain.numTerrain} n'est plus inondé");
            terrain.urgenceInondation = false;
        }
    }

    public void VerifierUrgence()
    {
        foreach (Terrains t in ListeTerrains)
        {
            if (t.urgenceAnimaux == true || t.urgenceInondation == true || t.urgenceMaladie == true || t.urgenceMauvaiseHerbe == true)
            {
                urgenceActive = true;
                break; // Plus besoin de continuer à parcourir la liste
            }
            else
                urgenceActive = false;
        } 
    }
}


