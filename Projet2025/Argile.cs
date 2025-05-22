public class Argile : Terrains
{
    public Argile(int lignes, int colonnes) : base(lignes, colonnes, Plantes.TypeTerrain.Argile, nivEau : 100, absorption : 0.8, capaciteEauMax : 120)
    {}
}
