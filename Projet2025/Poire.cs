public class Poire : Plantes {

    public Poire() : base(
        nom : "Poire", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps, Saisons.Automne}, 
        terrainPref : TypeTerrain.Terre, 
        espacement : 2, 
        place : 1, 
        vitesseCroissance : 0.3, 
        besoinEau : 45, 
        besoinLum : 5, 
        tempMax : 30, 
        tempMin : 3, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 240, 
        nbFruitsMax : 50,
        nbFruitsSemaine : 2,
        nature : NaturePlante.Vivace,
        saisonFruits : Saisons.Automne
        )
    {}


}