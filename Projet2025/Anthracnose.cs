public class Anthracnose : Maladies 
{
    public Anthracnose() : base(nom : "Moniliose", prob : 30 )
    {}

    public void Pourrir(Plantes plante)
    {
        if(plante.nbFruitsActuel != 0)
        {
            plante.nbFruitsActuel -= 0.2; // les fruits commencent à pourrir petit à petit 
            Console.WriteLine("Les fruits commencent à pourrir");
        }
    }

}