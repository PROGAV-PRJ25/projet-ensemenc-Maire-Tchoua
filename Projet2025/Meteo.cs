public class Meteo
{ 
    public int QuantEau {get;set;}
    public int QuantLum {get; set;}
    public Meteo(int quantEau, int quantLum)
    {
        QuantEau = quantEau;
        QuantLum = quantLum;
    }

    public void Pleuvoir(Potager potager) 
    {
        Random rnd = new Random();
        QuantEau = rnd.Next(50,200); // Quantité d'eau tombée sur le potager
        QuantLum = rnd.Next(0,15); // Quantité de lum reçue sur le potager

        foreach (Terrains terrain in potager.ListeTerrains)
        {
            terrain.NivEau +=  QuantEau - QuantEau*terrain.Absorption; // Le niveau d'eau danns le terrain augmente selon la quantité d'eau qu'il a absorbé
            foreach (Plantes p in terrain.ListePlantes)
            {
                p.eauRecu = terrain.NivEau; // L'eau recu par la plante correspond au niveau d'eau restant dans le terrain
                p.lumRecu = QuantLum;
            }
            
            ContexteSimulation.TempEnCours -= 3; // On pert 3 degrés quand il pleut

            if (terrain is Terre && terrain.NivEau > 60)
            {
                VerDeTerre verDeTerre = new VerDeTerre();
                terrain.Apparait(verDeTerre);
            }
            if ((terrain is Terre || terrain is Sable) && terrain.NivEau > 90) // Le ver de terre apparait sur du sable ou de la terre très humide
            {
                Escargot escargot = new Escargot();
                terrain.Apparait(escargot);
            }

            terrain.NivEau = terrain.NivEau*0.5;    // On baisse le niveau d'eau après que les plantes ait bu

        }
    }

    public void Ensoleiller(Potager potager) // potager
    {
        Random rnd = new Random();
        QuantLum  = rnd.Next(15,75);
        foreach (Terrains terrain in potager.ListeTerrains) //parcourir la liste des terrains du potager 
        {
            terrain.NivEau -= 0.5*QuantLum; // le niveau d'eau du terrain diminue avec l'intensité de la lumière (évaporation)
            foreach (Plantes p in terrain.ListePlantes)
            {
                p.eauRecu = terrain.NivEau; // Récupérer le niveau d'eau de chaque plante 
                p.lumRecu = QuantLum;
            }

            ContexteSimulation.TempEnCours += 3; // On gagne 3 degrés avec le soleil

            if (QuantLum > 30)
            {
                Abeille abeille = new Abeille();
                terrain.Apparait(abeille); // une abeille apparait peu importe le terrain 
            }
                
            if (terrain is Sable && terrain.NivEau < 15) // si le terrain est du sable et qu'il est très sec
            {
                Criquet criquet = new Criquet();
                terrain.Apparait(criquet); // Alors un criquet apparait
            }     
        }
        
    }

    public void Greler(Potager potager)
    {
        foreach (Terrains terrain in potager.ListeTerrains)
        {
            ContexteSimulation.TempEnCours -= 2; // On pert 2 degrés

            foreach (Plantes p in terrain.ListePlantes)
            {
                if (p.nbFruitsActuel >= 2)
                    p.nbFruitsActuel -= 2; // Détruit 2 fruits
                else
                    p.nbFruitsActuel = 0;
            } 
        }
        
    }
}