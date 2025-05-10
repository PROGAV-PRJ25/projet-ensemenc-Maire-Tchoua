public class Escargot : AnimauxNuisible
{

    public Escargot() : base(TypeTerrain.Terre, nomA : "Escargot", degat : 2)
    {}

    public void Grignotter(Plantes plante) // L'escargot réduit la taille de la plante
    {
        if (plante.estMature) // La plante est mature
        {
            plante.croissanceActuelle /= Degat;
            Console.WriteLine("La taille de votre plante est réduite");
        }
    }
}