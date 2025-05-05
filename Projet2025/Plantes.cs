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
    public NaturePlante Nature { get; init; }
    public string Nom {get;}
    public List<Saisons> SaisonsSemi {get; set;} 
    public TypeTerrain TerrainPref {get; set;}
    public int Espacement {get; set;} //en nombre de cases
    public int Place {get; set;}    // pareil
    public double VitesseCroissance {get; set;}
    public int BesoinEau {get; set;} // echelle de 1 à 100
    public int BesoinLum {get; set;}
    public double TempMax {get; set;}
    public double TempMin {get; set;}
    public List<Maladies> ListeMaladies {get; set;}
    public int EsperenceVie {get; set;} // en mois mais à convertir en semaine -> depend de l'unité d'un tour (semaines)
    public int NbFruitsMax {get; set;} // nb de fruits produits par le semi au maximum

    public Saisons SaisonsFruits {get; set;}

    //Infos dépendants de la simulation (tours de jeu)

    public double nbFruitsActuel = 0; // nombre de fruits lors de la croissance de la plante
    public double croissanceActuelle = 0;
    public int ageSemaines = 0; //meme unité qu'un tour
    public double eauRecu = 0; // A gerer avec la classe Météo OU avec Terrain suivant l'absorption
    public int lumRecu = 0; // A gerer avec la classe Météo
    public int coordX;  // Coordonnées sur le terrain (lorsque la plante est plantée) -> pas utile pour l'instant
    public int coordY;
    public TypeTerrain terrainActuel;
    public double tempActuelle; //tempActuelle = Meteo.Temperature -> dépend de la classe Meteo
    private int esperenceVieSemaines => EsperenceVie * 4; // ~4 semaines/mois
    public bool estMalade = false;
    public bool estMature = false;
    public bool estMorte = false;


    protected Plantes(string nom, List<Saisons> saisonsSemi, TypeTerrain terrainPref, int espacement, int place, double vitesseCroissance, int besoinEau, int besoinLum, double tempMax, double tempMin, List<Maladies> listeMaladies, int esperenceVie, int nbFruitsMax, NaturePlante nature, Saisons saisonsFruits) {

        Nom = nom;
        SaisonsSemi = saisonsSemi;
        TerrainPref = terrainPref;
        Espacement = espacement;
        Place = place;
        VitesseCroissance = vitesseCroissance;  //en unite/tour
        BesoinEau = besoinEau;  //echelle sur 10
        BesoinLum = besoinLum;
        TempMax = tempMax;
        TempMin = tempMin;
        ListeMaladies = listeMaladies;
        EsperenceVie = esperenceVie;    
        NbFruitsMax = nbFruitsMax;
        Nature = nature;
        SaisonsFruits = saisonsFruits;
    }

     public virtual void Pousser() //Appelé à chaque tour de jeu (simulation)
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
                //Supp la plante dans le terrain -> A gerer dans classe Terrain -> VerifierEtatPlantes()
                estMorte = true;
                Console.WriteLine($"{Nom} est en fin de vie.");
            }
            else // Si elle est vivace, elle reprendra sa croissance au printemps prochain /!\
            {
                nbFruitsActuel = 0;
                croissanceActuelle = 0; 
                ageSemaines = 0;
                estMalade = false;
                estMature = false;
            }
        }

        if(SaisonsFruits == ContexteSimulation.SaisonEnCours && estMature==true && nbFruitsActuel < NbFruitsMax)
        {
            if(Nom == "Fraise")
                nbFruitsActuel += 5;
            if(Nom == "Pomme")
                nbFruitsActuel += 2;
        }
     }

     public virtual bool VerifierConditionsPref()   // (de pousse)
     {
        int nbConditionsValides = 0;    //Faire le ration sur le nb de conditions totales à valider : besoin en eau et lumière, terrain pref, temp pref, 
        int nbConditionsMax = 4;
        double ratio;

        if (terrainActuel == TerrainPref)   //Terrain OK
            nbConditionsValides ++;
        
        if (eauRecu/BesoinEau >= 1)     //Assez d'eau
            nbConditionsValides ++;

        if (lumRecu/BesoinLum >= 1)     //Assez de lumière
            nbConditionsValides ++;

        if ((tempActuelle >= TempMin) && (tempActuelle <= TempMax))     //Temp OK
            nbConditionsValides ++; 

        ratio = nbConditionsValides/nbConditionsMax;

        if (ratio >= 0.5)
            return true;
        else
            return false;
     }

    public virtual string GetSymboleConsole()
    {
        if (croissanceActuelle == 0) 
            return " . ";   //plante juste semée
        
        if (croissanceActuelle < 0.8) 
            return $" {Nom[0]} ";   // initiale de la plante
        
        else // maturité → fruit ou plante adulte (croissanceActuelle >= 1)
            return Nom switch
            {
                "Pomme"  => " 🍎",
                "Fraise" => " 🍓",
                "MauvaiseHerbe" => " 🌿",
                _        => " ★ "
            };
    }

     //Gerer les fin de saisons, plantes vivace vs annuelles





}