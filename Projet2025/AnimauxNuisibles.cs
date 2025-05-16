public abstract class AnimauxNuisible : Animaux
{
    public double Degat {get; set;}
    public AnimauxNuisible(string nomA, double degat) : base(nomA)
    {
        Degat = degat;
    }

    public override abstract void Nuire(Terrains terrain);

    public void Deplacer()
    {
        Random rnd = new Random();
        int x  = rnd.Next(0, 1); // Coordonnées x,y de l'animal
        int y = rnd.Next(0, 1);
        
        posX += x;
        posY += y;
        
        Console.WriteLine($"L'animal s'est déplacé sur cette position : Ligne={posX}, Colonne={posY}"); 
    }
}