class Program
{
    static void Main(string[] args)
    {
        // 1) Création d’un terrain 5×5
        Terrains terrain = new Terre(5, 5);

        // 2) Plantation de quelques fruits
        terrain.Planter(new Pomme(), 2, 2);
        terrain.Planter(new Fraise(), 1, 3);

        // 3) Plantation d’une mauvaise herbe en (2,3)
        MauvaiseHerbe mh = new MauvaiseHerbe(); 
        terrain.Planter(mh, 2, 3);

        // 4) Affichage avant propagation
        Console.WriteLine("=== Avant propagation ===");
        terrain.AfficherConsole();

        // 5) Propagation de toutes les mauvaises herbes
        mh.Propager(terrain);

        // 6) Affichage après propagation
        Console.WriteLine("\n=== Après propagation ===");
        terrain.AfficherConsole();


        Console.ReadLine();
    }
}
