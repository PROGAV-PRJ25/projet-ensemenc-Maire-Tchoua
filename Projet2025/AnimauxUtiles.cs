public abstract class AnimauxUtiles : Animaux
{
    public int Bienfait {get; set;}
    public AnimauxUtiles(int nbrPlantesAttaquees, TypeTerrain habitat, int bienfait) : base(nbrPlantesAttaquees, habitat)
    {
        Bienfait = bienfait;
    }
}