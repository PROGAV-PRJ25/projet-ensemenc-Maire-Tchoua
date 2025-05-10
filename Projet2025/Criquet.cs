public class Criquet : AnimauxNuisible
{
    public Criquet() : base(TypeTerrain.Sable, nomA : "Criquet", degat : 3)
    {}

    public void Attaquer(Plantes plante) // Divise par 4 la vitesse de croissance de la plante
    {
        if (plante.croissanceActuelle > 0.5 && !plante.estMature) // La plante n'est pas encore mature (0.5 < croissanceActuelle < 1)
        {    
            plante.VitesseCroissance /= Degat;
            Console.WriteLine("La plante pousse moins vite");
        }
    }
}