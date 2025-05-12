public abstract class Animaux
{
    
    //public enum TypeTerrain {Terre, Argile, Sable}
    //public TypeTerrain Habitat {get; set;}
    public string NomA {get; set;}
    public int posX; //Coordonnées de l'animal sur le terrain
    public int posY;

    public Animaux(string nomA)
    {
        NomA = nomA;
        //Habitat = habitat;
    }

    public virtual void Nuire(Terrains terrain) {}
    public virtual void Aider(Terrains terrain) {}


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