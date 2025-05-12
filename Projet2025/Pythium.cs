public class Pythium : Maladies
{
    public Pythium() : base(nom : "Phylloxera", prob : 10 )
    {}

    public void Affaiblir(Plantes plante)
    {
        if(plante.croissanceActuelle < 0.5) // Ralenti la croissance des graines 
        {
            plante.VitesseCroissance -= 0.1;
            Console.WriteLine("La graine pousse moins vite");
        }
    }
}