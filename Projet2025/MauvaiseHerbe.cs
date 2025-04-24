public class MauvaiseHerbe : Plantes {

    private static List<Plantes.Saisons> ToutesSaisons =
        new List<Plantes.Saisons> {
            Plantes.Saisons.Hiver, Plantes.Saisons.Printemps,
            Plantes.Saisons.Eté,   Plantes.Saisons.Automne
        };

    public MauvaiseHerbe() : base(
        nom : "MauvaiseHerbe", 
        saisonsSemi : ToutesSaisons,
        terrainPref : TypeTerrain.Terre, //ignoré en propagation
        espacement : 0, //pas d'espacement requis
        place : 1, 
        vitesseCroissance : 4.0, //pousse très vite
        besoinEau : 1, 
        besoinLum : 1, 
        tempMax : 100, 
        tempMin : 0, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 12, 
        nbFruitsMax : 0,
        nature : NaturePlante.Annuelle  
        )
    {}

    /// Mauvaise herbe se propage rapidement, peut envahir toutes les cases et tuer les autres plantes.
    /// En créer qu'une seule à la fois, méthode pour les propager toutes, après une simulation
    /// Propager moins vite ????
    public void Propager(Terrains terrain) // Quand elle pousse elle se propage sur les autres cases du terrain
    {
        
        int lignes = terrain.Lignes;
        int colonnes = terrain.Colonnes;
        var aPropager = new List<(int i, int j)>();
        
        // Collecte positions actuelles de mauvaises herbes
        for (int i = 0; i < lignes; i++)
            for (int j = 0; j < colonnes; j++)
                if (terrain.grille[i, j] is MauvaiseHerbe)
                    aPropager.Add((i, j));

        // Pour chaque plante, essayer d'envahir les 8 voisins
        foreach (var (i, j) in aPropager)
        {
            for (int di = -1; di <= 1; di++)
                for (int dj = -1; dj <= 1; dj++)
                {
                    if (di == 0 && dj == 0) continue;
                    int ni = i + di, nj = j + dj;
                    // Vérifier bornes et présence
                    if (ni < 0 || ni >= lignes || nj < 0 || nj >= colonnes) continue;
                    if (terrain.grille[ni, nj] == null)
                    {
                        terrain.Planter(new MauvaiseHerbe(), ni, nj);
                    }
                    else if (!(terrain.grille[ni, nj] is MauvaiseHerbe))
                    {
                        // Tuer la plante existante
                        terrain.SupprimerPlante(ni, nj);
                        terrain.Planter(new MauvaiseHerbe(), ni, nj);
                    }
                }
        }
    }

}


