public class Abeille : AnimauxUtiles
{
    public Abeille() : base (habitat : TypeTerrain.Terre, nomA : "Abeille", bienfait : 2)
    {
    
    }

    public void Butiner(Plantes plantes)  // Ajoute 4 fruits sinon augmente la taille de la plante
    {
        if(plantes.estMature && plantes.NbFruitsMax >  plantes.nbFruitsActuel + Bienfait)
        {
            plantes.nbFruitsActuel += Bienfait;
        }
        if(!plantes.estMature)
        {
            plantes.croissanceActuelle *= 0.2;
        }
    } 
        
    
    
}