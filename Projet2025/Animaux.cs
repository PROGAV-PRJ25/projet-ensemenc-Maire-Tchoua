public abstract class Animaux
{
    public string NomA {get;} //Lecture seule
    public int posX; //Coordonnées de l'animal sur le terrain
    public int posY;

    protected Animaux(string nomA)
    {
        NomA = nomA;
    }

    public virtual void Nuire(Terrains terrain) {}
    public virtual void Aider(Terrains terrain) {}

    public virtual string GetSymboleConsole() // Permet d'avoir le symbole de l'animal
    {
        return NomA switch
        {
            "VerDeTerre"  => "🪱",
            "Criquet" => "🦗",
            "Oiseaux" => "🐦",
            "Escargot" => "🐌",
            "Abeille" => "🐝",
        };
    }
}