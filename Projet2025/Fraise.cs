public class Fraise : Plantes {

    public Fraise() : base(
        nom : "Fraise", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps, Saisons.Eté}, 
        terrainPref : TypeTerrain.Sable, 
        espacement : 1, 
        place : 1, 
        vitesseCroissance : 0.3, 
        besoinEau : 40, 
        besoinLum : 50, 
        tempMax : 28, 
        tempMin : 12, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 14, 
        nbFruitsMax : 100,
        nature : NaturePlante.Vivace,
        saisonFruits : Saisons.Printemps
        )
    {}


}