public class Mangue : Plantes {

    public Mangue() : base(
        nom : "Mangue", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps}, 
        terrainPref : TypeTerrain.Sable, 
        espacement : 1, 
        place : 3, 
        vitesseCroissance : 0.7, 
        besoinEau : 80, 
        besoinLum : 9, 
        tempMax : 35, 
        tempMin : 15, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 180, 
        nbFruitsMax : 15,
        nbFruitsSemaine : 1,
        nature : NaturePlante.Vivace,
        saisonFruits : Saisons.Eté
        )
    {}


}