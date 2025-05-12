public abstract class AnimauxNuisible : Animaux
{
    public double Degat {get; set;}
    public AnimauxNuisible(string nomA, double degat) : base(nomA)
    {
        Degat = degat;
    }

    public override abstract void Nuire(Terrains terrain);

    
}