public abstract class AnimauxNuisible : Animaux
{
    public double Degat {get; set;}
    public AnimauxNuisible(string nomA, double degat) : base(nomA)
    {
        Degat = degat;
    }

    public override abstract void Nuire(Terrains terrain);

    public void Deplacer(AnimauxNuisible animal)
    {
        Random rnd = new Random();
        int x  = rnd.Next(0, 1); // Coordonnées x,y de l'animal
        int y = rnd.Next(0, 1);
        
        animal.posX += x;
        animal.posY += y;
        
        Console.WriteLine($"L'animal est sur cette position : Ligne={animal.posX}, Colonne={animal.posY}"); 
    }
}