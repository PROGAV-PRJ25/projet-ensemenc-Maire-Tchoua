public class Simulation {

    private bool urgenceActive = false;
    private Random rnd = new Random();
    public Plantes.Saisons saisonActuelle;

    public Potager Potager { get; }

    // public MeteoMois Météo { get; } -> classe Météo à faire

    public DateTime DateCourante { get; private set; } // Date courante dans la simulation (tour = 1 mois)

    public Simulation(Potager potager) //Météo météo
    {
        Potager = potager;
        //Météo = météo;
        DateCourante = new DateTime(2025, 1, 1); // date de départ, par ex. 1er janvier 2025

    }

    private void ModeNormal() // Avance de semaine en semaine
    {
        Console.Clear();
        Potager.AfficherEtat();

        ContexteSimulation.SaisonEnCours = ObtenirSaison(DateCourante); //MàJ dans ContexteSimulation.cs
        
        saisonActuelle = ObtenirSaison(DateCourante);
        Console.WriteLine($"Tour du {DateCourante:MMM yyyy} - Saison : {saisonActuelle}");

        // Le jeu évolue d'une semaine

        

        // En fin de tour, avancer la date de 7 jours (fin de la semaine) :
        DateCourante = DateCourante.AddDays(7);

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

    


}