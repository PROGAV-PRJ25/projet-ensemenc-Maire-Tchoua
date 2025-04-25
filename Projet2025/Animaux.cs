public abstract class Animaux
{
    
    public enum TypeTerrain {Terre, Argile, Sable}
    public TypeTerrain Habitat {get; set;}
    public string NomA {get; set;}
    

    public Animaux(TypeTerrain habitat, string nomA)
    {
        NomA = nomA;
        Habitat = habitat;
         
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