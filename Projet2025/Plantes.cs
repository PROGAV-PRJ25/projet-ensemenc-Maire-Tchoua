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

    public NaturePlante Nature { get; init; }
    public string Nom {get;}
    public List<Saisons> SaisonsSemi {get; set;} //Enum ?
    public TypeTerrain TerrainPref {get; set;}
    public int Espacement {get; set;} //en nombre de cases
    public int Place {get; set;}    // pareil
    public double VitesseCroissance {get; set;}
    public int BesoinEau {get; set;} // faire une echelle
    public int BesoinLum {get; set;}
    public double TempMax {get; set;}
    public double TempMin {get; set;}
    public List<Maladies> ListeMaladies {get; set;} // a changer, List<Maladies>
    public int EsperenceVie {get; set;} // en mois
    public int NbFruitsMax {get; set;} // nb de fruits produits par le semi au maximum

    public double CroissanceActuelle {get; set;}
    public int EauDisponible {get; set;}
    public bool EstMalade {get; set;}


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

        CroissanceActuelle = 0;
        EauDisponible = 0;
        EstMalade = false;
    }

     public virtual void Pousser()
     {
        // Si les conditions sont ok 
        CroissanceActuelle += VitesseCroissance;
        Console.WriteLine($"{Nom} a poussé, croissance actuelle : {CroissanceActuelle}");

     }





}