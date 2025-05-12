public class Abeille : AnimauxUtiles
{
    public Abeille() : base (nomA : "Abeille", bienfait : 4)
    {
    
    }

    public override void Aider(Terrains terrain)  // Ajoute 4 fruits sinon augmente la taille de la plante
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(Math.Abs(p.coordX - posX) <= 1 && Math.Abs(p.coordY - posY) <= 1) // L'abeille butines la plante p et les plantes adjacentes
            {
                if (p.estMature)
                {
                    if (p.nbFruitsActuel <= p.NbFruitsMax - 4)
                        p.nbFruitsActuel += Bienfait;
                }
                else //si plante pas mature
                {
                    p.croissanceActuelle += Bienfait/10;    //croissance augmente de 0.4
                }
            }
        }  
       
    } 
        
    
    
}