public class Simulation {

    private bool urgenceActive = false;
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
        Console.Clear();
        
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

    public void LancerSimulation() // Ou à faire direct dans le Program.cs
    {
        Console.WriteLine("La simulation va démarrée !");
        bool continuer = true;
        string reponse;

        ModeNormal();   // 1er tour en mode normal (1 semaine passe)

        do
        {
            if (!urgenceActive)
            {
                Console.WriteLine("Continuer la simulation ?");
                reponse = Console.ReadLine();
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
            // else ModeUrgence();

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

    // Gerer les actions du joueur à chaque tour
    private void ChoisirAction() {  // Retourne le choix du joueur
        
        bool choixValide = true;
        int choix = 0;
        int comptActions = 0; 
        int nbActionsMax = 3*PotagerSimu.ListeTerrains.Count(); // nb actions max par tours proportinnel au nb de terrains dans le potager
        bool maxAtteint = false; 

        Console.WriteLine("- Menu d'actions -");
        Console.WriteLine($"Vous pouvez effectuer {nbActionsMax} actions maximum sur ce tour.");
        Console.WriteLine("1) Arroser un terrain, 2) Arroser une plante, 3) Planter une plante, 4) Ajouter un terrain, 5) Récolter un fruit, 6) Passer la semaine");   // + eloigner les animaux ?? recouvrir un terrain ??
        Console.WriteLine("Entrez le numéro de votre choix :");

        do
        {

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

            if (choix == 1) // Arroser un terrain
            {
                bool idxValide = true;
                int index;
                bool qteValide = true;
                double quantité;

                Console.Write("Numéro du terrain à arroser : ");
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

                PotagerSimu.ArroserTerrain(index, quantité);   //On arrose le terrain
            }

            if (choix == 2) // Arroser une plante
            {
                bool idxValide = true;
                bool coordXValide = true;
                bool coordYValide = true;
                int index;
                int coordX;
                int coordY;
                bool qteValide = true;
                double quantité;
                comptActions++;

                Console.Write("Numéro du terrain contenant la plante : ");
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
                bool idxValide = true;
                int index;
                int ligne;
                int col;
                bool nomPlanteValide = true;
                bool saisonSemiValide = true;
                string nomPlante;
                Plantes planteASemer = null;
                comptActions++;

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
                Console.Write("Numéro du terrain sur lequel planter : ");
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

                Terrains terrain = PotagerSimu.ListeTerrains[index];   // On recupère le terrain choisi

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
                bool typeValide = true;
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

                PotagerSimu.ListeTerrains.Add(terrain); // Terrain ajouté
            }

            if (choix == 5) //Recolter un fruit
            {
                bool idxValide = true;
                int index;
                comptActions++;
                
                Console.Write("Numéro du terrain sur lequel vous voulez recolter des fruits : ");
                do
                {
                    if (int.TryParse(Console.ReadLine()!, out index) && index >= 0 && index <= PotagerSimu.ListeTerrains.Count)
                    {
                        idxValide = true;
                    }
                    else
                    {
                        idxValide = false;
                        Console.Write("Index invalide, entrez un nouveau numéro : ");
                    }

                } while (!idxValide);

                Terrains terrain = PotagerSimu.ListeTerrains[index];   // On recupère le terrain choisi
                PotagerSimu.Recolter(terrain); //On récolte

            }

            if (choix == 6) //Continuer la simu
            {
                maxAtteint = true;
                return;
            }

            if (comptActions >= nbActionsMax) 
                maxAtteint = true;    //vérification nb actions
            else
            {
                Console.WriteLine($"Il vous reste {nbActionsMax-comptActions} actions à utiliser.");
                Console.WriteLine($"Tapez 1, 2, 3 ou 4 pour effectuer une action ; ou 5 pour continuer la simulation :");
            } 

        } while (!maxAtteint);
    
    }
    


}