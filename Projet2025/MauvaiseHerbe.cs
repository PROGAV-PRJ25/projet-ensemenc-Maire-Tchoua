public class MauvaiseHerbe : Plantes {

    private static List<Plantes.Saisons> ToutesSaisons =
        new List<Plantes.Saisons> {
            Plantes.Saisons.Hiver, Plantes.Saisons.Printemps,
            Plantes.Saisons.Eté,   Plantes.Saisons.Automne
        };

    public MauvaiseHerbe() : base(
        nom : "MauvaiseHerbe", 
        saisonsSemi : ToutesSaisons,
        terrainPref : TypeTerrain.Terre, //ignoré, pousse sur tous terrain
        espacement : 0, //pas d'espacement requis
        place : 1, 
        vitesseCroissance : 1, //inutile, pas besoin de temps
        besoinEau : 1, 
        besoinLum : 1, 
        tempMax : 100, 
        tempMin : 0, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 12, 
        nbFruitsMax : 0,
        nature : NaturePlante.Annuelle,  
        saisonFruits : Saisons.Hiver    //ignoré ne donne pas de fruits
        )
    {
        croissanceActuelle = 1; //Directement mature
        estMature = true;
    }

    /// Mauvaise herbe se propage rapidement, peut envahir toutes les cases et tuer les autres plantes.
    /// En créer qu'une seule à la fois, méthode pour les propager toutes, après une simulation
    /// Se propage d'une case adjacente à la fois (aléatoirement)
    public void Pousser(Terrains terrain) // Quand elle pousse elle se propage sur les autres cases du terrain
    {

        int lignes = terrain.Lignes;
        int colonnes = terrain.Colonnes;
        var aPropager = new List<(int i, int j)>();
        var rnd = new Random();
        bool propagationReussie;
        int newI;
        int newJ;

        // Collecte positions actuelles de mauvaises herbes
        for (int i = 0; i < lignes; i++)
            for (int j = 0; j < colonnes; j++)
                if (terrain.grille[i, j] is MauvaiseHerbe)
                    aPropager.Add((i, j));

        // Pour chaque plante, essayer d'envahir les 8 voisins
        foreach (var (i, j) in aPropager)
        {
            propagationReussie = false;
            while (propagationReussie == false)
            {
                newI = i + rnd.Next(-1,2);
                newJ = j + rnd.Next(-1,2);

                if (newI != i || newJ != j) 
                {
                    if (newI > 0 && newI <= lignes && newJ > 0 && newJ <= colonnes)
                    {
                         // Vérifier bornes et présence
                        if (terrain.grille[newI, newJ] == null)
                        {
                            terrain.Planter(new MauvaiseHerbe(), newI, newJ);
                        }
                        else if (!(terrain.grille[newI, newJ] is MauvaiseHerbe))
                        {
                            // Tuer la plante existante
                            terrain.SupprimerPlante(newI, newJ);
                            terrain.Planter(new MauvaiseHerbe(), newI, newJ);
                        }
                        propagationReussie = true;
                    }
                }

            }
        }
    }

}


