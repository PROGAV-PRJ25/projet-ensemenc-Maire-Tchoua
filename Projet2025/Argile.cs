public class Argile : Terrains
{
    public int Absorption {get; set;}
    public Argile(int nbrPlantes): base(nomTerrain : "Argile", nbrPlantes)
    {
        this.Absorption = 3;
    }
}