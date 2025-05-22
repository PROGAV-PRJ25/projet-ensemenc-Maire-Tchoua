public abstract class Maladies {

    public string Nom {get; set;}
    public double ProbabiliteContamination {get; set;}
    public double DureeContamination {get; set;}
    public int posX;
    public int posY;

    public Maladies(string nom, double prob, double dureeConta)
    {
        Nom = nom;
        ProbabiliteContamination = prob;
        DureeContamination = dureeConta;
    }
    
    public void Propager(Terrains terrain)
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
        
        Console.WriteLine($"{Nom} s'est propagée sur cette position : ({posX},{posY})");
    }
}