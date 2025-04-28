public class Escargot : AnimauxNuisible
{

    public Escargot() : base(TypeTerrain.Terre, nomA : "Escargot", degat : 0.5)
    {}

    public void Grignotter(Plantes plante) // L'escargot réduit la taille de la plante
    {
        plante.CroissanceActuelle -= Degat;
        Console.WriteLine("Votre plante grandit moins vite");
    }
}