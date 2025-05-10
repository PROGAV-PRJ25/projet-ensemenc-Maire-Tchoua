public abstract class Maladies {

    public string Nom {get; set;}
    public double ProbabiliteContamination { get; set; }

    public Maladies(string nom, double prob)
    {
        Nom = nom;
        ProbabiliteContamination = prob;
    }
    
}