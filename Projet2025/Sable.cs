public class Sable : Terrains
{
    public int Absorption {get; set;}
    public Sable(int nbrPlantes): base(nomTerrain:"Sable", nbrPlantes)
    {
        this.Absorption = 2;
    }
}