public class Criquet : AnimauxNuisible
{
    public Criquet() : base(nomA : "Criquet", degat : 3)
    {}
    public override void Nuire(Terrains terrain) // Divise par 3 la vitesse de croissance de la plante
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(Math.Abs(p.coordX - posX) <= 1 && Math.Abs(p.coordY - posY) <= 1)
            {
                if( p.croissanceActuelle > 0.5 && !p.estMature)
                {    
                    p.VitesseCroissance /= Degat;
                    Console.WriteLine("Le criquet a affaibli votre plante, elle pousse moins vite");
                }
            }
        }          
    }
}