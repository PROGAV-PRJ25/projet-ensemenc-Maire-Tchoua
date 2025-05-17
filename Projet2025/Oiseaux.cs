public class Oiseaux : AnimauxNuisible
{
    public Oiseaux() : base(nomA : "Oiseaux", degat : 2)
    {}

    public override void Nuire(Terrains terrain) // Mange 2 fruit 
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if (p.coordX == posX && p.coordY == posY)
            {
                if (p.estMature && p.nbFruitsActuel >= Degat)
                {
                    p.nbFruitsActuel -= Degat;  
                    Console.WriteLine("L'oiseaux à mangé 2 fruits");
                }
            }
        }
       
    }
}