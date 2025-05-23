public abstract class Plantes 
{
    public enum NaturePlante
    {
        Annuelle,
        Vivace
    }
    
    public enum Saisons
    {
        Hiver, Printemps, Eté, Automne
    }

    public enum TypeTerrain
    {
        Terre, Argile, Sable
    }

    // Infos génériques (pas de variations)
    private NaturePlante Nature { get; init; }
    public string Nom {get;}
    public List<Saisons> SaisonsSemi {get;} 
    public TypeTerrain TerrainPref {get;}
    public int Espacement {get;} //en nombre de cases
    public int Place {get;}  // pareil
    public double VitesseCroissance {get; set;}
    public int BesoinEau {get;} // echelle de 1 à 100
    public int BesoinLum {get;}
    public double TempMax {get;}
    public double TempMin {get;}
    public List<Maladies> ListeMaladies {get; set;}
    public int EsperenceVie {get;} // en mois mais à convertir en semaine -> depend de l'unité d'un tour (semaines)
    public int NbFruitsMax {get;} // nb de fruits produits par le semi au maximum
    public int NbFruitsSemaine {get;} // nb de fruits produits par semaine (tour), si on est dans la bonne saison
    public Saisons SaisonFruits {get;} // Saison durant laquelle poussent les fruits


    //Infos dépendants de la simulation (tours de jeu)

    public double nbFruitsActuel = 0; // nombre de fruits lors de la croissance de la plante
    public double croissanceActuelle = 0;
    public int ageSemaines = 0; //meme unité qu'un tour
    public double eauRecu = 0; // A gerer avec la classe Météo OU avec Terrain suivant l'absorption
    public double lumRecu = 0; // A gerer avec la classe Météo
    public int coordX;  // Coordonnées sur le terrain (lorsque la plante est plantée) -> pas utile pour l'instant
    public int coordY;
    public TypeTerrain terrainActuel;
    public double tempActuelle; //tempActuelle = Meteo.Temperature -> dépend de la classe Meteo
    private int esperenceVieSemaines => EsperenceVie * 4; // ~4 semaines/mois
    public bool estMalade = false;
    public bool estMature = false;
    public bool estMorte = false;

    protected Plantes(string nom, List<Saisons> saisonsSemi, TypeTerrain terrainPref, int espacement, int place, double vitesseCroissance, int besoinEau, int besoinLum, double tempMax, double tempMin, List<Maladies> listeMaladies, int esperenceVie, int nbFruitsMax, int nbFruitsSemaine, NaturePlante nature, Saisons saisonFruits) {

        Nom = nom;
        SaisonsSemi = saisonsSemi;
        TerrainPref = terrainPref;
        Espacement = espacement;
        Place = place;
        VitesseCroissance = vitesseCroissance;  //en unite/tour
        BesoinEau = besoinEau;  //echelle sur 100
        BesoinLum = besoinLum;
        TempMax = tempMax;
        TempMin = tempMin;
        ListeMaladies = listeMaladies;
        EsperenceVie = esperenceVie;    
        NbFruitsMax = nbFruitsMax;
        Nature = nature;
        SaisonFruits = saisonFruits;
        NbFruitsSemaine = nbFruitsSemaine;
    }

     public void Pousser() //Appelé à chaque tour de jeu (simulation)
     {
        if (!estMalade && !estMature && !estMorte) //Si la plante est pas malade ni mature (si elle est mature, plus besoin de pousser)
        {   
            if (VerifierConditionsPref())
            {
                // Si les conditions sont ok (au - 50%) elle pousse normalement
                croissanceActuelle += VitesseCroissance;
            }
            else
                // Sinon ça ralentit la croissance
                croissanceActuelle += VitesseCroissance/2;

            if (croissanceActuelle >= 1) 
                estMature = true;
            
            Console.WriteLine($"{Nom} a poussé, croissance actuelle : {croissanceActuelle}");
        }
        
        ageSemaines ++; // 1 semaine ajoutée

        if (ageSemaines >= esperenceVieSemaines)    //Si esperence de vie (age max) atteint
        {
            if (Nature == NaturePlante.Annuelle)
            {
                estMorte = true; // Puis la plante est supprimée du terrain -> voir Terrains -> VerifierEtatPlantes()
                Console.WriteLine($"{Nom} est en fin de vie.");
            }
            else // Si elle est vivace, elle reprendra sa croissance au printemps prochain /!\
            {
                nbFruitsActuel = 0;
                croissanceActuelle = 0; 
                ageSemaines = 0;
                estMalade = false;
                estMature = false;
                Console.WriteLine($"{Nom} est en fin de vie mais comme elle est vivace, elle reprend sa croissance depuis le début.");
            }
        }
    }
    public void DonnerFruit()
    {
        nbFruitsActuel += NbFruitsSemaine;
    }

     public virtual bool VerifierConditionsPref()   // (de pousse)
     {
        int nbConditionsValides = 0;  
        int nbConditionsMax = 4;
        double ratio;
        tempActuelle = ContexteSimulation.TempEnCours;

        if (terrainActuel == TerrainPref)   //Terrain OK
            nbConditionsValides ++;
        
        if (eauRecu/BesoinEau >= 1)     //Assez d'eau
            nbConditionsValides ++;

        if (lumRecu/BesoinLum >= 1)     //Assez de lumière
            nbConditionsValides ++;

        if ((tempActuelle >= TempMin) && (tempActuelle <= TempMax))     //Température OK
            nbConditionsValides ++; 

        ratio = nbConditionsValides/nbConditionsMax;

        if (ratio >= 0.5)
            return true;
        else
            return false;
     }

    public virtual string GetSymboleConsole()
    {
        if (croissanceActuelle < 0.5) 
            return " . ";   //plante juste semée
        
        else if (croissanceActuelle < 1) 
            return $" {Nom[0]} ";   // initiale de la plante
        
        else // maturité → fruit ou plante adulte (croissanceActuelle >= 1)
            return Nom switch
            {
                "Pomme"  => " 🍎",
                "Fraise" => " 🍓",
                "MauvaiseHerbe" => " 🌿",
                "Kiwi" => " 🥝",
                "Poire" => " 🍐",
                "Mangue" => " 🥭",
                "Pasteque" => " 🍉",
                _        => " ★ "
            };
    }
}