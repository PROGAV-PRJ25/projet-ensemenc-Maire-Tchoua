using System.Security.Cryptography.X509Certificates;

public class Simulation {

    
    private Random rnd = new Random(); //Pas besoin pour l'instant
    public Plantes.Saisons saisonActuelle;
    public double tempActuelle;
    public List<string> ListePlantes = new List<string> {"pomme","fraise"};   //Liste des types de plantes existantes. A completer en ajoutant les autres plantes!
    public List<string> ListeTerrains = new List<string> {"terre","argile","sable"}; //Liste des types de terrains existants.
   

    public Potager PotagerSimu { get; }
    public Meteo MétéoSimu { get; set; } 
    public DateTime DateCourante { get; private set; } // Date courante dans la simulation (tour = 1 mois)

    public Simulation(Potager potagerSimu , Meteo météoSimu, DateTime dateCourante)
    {
        PotagerSimu = potagerSimu;
        MétéoSimu = météoSimu;
        DateCourante = dateCourante; // new DateTime(2025, 1, 1); // date de départ, par ex. 1er janvier 2025
    }

    private void ModeNormal() // Avance de semaine en semaine
    {
        //Console.Clear();
        
        saisonActuelle = ObtenirSaison(DateCourante);
        ContexteSimulation.SaisonEnCours = saisonActuelle; //MàJ dans ContexteSimulation.cs
        Console.WriteLine($"\n Semaine du {DateCourante: dd MMM yyyy} - Saison : {saisonActuelle}");

        // Le jeu évolue d'une semaine

        GererMeteo();
        foreach (Terrains t in PotagerSimu.ListeTerrains)
        {
            foreach(Plantes p in t.ListePlantes)
            {
                p.Pousser();
                if ((p.SaisonFruits == saisonActuelle) && (p.estMature) && (p.nbFruitsActuel<p.NbFruitsMax)) // /!\ à la saison de récolte des fruits
                {
                    if (p.Nom == "Fraise")
                        p.nbFruitsActuel += 5;    //Si les conditions sont remplies, à chaque tours il peut pousser 5 fraises

                    else if (p.Nom == "Pomme")
                        p.nbFruitsActuel += 2;    //Si les conditions sont remplies, à chaque tours il peut pousser 2 pommes
                }
            }

            t.VerifierEtatPlantes();
        }

        PotagerSimu.AfficherEtat();

        // Menu d'action, le joueur peut effectuer des actions jusqu'à atteindre une limite ou lorsqu'il continuer la simulation
        
        ChoisirAction();

    }

    public void ModeUrgence()
    {
        saisonActuelle = ObtenirSaison(DateCourante);
        ContexteSimulation.SaisonEnCours = saisonActuelle; //MàJ dans ContexteSimulation.cs
        Console.WriteLine("MODE URGENCE ACTIF !");
        Console.WriteLine($"\n Jour {DateCourante: dd MMM yyyy} - Saison : {saisonActuelle}");

        Random rnd = new Random();
        int cont = rnd.Next(0, 101);

        // Avancer de jour en jour

        foreach (Terrains t in PotagerSimu.ListeTerrains)
        {
            // Evolution des maladies
            foreach (Maladies m in t.ListeMaladie)
            {
                if(cont <= m.ProbabiliteContamination)
                {
                    m.Propager();
                }
                PotagerSimu.Contaminer(m,t);
            }

            // Evolution des animaux
            foreach(AnimauxNuisible nuisible in t.ListeAnimauxNuisibles)
            {
                nuisible.Deplacer();
                PotagerSimu.Impacter(nuisible,t);
            }
        }

    }

    public void LancerSimulation() // Ou à faire direct dans le Program.cs
    {
        Console.WriteLine("La simulation va démarrée !");
        bool continuer = true;
        string reponse;

        ModeNormal();   // 1er tour en mode normal (1 semaine passe)

        do
        {
            if (!PotagerSimu.urgenceActive)
            {
                Console.WriteLine("Continuer la simulation ?");
                reponse = Console.ReadLine()!;
                if (reponse == "non" || reponse == "Non")
                { 
                    continuer = false;
                }
                else
                {
                    // Avancer la date de 7 jours (fin de la semaine) :
                    DateCourante = DateCourante.AddDays(7);
                    ModeNormal();   // Relance la simu d'une semaine
                }
            }
            else 
            {
                DateCourante = DateCourante.AddDays(1);
                ModeUrgence();
                ChoisirActionUrgente();
                
                // Continuer simulation ?  
            }

        } while (continuer);
    }

