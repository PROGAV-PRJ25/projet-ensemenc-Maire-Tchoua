public class MauvaiseHerbe : Plantes
{

    private static List<Plantes.Saisons> ToutesSaisons =
        new List<Plantes.Saisons> {
            Plantes.Saisons.Hiver, Plantes.Saisons.Printemps,
            Plantes.Saisons.Eté,   Plantes.Saisons.Automne
        };

    public MauvaiseHerbe() : base(
        nom: "MauvaiseHerbe",
        saisonsSemi: ToutesSaisons,
        terrainPref: TypeTerrain.Terre, //ignoré, pousse sur tous terrain
        espacement: 0, //pas d'espacement requis
        place: 1,
        vitesseCroissance: 1, //inutile, pas besoin de temps
        besoinEau: 1,
        besoinLum: 1,
        tempMax: 100,
        tempMin: 0,
        listeMaladies: new List<Maladies>(),
        esperenceVie: 12,
        nbFruitsMax: 0,
        nbFruitsSemaine : 0,
        nature: NaturePlante.Annuelle,
        saisonFruits: Saisons.Hiver    //ignoré ne donne pas de fruits
        )
    {
        croissanceActuelle = 1; //Directement mature
        estMature = true;
    }




}


