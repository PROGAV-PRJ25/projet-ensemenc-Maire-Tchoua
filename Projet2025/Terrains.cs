public abstract class Terrains
{
    public int NbrPlantes {get;set;}
    public List<Plantes> ListePlantes;
    public int NivEau {get; set;}

    public string NomTerrain {get; set;}
    
    public Terrains(string nomTerrain, int nbrPlantes)
    {
        NomTerrain = nomTerrain;
        NbrPlantes = nbrPlantes;
        NivEau = 10;
         
    }
}