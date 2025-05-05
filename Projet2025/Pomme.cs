public class Pomme : Plantes {

    public Pomme() : base(
        nom : "Pomme", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps, Saisons.Automne}, 
        terrainPref : TypeTerrain.Terre, 
        espacement : 2, 
        place : 1, 
        vitesseCroissance : 0.2 , //2 unités de croissance par tour
        besoinEau : 50, 
        besoinLum : 60, 
        tempMax : 30, 
        tempMin : 10, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 24, 
        nbFruitsMax : 50,
        nature : NaturePlante.Vivace
        )
    {}


}