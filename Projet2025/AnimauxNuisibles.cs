public class AnimauxNuisible : Animaux
{
    public int Degat {get; set;}
    public AnimauxNuisible(int nbrPlantesAttaquees, TypeTerrain habitat, int degat) : base(nbrPlantesAttaquees, habitat)
    {
        Degat = degat;
    }
}