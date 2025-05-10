public class Abeille : AnimauxUtiles
{
    public Abeille() : base (habitat : TypeTerrain.Terre, nomA : "Abeille", bienfait : 2)
    {
    
    }

    public void Butiner(Plantes plantes)  // Ajoute 4 fruits sinon augmente la taille de la plante
    {
        if(plantes.NbFruitsMax > plantes.nbFruitsActuel + Bienfait && plantes.nbFruitsActuel != 0) // si la plante à des fruits
        {
            plantes.nbFruitsActuel += Bienfait;
            Console.WriteLine("De nouveaux fruits ont poussé sur la plante ");
        }
        else if(plantes.estMature) // si la plante est mature 
        {
            plantes.croissanceActuelle *= 0.2;
            Console.WriteLine("La plante a grandie");
        }
    } 
        
    
    
}