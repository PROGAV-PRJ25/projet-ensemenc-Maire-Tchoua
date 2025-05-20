public class Potager
{
    //public string Nom { get; }
    public List<Terrains> ListeTerrains { get; }
    public bool urgenceActive = false;

    //A generaliser une liste pour tout les fruits (stockFruits)
    public double ReserveFraise {get; set;} // Nombre de fruits dans la reserve
    public double ReservePomme {get; set;}
    public List<Plantes> ListeRecolte {get; set;}
    public double index = 0;
    
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
        Console.WriteLine($"Nombre de fruits dans la réserve : {ReserveFraise} fraises, {ReservePomme} pommes");
    }

    public void ApparaitAnimaux(Animaux animal,Terrains terrain)
    {
        Random rnd = new Random();
        animal.posX  = rnd.Next(0, terrain.Lignes); // Coordonnées x,y de l'obstacle
        animal.posY = rnd.Next(0, terrain.Colonnes);
        
        Console.WriteLine($"Un {animal.NomA} est apparut sur le Terrain {terrain.numTerrain}");
        Console.WriteLine($"Il est sur cette position : Ligne = {animal.posX}, Colonne = {animal.posY}"); 
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
        Console.WriteLine($"La maladie {maladie.Nom} est apparue sur le Terrain {terrain.numTerrain}");
        Console.WriteLine($"Elle est sur cette position : ({maladie.posX},{maladie.posY})");
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
        Console.WriteLine($"Une mauvaise herbe est apparue sur le Terrain {terrain.numTerrain}");
        Console.WriteLine($"Elle est sur cette position : ({mauvaiseHerbe.coordX},{mauvaiseHerbe.coordY})");
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
        double nbrFraiseRecolte = 0;
        double nbrPommeRecolte = 0;

        Console.WriteLine("Quel type de fruits voulez-vous récolter (pomme, fraise) ?");
        string typeFruits = Console.ReadLine()!.ToLower();

        foreach (Plantes p in terrain.ListePlantes)
        {
            if (p.nbFruitsActuel != 0)
            {
                if (p is Fraise && typeFruits == "fraise")
                {
                    nbrFraiseRecolte += p.nbFruitsActuel;
                    p.nbFruitsActuel = 0;
                }

                if (p is Pomme && typeFruits == "pomme")
                {
                    nbrPommeRecolte += p.nbFruitsActuel;
                    p.nbFruitsActuel = 0;
                }
            }
        }

        if (nbrFraiseRecolte == 0 && typeFruits == "fraise")
            Console.WriteLine($"Il n'y a pas de fraises à récolter sur le terrain {terrain.numTerrain}");
        if (nbrPommeRecolte == 0 && typeFruits == "pomme")
            Console.WriteLine($"Il n'y a pas de pommes à récolter sur le terrain {terrain.numTerrain}");

        Console.WriteLine($"Vous avez récolté {nbrPommeRecolte} pommes et {nbrFraiseRecolte} fraises sur le terrain {terrain.numTerrain}");

        ReserveFraise += nbrFraiseRecolte;
        ReservePomme += nbrPommeRecolte;

        Console.WriteLine($"Souhaitez vous consulter votre stock de fruits ? (oui ou non)");
        string reponse = Console.ReadLine().ToLower();
        if (reponse == "oui")
        {
            Console.WriteLine("- Stock de fruits -");
            Console.WriteLine($"Fraises : {ReserveFraise}, Pommes : {ReservePomme}, Total fruits : {ReserveFraise + ReservePomme}");
        }
    }

    public void Chasser(AnimauxNuisible animal, Terrains terrain)
    {
        terrain.ListeAnimauxNuisibles.Remove(animal);     // Supprime l'animal de la liste 
        Console.WriteLine($"Vous avez chasser {animal} du terrain {terrain.numTerrain}");  
        terrain.urgenceAnimaux = false; 
    }

    public void Traiter(Maladies maladie, Terrains terrain)
    {
        maladie.DureeContamination -= 5;  // La maladie est progressivement éradiquée 
        
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

                if(maladie.DureeContamination == 0 )
                {
                    terrain.ListeMaladie.Remove(maladie);
                    p.estMalade = false;
                    Console.WriteLine($"Votre plante ({p.coordX},{p.coordY}) n'est plus malade");
                    //terrain.urgenceMaladie = false;
                }
                else
                    Console.WriteLine("Continuez de traiter votre plante");
            }            
        }
        if(maladie.DureeContamination == 0)
        {
            terrain.urgenceMaladie = false;
        }
    }

    public void Couvrir(Terrains terrain)
    {
        terrain.NivEau -= 20;
        Console.WriteLine($"Le niveau d'eau du terrain {terrain.numTerrain} diminue");
        if(terrain.NivEau < terrain.CapaciteEauMax) //Modifier les conditions car ne s'enclenche jamais ?
        {
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


