public class Simulation {

    private bool urgenceActive = false;
    private Random rnd = new Random(); //Pas besoin pour l'instant
    public Plantes.Saisons saisonActuelle;
    public double tempActuelle;

    public Potager PotagerSimu { get; }
    public Meteo MétéoSimu { get; set; } 
    public DateTime DateCourante { get; private set; } // Date courante dans la simulation (tour = 1 mois)

    public Simulation(Potager potagerSimu , Meteo météoSimu)
    {
        PotagerSimu = potagerSimu;
        MétéoSimu = météoSimu;
        DateCourante = new DateTime(2025, 1, 1); // date de départ, par ex. 1er janvier 2025
    }

    private void ModeNormal() // Avance de semaine en semaine
    {
        Console.Clear();
        

        saisonActuelle = ObtenirSaison(DateCourante);
        ContexteSimulation.SaisonEnCours = saisonActuelle; //MàJ dans ContexteSimulation.cs
        Console.WriteLine($"Tour du {DateCourante:MMM yyyy} - Saison : {saisonActuelle}");

        GererMeteo();
        foreach (Terrains t in PotagerSimu.ListeTerrains)
        {
            foreach(Plantes p in t.ListePlantes)
            {
                p.Pousser();
                
            }
        }

        // Le jeu évolue d'une semaine

        

        PotagerSimu.AfficherEtat();

        // En fin de tour, avancer la date de 7 jours (fin de la semaine) :
        DateCourante = DateCourante.AddDays(7);

    }

    private void LancerSimulation() // Ou à faire direct dans le Program.cs
    {
        Console.WriteLine("La simulation va démarrée !");
        bool continuer = true;
        string reponse;

        while (continuer)
        {
            if (!urgenceActive)
            {
                ModeNormal();
                Console.WriteLine("Continuer la simulation ?");
                reponse = Console.ReadLine();
                if (reponse == "non" || reponse == "Non")
                { 
                    continuer = false;
                }

            }
            // else ModeUrgence();
        }
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

    public void GererMeteo()
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

        // Temps aléatoire

        /*
        int typeMeteo = rnd.Next(1,4);
        if (typeMeteo == 1) 
            Meteo.Pleuvoir(PotagerSimu);
        else if (typeMeteo == 2) 
            Meteo.Ensoleiller(PotagerSimu);
        else 
            Meteo.Greler(PotagerSimu); //-> Urgence*/

    }


}