public abstract class AnimauxUtiles : Animaux
{
    public int Bienfait {get; set;}
    public AnimauxUtiles(string nomA, int bienfait) : base(nomA)
    {
        Bienfait = bienfait;
    }
    public override abstract void Aider(Terrains terrain);
}