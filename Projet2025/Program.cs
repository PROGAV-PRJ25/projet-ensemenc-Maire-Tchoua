Potager monPotager = new Potager();
monPotager.AjouterTerrain(new Terre(5,5));

Meteo maMétéo = new Meteo(0,0);

Simulation maSimu = new Simulation(monPotager, maMétéo, new DateTime(2025, 5, 1));

maSimu.LancerSimulation();




/* Test Mauvaises Herbes

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

// 5) Propagation de toutes les mauvaises herbes
mh.Propager(terrain);

// 6) Affichage après propagation
Console.WriteLine("\n=== Après 2e propagation ===");
terrain.AfficherConsole();


Console.ReadLine();*/



