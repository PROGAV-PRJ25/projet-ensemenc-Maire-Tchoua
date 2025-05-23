public abstract class AnimauxNuisible : Animaux
{
    public double Degat {get; set;}
    public AnimauxNuisible(string nomA, double degat) : base(nomA)
    {
        Degat = degat;
    }

    public override abstract void Nuire(Terrains terrain);

    public void Deplacer(Terrains terrain)
    {
        Random rnd = new Random();
        int newX, newY;
        do
        {
            newX = posX + rnd.Next(-1, 2); // -1, 0 ou 1
            newY = posY + rnd.Next(-1, 2);
        }
        while (newX < 0 || newX >= terrain.Lignes || newY < 0 || newY >= terrain.Colonnes);

        // Si on arrive ici, les coordonnées sont valides
        posX = newX;
        posY = newY;
        
        Console.WriteLine($"{NomA} s'est déplacé(e) sur cette position : ({posX},{posY})"); 
    }
}