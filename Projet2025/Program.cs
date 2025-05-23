Potager monPotager = new Potager();

Console.WriteLine();

monPotager.AjouterTerrain(new Terre(3,3));

Meteo maMétéo = new Meteo(0,0);

Simulation maSimu = new Simulation(monPotager, maMétéo, new DateTime(2025, 3, 1)); //début du printemps

maSimu.LancerSimulation();



