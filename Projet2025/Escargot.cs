public class Escargot : AnimauxNuisible
{
    public Escargot() : base(nomA : "Escargot", degat : 2)
    {}
    public override void Nuire(Terrains terrain) // L'escargot réduit la taille de la plante
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            if (p.coordX == posX && p.coordY == posY) // Cherche la plante qui est sur la même position que l'animal
            {
                if (!p.estMature)
                {
                    p.croissanceActuelle /= Degat;
                    Console.WriteLine("L'escargot à grignotter votre plante, sa taille est réduite'");
                }
            }
        }
    }
}