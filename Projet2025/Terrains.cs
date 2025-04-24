public abstract class Terrains {
    
    public int Lignes { get; protected set; }
    public int Colonnes { get; protected set; } 
    public int Capacite {get;} // Nombre de plantes max
    public Plantes.TypeTerrain Type { get; protected set; } // Type de terrain 
    public List<Plantes> ListePlantes {get; set;} // Liste des plantes plantées dans le terrain

    public Plantes[,] grille;   //Matrice des plantes pour gérer positions et espacement



    // Constructeur
    protected Terrains(int lignes, int colonnes, Plantes.TypeTerrain type)
    {
        Lignes = lignes;
        Colonnes = colonnes;
        Capacite = Lignes*Colonnes; //pas necessaire?
        Type = type;
        ListePlantes = new List<Plantes>();
        grille = new Plantes[Lignes,Colonnes];
    }

    public bool Planter(Plantes plante, int i, int j)   //i : ligne, j : colonne
    {

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

        // Vérification du type de terrain (à modif ?) -> si pas sur le bon terrain, vitesse de croissance /2 par ex
        /*
        if (plante.TerrainPref != Type) 
        {
            Console.WriteLine($"Le terrain ({Type}) n'est pas adapté pour {plante.Nom}.");
            return false;
        }*/

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

        // Retrait de la grille
        grille[i, j] = null;
        // Retrait de la liste
        ListePlantes.Remove(plante);
        Console.WriteLine($"Plante {plante.Nom} est morte en ({i},{j}).");
        return true;
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
                string symbole = p != null ? p.GetSymboleConsole() : " . ";
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