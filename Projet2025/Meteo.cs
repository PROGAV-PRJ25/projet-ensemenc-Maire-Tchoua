public class Meteo
{ 
    public Meteo()
    {}

    public void Pleuvoir(Terrains terrain, Escargot escargot, VerDeTerre verDeTerre)
    {
        Random rnd = new Random();
        int quantEau  = rnd.Next(10,100); // Quantité d'eau tombée sur le terrain
        terrain.NivEau +=  quantEau - quantEau*terrain.Absorption; // Le niveau d'eau danns le terrain augmente selon la quantité d'eau qu'il a absorbé
        
        if (terrain is Terre)
            terrain.Apparait(verDeTerre);
        if ((terrain is Terre || terrain is Sable) && terrain.NivEau > 90) // Le ver de terre apparait sur du sable ou de la terre très humide
            terrain.Apparait(escargot);
    }

    public void Ensoleiller(Terrains terrain, Abeille abeille, Criquet criquet)
    {
        Random rnd = new Random();
        int indiceUV  = rnd.Next(5,50);
        terrain.NivEau -= indiceUV; // le niveau d'eau du terrain diminue grâce à l'intensité des UV
        
        terrain.Apparait(abeille); // une abeille apparait peu importe le terrain 
        if (terrain is Sable && terrain.NivEau < 15) // si le terrain est du sable et qu'il est très sec
            terrain.Apparait(criquet); // Alors un criquet apparait 
    }

    public void Greler(Terrains terrain)
    {
        foreach (Plantes p in terrain.ListePlantes)
        {
            p.nbFruitsActuel = 0; // Détruit tous les fruits de toutes les plantes du terrain
        }
    }
}