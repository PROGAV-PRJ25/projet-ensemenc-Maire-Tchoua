public abstract class AnimauxUtiles : Animaux
{
    public int Bienfait {get; set;}
    public AnimauxUtiles(TypeTerrain habitat, string nomA, int bienfait) : base(habitat, nomA)
    {
        Bienfait = bienfait;
    }
}