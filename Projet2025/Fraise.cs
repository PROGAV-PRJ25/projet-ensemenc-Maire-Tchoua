public class Fraise : Plantes {

    public Fraise() : base(
        nom : "Fraise", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps, Saisons.Automne}, 
        terrainPref : TypeTerrain.Terre, 
        espacement : 2, 
        place : 1, 
        vitesseCroissance : 2 , //2 unités de croissance par tour
        besoinEau : 5, 
        besoinLum : 6, 
        tempMax : 30, 
        tempMin : 10, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 24, 
        nbFruitsMax : 50,
        nature : NaturePlante.Vivace
        )
    {}


}