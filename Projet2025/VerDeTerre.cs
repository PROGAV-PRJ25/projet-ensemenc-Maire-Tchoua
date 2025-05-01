public class VerDeTerre : AnimauxUtiles
{
    public VerDeTerre() : base (habitat : TypeTerrain.Terre, nomA : "VerDeTerre", bienfait : 5)
    {
    
    }

    public void Remuer(Plantes plante) 
    {
        plante.VitesseCroissance += 0.2;
    }   
        
    
    
}