using System.Security.Cryptography.X509Certificates;

public class Simulation {

    private Random rnd = new Random(); //Pas besoin pour l'instant
    public Plantes.Saisons saisonActuelle;
    public double tempActuelle;
    public List<string> ListePlantes = new List<string> {"pomme","fraise","kiwi","mangue","poire","pasteque","mauvaise herbe"};   //Liste des types de plantes existantes. A completer en ajoutant les autres plantes!
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
            t.PropagerMauvaiseHerbe(); // Propagation des mauvaises herbes (s'il y en a)

            foreach (Plantes p in t.ListePlantes)
            {
                p.Pousser();
                if ((p.SaisonFruits == saisonActuelle) && (p.estMature) && (p.nbFruitsActuel < p.NbFruitsMax)) // /!\ à la saison de récolte des fruits
                {
                    p.DonnerFruit();
                }
            }

            t.VerifierEtatPlantes();
        }

        PotagerSimu.AfficherEtat();

        // Menu d'action, le joueur peut effectuer des actions jusqu'à atteindre une limite ou lorsqu'il continuer la simulation
        
        ChoisirAction();

    }

    public void ModeUrgence() //Avance de jour en jour
    {
        saisonActuelle = ObtenirSaison(DateCourante);
        ContexteSimulation.SaisonEnCours = saisonActuelle; //MàJ dans ContexteSimulation.cs
        Console.WriteLine("\n MODE URGENCE ACTIF !");
        // Affichage de la date
        Console.WriteLine($"Jour {DateCourante: dd MMM yyyy} - Saison : {saisonActuelle}");
        Console.WriteLine();
        //Afficher l'origine de l'urgence
        
        foreach (Terrains t in PotagerSimu.ListeTerrains)
        {
            if (t.urgenceAnimaux)
                Console.WriteLine($"⚠️ Présence d'un animal nuisible sur le terrain {t.numTerrain}.");

            if (t.urgenceMaladie)
                Console.WriteLine($"⚠️ Présence d'une maladie sur le terrain {t.numTerrain}.");

            if (t.urgenceInondation)
                Console.WriteLine($"⚠️ Présence d'une innondation sur le terrain {t.numTerrain}.");

            if (t.urgenceMauvaiseHerbe)
                Console.WriteLine($"⚠️ Présence de mauvaises herbes sur le terrain {t.numTerrain}.");
        }

        Random rnd = new Random();
        int cont = rnd.Next(0, 101);

        foreach (Terrains t in PotagerSimu.ListeTerrains)
        {
            // Evolution des maladies
            foreach (Maladies m in t.ListeMaladie)
            {
                if (cont <= m.ProbabiliteContamination)
                {
                    m.Propager(t);
                }
                PotagerSimu.Contaminer(m, t);
            }

            // Evolution des animaux
            foreach (AnimauxNuisible nuisible in t.ListeAnimauxNuisibles)
            {
                nuisible.Deplacer(t);
                PotagerSimu.Impacter(nuisible, t);
            }
        }
        ChoisirActionUrgente();
        
    }

    public void LancerSimulation() // Ou à faire direct dans le Program.cs
    {
        Console.WriteLine("La simulation va démarrée !");
        bool continuer = true;
        string reponse;

        ModeNormal();   // 1er tour en mode normal (1 semaine passe)

        do
        {
            PotagerSimu.VerifierUrgence();

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
            tempActuelle = rnd.Next(15,40);

        else
            tempActuelle = rnd.Next(0,16);

        ContexteSimulation.TempEnCours = tempActuelle;

        Console.WriteLine("\n - Météo de la semaine -");
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
    private void ChoisirAction() 
    {  
        bool choixValide = true;
        int choix = 0;
        int comptActions = 0; 
        int nbActionsMax = 3*PotagerSimu.ListeTerrains.Count(); // nb actions max par tours proportinnel au nb de terrains dans le potager
        bool maxAtteint = false; 

        Console.WriteLine("\n - Menu d'actions -");
        Console.WriteLine($"Vous pouvez effectuer {nbActionsMax} actions maximum sur ce tour.");
        Console.WriteLine("1) Arroser un terrain, 2) Arroser une plante, 3) Planter une plante, 4) Ajouter un terrain, 5) Récolter un fruit, 6) Désherber, 7) Passer la semaine"); 
        Console.WriteLine("Entrez le numéro de votre choix :");
        do
        {
            do
            {
                string reponse = Console.ReadLine()!;

                if (int.TryParse(reponse, out choix) && choix >= 1 && choix <= 7)
                {
                    choixValide = true;
                }
                else 
                { 
                    Console.WriteLine("Choix invalide, recommencez.");
                    choixValide = false;
                }

            } while (!choixValide);

            if (choix == 1) // Arroser un terrain
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

                PotagerSimu.ArroserTerrain(index, quantité);   //On arrose le terrain
            }
            if (choix == 2) // Arroser une plante
            {
                
                bool coordXValide = true;
                bool coordYValide = true;
                int coordX;
                int coordY;
                bool qteValide = true;
                double quantité;
                comptActions++;

                int index = VerifierNumTerrain();

                Console.Write("Quantité d’eau à verser (entre 1 et 100): "); 
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

                do
                {
                    Console.Write("Coordonnée X de la plante à arroser : ");
                    do
                    {
                        if (int.TryParse(Console.ReadLine()!, out coordX) && coordX >= 0 && coordX < PotagerSimu.ListeTerrains[index].Lignes)
                        {
                            coordXValide = true;
                        }
                        else
                        {
                            coordXValide = false;
                            Console.Write("Coordonnée invalide, entrez un nouveau numéro : ");
                        }

                    } while (!coordXValide);
                    
                    Console.Write("Coordonnée Y de la plante à arroser : ");
                    do
                    {
                        if (int.TryParse(Console.ReadLine()!, out coordY) && coordY >= 0 && coordY < PotagerSimu.ListeTerrains[index].Colonnes)
                        {
                            coordYValide = true;
                        }
                        else
                        {
                            coordYValide = false;
                            Console.Write("Coordonnée invalide, entrez un nouveau numéro : ");
                        }

                    } while (!coordYValide);
                
                } while(!PotagerSimu.ArroserPlante(index, coordX, coordY, quantité)); //tant que la plante n'a pas été trouvée

            }

            if (choix == 3) //Planter
            {
                int ligne;
                int col;
                bool nomPlanteValide;
                bool saisonSemiValide;
                string nomPlante;
                Plantes planteASemer = null;
                comptActions++;

                //Demander la plante à semer
                Console.Write("Type de plante à semer (Pomme, Fraise, Kiwi, Mangue, Poire, Pasteque) : ");
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
                        "pomme" => new Pomme(),
                        "fraise" => new Fraise(),
                        "kiwi" => new Kiwi(),
                        "poire" => new Poire(),
                        "pasteque" => new Pasteque(),
                        "mauvaise herbe" => new MauvaiseHerbe()
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

            if (choix == 4) //Ajouter un terrain au potager
            {
                bool typeValide;
                string nomType;
                int ligne;
                int col;
                comptActions++;

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

                PotagerSimu.AjouterTerrain(terrain); // Terrain ajouté
            }

            if (choix == 5) //Recolter des fruits sur un terrain
            {
                comptActions++;
                Terrains terrain = PotagerSimu.ListeTerrains[VerifierNumTerrain()];   // On recupère le terrain choisi
                PotagerSimu.Recolter(terrain);  // On récolte
            }

            if (choix == 6)
            {
                comptActions++;
                Terrains terrain = PotagerSimu.ListeTerrains[VerifierNumTerrain()];   // On recupère le terrain choisi
                PotagerSimu.Desherber(terrain); //On désherbe
            }

            if (choix == 7) //Continuer la simu
            {
                return;
            }

            if (comptActions == nbActionsMax) 
                maxAtteint = true;    //vérification nb actions
            else
            {
                Console.WriteLine($"Il vous reste {nbActionsMax-comptActions} actions à utiliser.");
                Console.WriteLine($"Tapez 1, 2, 3, 4, 5 ou 6 pour effectuer une action ; 7 pour continuer la simulation :");
            } 
        
        } while(!maxAtteint);
    }
    
    public void ChoisirActionUrgente()
    {
        bool choixValide;
        int choix;

        Console.WriteLine("\n - Menu d'actions d'urgence -");
        Console.WriteLine("1) Chasser les animaux, 2) Traiter une maladie, 3) Désherber, 4) Recouvrir le terrain, 5) Passer un jour");   // + eloigner les animaux, recouvrir un terrain, traiter la plante
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

        if (choix == 1) // Chasser
        {
            Terrains t = PotagerSimu.ListeTerrains[VerifierNumTerrain()];
            
            string nomAnimal;
            bool validAnimal; 
            AnimauxNuisible animal;

            Console.Write($"Quel animal voulez-vous chasser du terrain {t.numTerrain} ? : ");
            
            do
            {
                nomAnimal = Console.ReadLine()!.ToLower();

                validAnimal = t.ListeAnimauxNuisibles.Any(a =>
                    (nomAnimal == "criquet" && a is Criquet) ||
                    (nomAnimal == "oiseaux" && a is Oiseaux) ||
                    (nomAnimal == "escargot" && a is Escargot)
                );

                if (!validAnimal)
                {
                    Console.Write("Animal inexistant sur ce terrain, entrez en un autre : ");
                }

            } while (!validAnimal);

            // Récupérer l’instance déjà présente
            animal = t.ListeAnimauxNuisibles.First(a =>
                (nomAnimal == "criquet" && a is Criquet) ||
                (nomAnimal == "oiseaux" && a is Oiseaux) ||
                (nomAnimal == "escargot" && a is Escargot));

            PotagerSimu.Chasser(animal, t);
        }

        if (choix == 2) // Traiter
        {
            Terrains t = PotagerSimu.ListeTerrains[VerifierNumTerrain()];
            string nomMaladie;
            bool validMaladie; 
            Maladies maladie;

            Console.Write($"Quelle maladie voulez-vous traiter sur le terrain {t.numTerrain} ? : ");
            
            do
            {
                nomMaladie = Console.ReadLine()!.ToLower();

                validMaladie = t.ListeMaladie.Any(a =>
                    (nomMaladie == "pythium" && a is Pythium) ||
                    (nomMaladie == "anthracnose" && a is Anthracnose)                    
                );

                if (!validMaladie)
                {
                    Console.Write("Maladie inexistante sur ce terrain, entrez en une autre : ");
                }

            } while (!validMaladie);

            // Récupérer l’instance déjà présente
            maladie = t.ListeMaladie.First(a =>
                (nomMaladie == "pythium" && a is Pythium) ||
                (nomMaladie == "anthracnose" && a is Anthracnose));
            
            PotagerSimu.Traiter(maladie,t);
        }
        
        if (choix == 3) // Désherber
        {
            Terrains terrain = PotagerSimu.ListeTerrains[VerifierNumTerrain()];   // On recupère le terrain choisi
            PotagerSimu.Desherber(terrain); //On désherbe
        }

        if (choix == 4)// Couvrir terrain
        {
            Terrains t = PotagerSimu.ListeTerrains[VerifierNumTerrain()];
            PotagerSimu.Couvrir(t);
        }

        if (choix == 5) // Passer un jour
        {
            return;
        }
    } 
}