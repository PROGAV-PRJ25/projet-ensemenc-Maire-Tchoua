public class Abeille : AnimauxUtiles
{
    public Abeille() : base (nomA : "Abeille", bienfait : 2)
    {}

    public override void Aider(Terrains terrain)  // Ajoute 4 fruits sinon augmente la taille de la plante
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(Math.Abs(p.coordX - posX) <= 1 && Math.Abs(p.coordY - posY) <= 1) // L'abeille butines la plante p et les plantes adjacentes
            {
                if (p.nbFruitsActuel != 0)
                {
                    if (p.NbFruitsMax > p.nbFruitsActuel + Bienfait)
                    {   
                        p.nbFruitsActuel += Bienfait;
                        Console.WriteLine("De nouveaux fruits ont poussé sur la plante ");
                    }
                }
                else //si plante pas mature
                {
                    p.croissanceActuelle += Bienfait/10;    //croissance augmente de 0.4
                    Console.WriteLine("La plante a grandie");
                }
            }
        }  
    }  
}