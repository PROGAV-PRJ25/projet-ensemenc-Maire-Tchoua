public class Fraise : Plantes {

    public Fraise() : base(
        nom : "Fraise", 
        saisonsSemi : new List<Saisons> {Saisons.Printemps, Saisons.Eté}, 
        terrainPref : TypeTerrain.Sable, 
        espacement : 1, 
        place : 1, 
        vitesseCroissance : 2.5 , 
        besoinEau : 4, 
        besoinLum : 5, 
        tempMax : 28, 
        tempMin : 12, 
        listeMaladies : new List<Maladies>(), 
        esperenceVie : 14, 
        nbFruitsMax : 100,
        nature : NaturePlante.Vivace
        )
    {}


}