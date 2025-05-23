public abstract class AnimauxUtiles : Animaux
{
    protected int Bienfait{ get;}
    protected AnimauxUtiles(string nomA, int bienfait) : base(nomA)
    {
        Bienfait = bienfait;
    }
    public override abstract void Aider(Terrains terrain);
}