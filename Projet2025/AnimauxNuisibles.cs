public class AnimauxNuisible : Animaux
{
    public double Degat {get; set;}
    public AnimauxNuisible(TypeTerrain habitat, string nomA, double degat) : base(habitat, nomA)
    {
        Degat = degat;
    }

    
}