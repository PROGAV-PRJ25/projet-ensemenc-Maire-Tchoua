public class Sable : Terrains
{
    public Sable(int lignes, int colonnes) : base(lignes, colonnes, Plantes.TypeTerrain.Sable, nivEau : 50, absorption : 0.3, capaciteEauMax : 80)
    {}
}
