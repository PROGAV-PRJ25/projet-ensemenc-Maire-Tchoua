public class Oiseaux : AnimauxNuisible
{
    public Oiseaux() : base(TypeTerrain.Argile, nomA : "Oiseaux", degat : 2)
    {}

    public void Picorer(Plantes plante) // Mange 2 fruit 
    {
        if(plante.nbFruitsActuel >= Degat)
        {
            plante.nbFruitsActuel -= Degat;
        }
    }
}