    public Plantes.Saisons ObtenirSaison(DateTime d)
    {
        switch (d.Month)
        {
            case 12:
            case 1:
            case 2:  return Plantes.Saisons.Hiver;
            case 3:
            case 4:
            case 5:  return Plantes.Saisons.Printemps;
            case 6:
            case 7:
            case 8:  return Plantes.Saisons.Eté;
            default: return Plantes.Saisons.Automne;
        }
    }      

    private void GererMeteo()
    {
        // Temperature aléatoire en fonction de la saison

        if (saisonActuelle == Plantes.Saisons.Hiver )
            tempActuelle = rnd.Next(-5,11);

        else if (saisonActuelle == Plantes.Saisons.Printemps )
            tempActuelle = rnd.Next(5,21);

        else if (saisonActuelle == Plantes.Saisons.Eté )
            tempActuelle = rnd.Next(15,36);

        else
            tempActuelle = rnd.Next(0,16);

        ContexteSimulation.TempEnCours = tempActuelle;

        Console.WriteLine("- Météo de la semaine -");
        Console.WriteLine($"Temperature moyenne : {tempActuelle}°C");

        // Type de Météo aléatoire chaque semaine (pluie, soleil, grele)

        int typeMeteo = rnd.Next(1,4);

        if (typeMeteo == 1) 
        {
            MétéoSimu.Pleuvoir(PotagerSimu);
            Console.WriteLine("Temps : pluvieux 🌧️");
        }


        else if (typeMeteo == 2) 
        {
            MétéoSimu.Ensoleiller(PotagerSimu);
            Console.WriteLine("Temps : ensolleillé 🔆");
        }

        else 
        {
            MétéoSimu.Greler(PotagerSimu); //-> Urgence
            Console.WriteLine("Temps : orageux ⛈️");
        }
    }

    // Vérification de saisi du numéro de terrain
    int VerifierNumTerrain()
    {
        bool idxValide;
        int index;
        Console.Write("Numéro du terrain sur lequel vous voulez agir : ");
        do
        {
            if (int.TryParse(Console.ReadLine()!, out index) && index >= 0 && index < PotagerSimu.ListeTerrains.Count)
            {
                idxValide = true;
            }
            else
            {
                idxValide = false;
                Console.Write("Index invalide, entrez un nouveau numéro : ");
            }

        } while (!idxValide);

        return index;  
    }

