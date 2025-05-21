public class Kiwi : Plantes {

    public Kiwi() : base(
        nom : "Kiwi", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps}, 
        terrainPref : TypeTerrain.Terre, 
        espacement : 0, 
        place : 1, 
        vitesseCroissance : 0.7, 
        besoinEau : 60, 
        besoinLum : 7, 
        tempMax : 30, 
        tempMin : 5, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 180, 
        nbFruitsMax : 12,
        nbFruitsSemaine : 2,
        nature : NaturePlante.Vivace,
        saisonFruits : Saisons.Eté
        )
    {}


}