public class Pasteque : Plantes {

    public Pasteque() : base(
        nom : "Pasteque", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps}, 
        terrainPref : TypeTerrain.Argile, 
        espacement : 3, 
        place : 2, 
        vitesseCroissance : 0.2, 
        besoinEau : 90, 
        besoinLum : 9, 
        tempMax : 40, 
        tempMin : 15, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 4, 
        nbFruitsMax : 5,
        nbFruitsSemaine : 1,
        nature : NaturePlante.Annuelle,
        saisonFruits : Saisons.Printemps
        )
    {}


}