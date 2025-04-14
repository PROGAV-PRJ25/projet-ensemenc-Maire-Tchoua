public class Terre : Terrains
{
    public int Absorption {get; set;}
    public Terre(int nbrPlantes): base(nomTerrain:"Terre", nbrPlantes)
    {
        this.Absorption = 1;
    }
}