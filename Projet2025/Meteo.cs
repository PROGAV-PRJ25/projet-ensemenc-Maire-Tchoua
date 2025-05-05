public class Meteo
{ 
    public int QuantEau {get;set;}
    public int IndiceUV {get; set;}
    public Meteo(int quantEau, int indiceUV)
    {
        QuantEau = quantEau;
        IndiceUV = indiceUV;
    }

    public void Pleuvoir(Terrains terrain) // METTRE POTAGER
    {
        Random rnd = new Random();
        QuantEau = rnd.Next(10,100); // Quantité d'eau tombée sur le terrain
        terrain.NivEau +=  QuantEau - QuantEau*terrain.Absorption; // Le niveau d'eau danns le terrain augmente selon la quantité d'eau qu'il a absorbé
        
        // Gérer la température 
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

    public void Ensoleiller(Terrains terrain) // potager
    {
        
        Random rnd = new Random();
        IndiceUV  = rnd.Next(5,50);
        terrain.NivEau -= IndiceUV; // le niveau d'eau du terrain diminue grâce à l'intensité des UV
        
        // gérer la température 

        //parcourir la liste des terrains du potager => foreach 
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

    public void Greler(Terrains terrain)
    {
        // gérer la temperature 
        foreach (Plantes p in terrain.ListePlantes)
        {
            p.nbFruitsActuel = 0; // Détruit tous les fruits de toutes les plantes du terrain
        }
    }
}