    // Gerer les actions du joueur à chaque tour
    private void ChoisirAction() {  // Retourne le choix du joueur
        
        bool choixValide = true;
        int choix = 0;

        Console.WriteLine("- Menu d'actions -");
        Console.WriteLine("1) Arroser un terrain, 2) Planter une plante, 3) Ajouter un terrain, 4) Récolter un fruit, 5) Passer la semaine");   // + eloigner les animaux ?? recouvrir un terrain ??
        Console.WriteLine("Entrez le numéro de votre choix :");

        do
        {
            string reponse = Console.ReadLine()!;

            if (int.TryParse(reponse, out choix) && choix >= 1 && choix <= 5)
            {
                choixValide = true;
            }
            else 
            { 
                Console.WriteLine("Choix invalide, recommencez.");
                choixValide = false;
            }

        } while (!choixValide);

        if (choix == 1) // Arroser
        {
            bool qteValide = true;
            double quantité;

            int index = VerifierNumTerrain();

            Console.Write("Quantité d’eau à verser (entre 1 et 100): "); // Limite d'eau dispo par tour à gerer /!\
            do
            {
                if (double.TryParse(Console.ReadLine()!, out quantité) && quantité >= 1 && quantité <= 100)
                {
                    qteValide = true;
                }
                else
                {
                    qteValide = false;
                    Console.Write("Quantité invalide, entrez en une nouvelle : ");
                }

            } while (!qteValide);

            PotagerSimu.Arroser(index, quantité);   //On arrose le terrain
        }

        if (choix == 2) //Planter
        {
            int ligne;
            int col;
            bool nomPlanteValide;
            bool saisonSemiValide;
            string nomPlante;
            Plantes planteASemer = null;

            //Demander la plante à semer
            Console.Write("Type de plante à semer (Pomme, Fraise) : ");
            do 
            {
                do
                {
                    nomPlante = Console.ReadLine()!.ToLower();
                    if (ListePlantes.Contains(nomPlante)) //On vérifie que le semi existe
                    {
                        nomPlanteValide = true;
                    }
                    else
                    {
                        nomPlanteValide = false;
                        Console.Write("Plante inexistante, entrez en une autre : ");
                    }

                } while (!nomPlanteValide);

                Plantes plante = nomPlante switch
                {
                    "pomme"          => new Pomme(),
                    "fraise"         => new Fraise(),
                };

                // Vérif de la saison de semis de la plante -> si aucune plante à planter dans cette saison (à gérer!)
                if (!plante.SaisonsSemi.Contains(saisonActuelle))
                {
                    Console.WriteLine($"{plante.Nom} ne peut être semé en {saisonActuelle}.");
                    saisonSemiValide = false;
                    Console.Write("Veuillez choisir une autre plante : "); //Retour menu d'actions à gérer !
                    //if (Console.ReadLine()!.ToLower() == "retour")    //retour menu d'action
                        
                }
                else
                {
                    saisonSemiValide = true;
                    planteASemer = plante;
                }

            } while (!saisonSemiValide);

            //Demander le terrain sur lequel semer
            
            Terrains terrain = PotagerSimu.ListeTerrains[VerifierNumTerrain()];   // On recupère le terrain choisi

            //Demander l'emplacement
            do 
            {
                Console.Write("Ligne du terrain : ");
                ligne = int.Parse(Console.ReadLine()!);
                /*if (int.TryParse(Console.ReadLine()!, out ligne)){}*/

                Console.Write("Colonne du terrain : ");
                col = int.Parse(Console.ReadLine()!);

            } while(terrain.Planter(planteASemer, ligne, col) == false); // Tant que l'emplacement est mauvais, redemander les coordonnées
        
        }

        if (choix == 3) //Ajouter un terrain au potager
        {
            bool typeValide = true;
            string nomType;
            int ligne;
            int col;

            // Demander le type de terrain
            Console.Write("Type du terrain à ajouer (sable, argile, terre) : ");
            do
            {
                nomType = Console.ReadLine()!;
                nomType.ToLower();
                if (ListeTerrains.Contains(nomType)) //On vérifie que le semi existe
                {
                    typeValide = true;
                }
                else
                {
                    typeValide = false;
                    Console.Write("Type inexistant, entrez en un autre : ");
                }

            } while (!typeValide);

            //Demander la dimension du terrain
            Console.Write("Nombre de lignes du terrain : ");    // Mettre une limite !
            ligne = int.Parse(Console.ReadLine()!);

            Console.Write("Nombre de colonnes du terrain : ");
            col = int.Parse(Console.ReadLine()!);

            Terrains terrain = nomType switch
            {
                "sable"          => new Sable(ligne,col),
                "terre"         => new Terre(ligne,col),
                "argile"        => new Argile(ligne,col)
            };

            PotagerSimu.ListeTerrains.Add(terrain); // Terrain ajouté
        }


        if (choix == 4) //Recolter des fruits sur un terrain
        {
            Terrains terrain = PotagerSimu.ListeTerrains[VerifierNumTerrain()];   // On recupère le terrain choisi

            PotagerSimu.Recolter(terrain);

        }

        if (choix == 5) //Continuer la simu
        {
            return;
        }
    
    }

    public void ChoisirActionUrgente()
    {
        bool choixValide;
        int choix;

        Console.WriteLine("- Menu d'actions d'urgence -");
        Console.WriteLine("1) Chasser les animaux, 2) Traiter une maladie, 3) Recouvrir le terrain");   // + eloigner les animaux, recouvrir un terrain, traiter la plante
        Console.WriteLine("Entrez le numéro de votre choix :");

        do
        {
            string reponse = Console.ReadLine()!;

            if (int.TryParse(reponse, out choix) && choix >= 1 && choix <= 6)
            {
                choixValide = true;
            }
            else 
            { 
                Console.WriteLine("Choix invalide, recommencez.");
                choixValide = false;
            }

        } while (!choixValide);

        if (choix == 1) // Chasser
        {
            Terrains t = PotagerSimu.ListeTerrains[VerifierNumTerrain()];
            
            string nomAnimal;
            bool validAnimal; 
            Console.Write("Quel animal voulez-vous chasser de votre terrain ? : ");
            AnimauxNuisible animal;
            do
            {
                nomAnimal = Console.ReadLine()!.ToLower();

                animal = nomAnimal switch
                {
                    "criquet"           => new Criquet(),
                    "oiseaux"           => new Oiseaux(),
                    "escargot"          => new Escargot(),
                    //_ => throw new InvalidOperationException("Animal non pris en charge.")
                };

                if (t.ListeAnimauxNuisibles.Contains(animal)) //On vérifie que l'animal existe sur le terrain
                {
                    validAnimal = true;
                }
                else
                {
                    validAnimal = false;
                    Console.Write("Animal inexistant sur ce terrain, entrez en un autre : ");
                }

            } while (!validAnimal);

            PotagerSimu.Chasser(animal,t);
        }

        if (choix == 2) // Traiter
        {
            Terrains t = PotagerSimu.ListeTerrains[VerifierNumTerrain()];

        }
    }


}