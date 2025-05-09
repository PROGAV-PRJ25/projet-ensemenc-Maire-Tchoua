using System.Runtime.CompilerServices;

public abstract class Terrains {
    public double Absorption {get;set;}
    public double NivEau {get; set;}
    public double CapaciteEauMax {get; set;}
    public int Lignes { get; protected set; }
    public int Colonnes { get; protected set; } 
    public int Capacite {get;} // Nombre de plantes max
    public Plantes.TypeTerrain Type { get; protected set; } // Type de terrain 
    public List<Plantes> ListePlantes {get; set;} // Liste des plantes plantées dans le terrain
    
    //public List<Animaux> ListeAnimaux {get; set;} // Liste des animaux actuellement sur le terrain


    public Plantes[,] grille;   //Matrice des plantes pour gérer positions et espacement


    // Constructeur
    protected Terrains(int lignes, int colonnes, Plantes.TypeTerrain type, double nivEau, double absorption, double capaciteEauMax)
    {
        Lignes = lignes;
        Colonnes = colonnes;
        Capacite = Lignes*Colonnes; //pas necessaire?
        Type = type;
        ListePlantes = new List<Plantes>();
        grille = new Plantes [Lignes,Colonnes];
        NivEau = nivEau;
        Absorption = absorption;
        CapaciteEauMax = capaciteEauMax;
    }

    public bool Planter(Plantes plante, int i, int j)   //i : ligne, j : colonne
    {
        // /!\ Controle de la saison de semi avant de planter
        var saison = ContexteSimulation.SaisonEnCours; //Bizarre ça met hiver par défaut ?!
        if (!plante.SaisonsSemi.Contains(saison))
        {
            Console.WriteLine($"{plante.Nom} ne peut être semé en {saison}.");
            return false;
        }
        
        // Vérification des bornes
        if (i < 0 || i >= Lignes || j < 0 || j >= Colonnes)
        {
            Console.WriteLine("Coordonnées hors limites.");
            return false;
        }

         // Vérification de la case libre
        if (grille[i, j] != null)
        {
            Console.WriteLine("Case déjà occupée.");
            return false;
        }

        // Vérification de l'espacement : aucune plante ne doit être à moins de plante.Espacement cases
        // Espacement pour la plante à planter respecté, mais pas pour les plantes qui sont déjà plantées... -> limite
        int d = plante.Espacement;  // Distance à respecter        
        int iMin = Math.Max(0, i - d);
        int iMax = Math.Min(Lignes - 1, i + d);
        int jMin = Math.Max(0, j - d);
        int jMax = Math.Min(Colonnes - 1, j + d);

        for (int iNew = iMin; iNew <= iMax; iNew++)
        {
            for (int jNew = jMin; jNew <= jMax; jNew++)
            {
                if (iNew == i && jNew == j) continue; // ignorer la case cible
                if (grille[iNew, jNew] != null)
                {
                    Console.WriteLine($"Les distances autour de la case ({i},{j}) ne sont pas respectées.");
                    return false;
                }
            }
        }
        
        // Tout est OK, on plante
        plante.terrainActuel = Type;    //On dit à la plante dans quelle terrain elle est plantée
        plante.coordX = i;  //On récupère les coordonnées dans la classe Plante
        plante.coordY = j;
        grille[i, j] = plante;
        ListePlantes.Add(plante);
        Console.WriteLine($"{plante.Nom} plantée en ({i},{j})."); 
        return true;
    }

    public bool SupprimerPlante(int i, int j)
    {
        var plante = grille[i, j];
        if (plante == null)
            return false;              // rien à supprimer

        // Retirer les coordonnées dans plantes... ?
        plante.estMorte = true;

        // Retrait de la grille
        grille[i, j] = null;

        // Retrait de la liste
        ListePlantes.Remove(plante);

        Console.WriteLine($"Plante {plante.Nom} est morte en ({i},{j}).");

        return true;
    }

    public void Apparait(Animaux animal) // Un animal apparait sur le terrain
    {
        //Ajouter l'animal dans la list<Animaux> présents sur le terrain
        Random rnd = new Random();
        int posx  = rnd.Next(0, Lignes); // Coordonnées x,y de l'animal
        int posy = rnd.Next(0, Colonnes);
        Console.WriteLine($"Un {animal.NomA} est apparut sur votre Terrain");
        Console.WriteLine($"Il est sur cette position : Ligne={posx}, Colonne={posy}");   

        if(grille[posx,posy] != null && animal is AnimauxNuisible)
        {
            Console.WriteLine("Votre plante est en danger !");
             
            foreach (Plantes p in ListePlantes) // Cherche la plante qui est sur la même position que l'animal
            {
                if(p.coordX == posx && p.coordY == posy)
                {
                    //Escargot escargot = (Escargot) animal; // Récupère l'information que l'animal est de classe escargot pour appeler la méthode
                    if(animal is Escargot escargot) // si l'animal de la classe est un escargot
                        escargot.Grignotter(p); // la plante est grignottée par l'escargot

                    if (animal is Oiseaux oiseaux) 
                            oiseaux.Picorer(p);
                }
                if(Math.Abs(p.coordX - posx) <= 1 && Math.Abs(p.coordY - posy) <= 1)
                {
                    if (animal is Criquet criquet) 
                        criquet.Affaiblir(p);
                }
            }                   
        }

        if(grille[posx,posy] != null && animal is AnimauxUtiles)
        {
            Console.WriteLine("Votre plante n'est pas en danger");
            
            foreach (Plantes p in ListePlantes)
            {
                if(Math.Abs(p.coordX - posx) <= 1 && Math.Abs(p.coordY - posy) <= 1)
                {
                    if(animal is Abeille abeille) 
                        abeille.Butiner(p); // L'abeille butines la plante p et les plantes adjacentes
                    if(animal is VerDeTerre verDeTerre)
                        verDeTerre.Remuer(p); 
                }
            }  
        }
    }

    public void VerifierEtatPlantes() // A lancer après Pousser, pour vérifier si plante morte
    {
        foreach (Plantes p in ListePlantes)
        {
            if (p.estMorte) // Si la plante est morte (fin de vie) on la supp du terrain
                SupprimerPlante(p.coordX, p.coordY);
        }
    }

    // Affichage
    public void AfficherConsole()
    {
        // En-tête colonnes
        Console.Write("   ");
        for (int j = 0; j < Colonnes; j++)
            Console.Write($" {j}  ");
        Console.WriteLine();

        // Ligne séparatrice
        Console.Write("  +");
        for (int j = 0; j < Colonnes; j++)
            Console.Write("---+");
        Console.WriteLine();

        // Chaque ligne
        for (int i = 0; i < Lignes; i++)
        {
            // Contenu des cases
            Console.Write($"{i} |");
            for (int j = 0; j < Colonnes; j++)
            {
                var p = grille[i, j];
                string symbole = p != null ? p.GetSymboleConsole() : "   ";
                Console.Write(symbole + "|");
            }
            Console.WriteLine();

            // Séparateur de fin de ligne
            Console.Write("  +");
            for (int j = 0; j < Colonnes; j++)
                Console.Write("---+");
            Console.WriteLine();
        }
       
    }
    
    public override string ToString()
    {
        return $"Terrain de {Type} (Capacité: {Capacite}, Plantes plantées: {ListePlantes.Count})";
    }
    
}