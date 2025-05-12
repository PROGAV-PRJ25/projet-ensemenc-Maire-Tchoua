public class Criquet : AnimauxNuisible
{
    public Criquet() : base(nomA : "Criquet", degat : 4)
    {}

    public override void Nuire(Terrains terrain) // Divise par 4 la vitesse de croissance de la plante
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if(Math.Abs(p.coordX - posX) <= 1 && Math.Abs(p.coordY - posY) <= 1)
            {
                if (!p.estMature)
                    p.VitesseCroissance /= Degat;
            }
        }  

        
    }
}