public abstract class Maladies {

    public string Nom {get; set;}
    public double ProbabiliteContamination { get; set; }

    public int posX;
    public int posY;

    public Maladies(string nom, double prob)
    {
        Nom = nom;
        ProbabiliteContamination = prob;
    }
    
    public void Propager()
    {
        Random rnd = new Random();
        int x  = rnd.Next(0, 1); // Coordonnées x,y de l'animal
        int y = rnd.Next(0, 1);
        
        posX += x;
        posY += y;
        
        Console.WriteLine($"L'animal s'est propagée sur cette position : Ligne={posX}, Colonne={posY}");

    }
}