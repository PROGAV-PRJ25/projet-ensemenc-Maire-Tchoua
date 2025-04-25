public class Criquet : AnimauxNuisible
{
    public Criquet() : base(TypeTerrain.Sable, nomA : "Criquet", degat : 4)
    {}

    public void Affaiblir(Plantes plante) // Divise par 4 la vitesse de croissance de la plante
    {
        plante.VitesseCroissance /= Degat;
    }
}