public class Anthracnose : Maladies 
{
    public Anthracnose() : base(nom : "Anthracnose", prob : 30, dureeConta : 10 )
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