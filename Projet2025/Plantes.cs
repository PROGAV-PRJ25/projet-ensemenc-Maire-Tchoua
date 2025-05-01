public abstract class Plantes 
{
    
    public enum NaturePlante
    {
        Annuelle,
        Vivace
    }
    
    public enum Saisons
    {
        Hiver, Printemps, Eté, Automne
    }

    public enum TypeTerrain
    {
        Terre, Argile, Sable
    }

    // Infos génériques (pas de variations)
    public NaturePlante Nature { get; init; }
    public string Nom {get;}
    public List<Saisons> SaisonsSemi {get; set;} 
    public TypeTerrain TerrainPref {get; set;}
    public int Espacement {get; set;} //en nombre de cases
    public int Place {get; set;}    // pareil
    public double VitesseCroissance {get; set;}
    public int BesoinEau {get; set;} // faire une echelle
    public int BesoinLum {get; set;}
    public double TempMax {get; set;}
    public double TempMin {get; set;}
    public List<Maladies> ListeMaladies {get; set;}
    public int EsperenceVie {get; set;} // en mois
    public int NbFruitsMax {get; set;} // nb de fruits produits par le semi au maximum

    public double NbFruitsActuel {get; set;} // nombre de fruits lors de la croissance de la plante
    public double CroissanceActuelle {get; set;}
    public int EauRecu {get; set;} // pas utile pour l'instant
    public int LumRecu{get; set;} // pas utile pour l'instant
    public bool EstMalade {get; set;}

    public int coordX;  // Coordonnées sur le terrain (lorsque la plante est plantée) -> pas utile pour l'instant
    public int coordY;
    public TypeTerrain terrainActuel;
    public double tempActuelle; //tempActuelle = Meteo.Temperature -> dépend de la classe Meteo

    protected Plantes(string nom, List<Saisons> saisonsSemi, TypeTerrain terrainPref, int espacement, int place, double vitesseCroissance, int besoinEau, int besoinLum, double tempMax, double tempMin, List<Maladies> listeMaladies, int esperenceVie, int nbFruitsMax, NaturePlante nature) {

        Nom = nom;
        SaisonsSemi = saisonsSemi;
        TerrainPref = terrainPref;
        Espacement = espacement;
        Place = place;
        VitesseCroissance = vitesseCroissance;
        BesoinEau = besoinEau;
        BesoinLum = besoinLum;
        TempMax = tempMax;
        TempMin = tempMin;
        ListeMaladies = listeMaladies;
        EsperenceVie = esperenceVie;
        NbFruitsMax = nbFruitsMax;
        Nature = nature;

        NbFruitsActuel = 0;
        CroissanceActuelle = 0;
        EauRecu = 0;
        LumRecu = 0;
        EstMalade = false;
    }

     public virtual void Pousser()
     {
        if (!EstMalade) //Si la plante est pas malade
        {
            
            if (VerifierConditionsPref())
            {
                // Si les conditions sont ok (au - 50%) elle pousse normalement
                CroissanceActuelle += VitesseCroissance;
            }
            else
                // Sinon ça ralentit la croissance
                CroissanceActuelle += VitesseCroissance/2;
            
            Console.WriteLine($"{Nom} a poussé, croissance actuelle : {CroissanceActuelle}");
        }
     }

     public virtual bool VerifierConditionsPref()   // (de pousse)
     {
        int nbConditionsValides = 0;    //Faire le ration sur le nb de conditions totales à valider : besoin en eau et lumière, terrain pref, temp pref, 
        int nbConditionsMax = 4;
        double ration;

        if (terrainActuel == TerrainPref)   //Terrain OK
            nbConditionsValides ++;
        
        if (EauRecu/BesoinEau >= 1)     //Assez d'eau
            nbConditionsValides ++;

        if (LumRecu/BesoinLum >= 1)     //Assez de lumière
            nbConditionsValides ++;

        if ((tempActuelle >= TempMin) && (tempActuelle <= TempMax))     //Temp OK
            nbConditionsValides ++; 

        ration = nbConditionsValides/nbConditionsMax;

        if (ration >= 0.5)
            return true;
        else
            return false;
     }

    public virtual string GetSymboleConsole()
    {
        //double ratio = CroissanceActuelle / EsperenceVie;
        //if (ratio < 0.33) return "▲";   //plante semée
        //if (ratio < 0.66) return $" {Nom[0]} ";   // initiale de la plante
        // maturité → fruit ou plante adulte
        return Nom switch
        {
            "Pomme"  => " 🍎",
            "Fraise" => " 🍓",
            "MauvaiseHerbe" => " 🌿",
            _        => " ★ "
        };
    }


     //Gerer les fin de saisons, plantes vivace vs annuelles





}