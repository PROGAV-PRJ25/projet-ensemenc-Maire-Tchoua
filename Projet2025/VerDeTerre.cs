public class VerDeTerre : AnimauxUtiles
{
    public VerDeTerre() : base (nomA : "VerDeTerre", bienfait : 2)
    {
    
    }

    public override void Aider(Terrains terrain) 
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(Math.Abs(p.coordX - posX) <= 1 && Math.Abs(p.coordY - posY) <= 1)
            {
                if (!p.estMature)
                {
                    p.VitesseCroissance += Bienfait/10 ;    //Plante pousse + vite (0.2)
                    Console.WriteLine("La plante pousse moins vite");
                }
            }
        } 
        
    }   
        
    
    
}