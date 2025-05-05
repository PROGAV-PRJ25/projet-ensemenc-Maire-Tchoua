public class Meteo
{ 
    public int QuantEau {get;set;}
    public int IndiceUV {get; set;}
    public Meteo(int quantEau, int indiceUV)
    {
        QuantEau = quantEau;
        IndiceUV = indiceUV;
    }

    public void Pleuvoir(Potager potager) 
    {
        Random rnd = new Random();
        QuantEau = rnd.Next(10,101); // Quantité d'eau tombée sur le terrain

        foreach (Terrains terrain in potager.ListeTerrains)
        {
            terrain.NivEau +=  QuantEau - QuantEau*terrain.Absorption; // Le niveau d'eau danns le terrain augmente selon la quantité d'eau qu'il a absorbé
            foreach (Plantes p in terrain.ListePlantes)
            {
                p.eauRecu = terrain.NivEau;
            }
            
            ContexteSimulation.TempEnCours -= 3; // On pert 3 degrés  

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
        }
    }

    public void Ensoleiller(Potager potager) // potager
    {
        Random rnd = new Random();
        IndiceUV  = rnd.Next(5,51);
        foreach (Terrains terrain in potager.ListeTerrains) //parcourir la liste des terrains du potager 
        {
            terrain.NivEau -= IndiceUV; // le niveau d'eau du terrain diminue grâce à l'intensité des UV
            foreach (Plantes p in terrain.ListePlantes)
            {
                p.eauRecu = terrain.NivEau; // Récupérer le niveau d'eau de chaque plante 
            }

            ContexteSimulation.TempEnCours += 3; // On gagne 3 degrés 

            if (IndiceUV > 30)
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
            ContexteSimulation.TempEnCours -= 2; // On pert 2 degrés sur tous les terrains 

            foreach (Plantes p in terrain.ListePlantes)
            {
                p.nbFruitsActuel = 0; // Détruit tous les fruits de toutes les plantes du terrain
            } 
        }
        
    }
}