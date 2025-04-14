public abstract class Animaux
{
    public int NbrPlantesAttaquees {get; set;}
    public enum TypeTerrain {Terre, Argile, Sable}
    public TypeTerrain Habitat {get; set;}

    public Animaux(int nbrPlantesAttaquees, TypeTerrain habitat)
    {
     
        Habitat = habitat;
        NbrPlantesAttaquees = nbrPlantesAttaquees; 
    }

}