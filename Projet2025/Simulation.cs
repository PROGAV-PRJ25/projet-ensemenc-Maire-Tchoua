public class Simulation {

    private bool urgenceActive = false;
    private Random rnd = new Random(); //Pas besoin pour l'instant
    public Plantes.Saisons saisonActuelle;

    public Potager Potager { get; }

    // public Météo MétéoSemaine { get; } -> classe Météo à faire

    public DateTime DateCourante { get; private set; } // Date courante dans la simulation (tour = 1 mois)

    public Simulation(Potager potager /*, Météo météoSemaine*/)
    {
        Potager = potager;
        //MétéoSemaine = météoSemaine;
        DateCourante = new DateTime(2025, 1, 1); // date de départ, par ex. 1er janvier 2025
    }

    private void ModeNormal() // Avance de semaine en semaine
    {
        // typeMeteo = rnd.Next(1,4);
        //if (typeMeteo == 1) Meteo.Pleuvoir(Potager)
        // else if (typeMeteo == 2) Meteo.Soleil(Potager)
        // else Meteo.Grele(Potager) -> Urgence

        Console.Clear();
        Potager.AfficherEtat();
        
        saisonActuelle = ObtenirSaison(DateCourante);
        ContexteSimulation.SaisonEnCours = saisonActuelle; //MàJ dans ContexteSimulation.cs

        Console.WriteLine($"Tour du {DateCourante:MMM yyyy} - Saison : {saisonActuelle}");

        // Le jeu évolue d'une semaine

        

        // En fin de tour, avancer la date de 7 jours (fin de la semaine) :
        DateCourante = DateCourante.AddDays(7);

    }

    /*private void LancerSimulation() // Ou à faire direct dans le Program.cs
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
                if (reponse == "non")
                { 
                    continuer = false;
                }
                

            }
        }
    }*/

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

    


}