public class Terre : Terrains
{
    public Terre(int lignes, int colonnes) : base(lignes, colonnes, Plantes.TypeTerrain.Terre, nivEau : 80, absorption : 0.6, capaciteEauMax : 100)
    {}
    
}
