public abstract class Plantes 
{

    public string Nom {get;}
    public List<string> ListeSaisons {get; set;}
    public string TerrainPref {get;}
    public int Espacement {get;}
    public int Place {get;}
    public double VitesseCroissance {get;}
    public int BesoinEau {get;}
    public int BesoinLum {get;}
    public double TempMax {get;}
    public double TempMin {get;}
    public List<string> ListeMaladies {get;} // a changer, List<Maladies>
    public int EsperenceVie {get;} // en mois
    public int NbFruitsMax {get;} // nb de fruits produits par le semi au maximum

    

    
}