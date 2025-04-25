public class Abeille : AnimauxUtiles
{
    public Abeille() : base (habitat : TypeTerrain.Terre, nomA : "Abeille", bienfait : 4)
    {
    
    }

    public void Butine(Plantes plantes)  // Ajoute 4 fruits sinon augmente la taille de la plante
    {
        if(plantes.NbFruitsMax != 0)
        {
            plantes.NbFruitsActuel += Bienfait;
        }
        else
        {
            plantes.CroissanceActuelle += Bienfait;
        }
    } 
        
    
    
}