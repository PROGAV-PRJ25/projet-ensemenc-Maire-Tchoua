public abstract class Animaux
{
    
    public enum TypeTerrain {Terre, Argile, Sable}
    public TypeTerrain Habitat {get; set;}
    public string NomA {get; set;}

    public int coordX;
    public int coordY;
    

    public Animaux(TypeTerrain habitat, string nomA)
    {
        NomA = nomA;
        Habitat = habitat;
         
    }

    public void Deplacer(Animaux animal)
    {
        Random rnd = new Random();
        int posx  = rnd.Next(0, 1); // Coordonnées x,y de l'animal
        int posy = rnd.Next(0, 1);
        
        animal.coordX += posx;
        animal.coordY += posy;
        
        Console.WriteLine($"L'animal est sur cette position : Ligne={animal.coordX}, Colonne={animal.coordY}"); 
    }

    public virtual string GetSymboleConsole() // Pas utilisé, permet d'avoir le symbole de l'animal
    {
        return NomA switch
        {
            "VerDeTerre"  => "🪱",
            "Criquet" => "🦗",
            "Oiseaux" => "🐦",
            "Escargot" => "🐌 ",
            "Abeille" => "🐝",
        };
    }
}