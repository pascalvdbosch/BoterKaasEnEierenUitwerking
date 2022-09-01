
class Spel
{
    public static void Main()
    {
        Spelers spelers = new Spelers(2);
        Veld veld = new Veld(3);
        while (veld.Winnaar() == null)
        {
            Console.WriteLine("Het veld is nu: ");
            var opties = veld.Teken();
            int keuze = -1;
            while (keuze < 0 || keuze >= opties.Count)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Kies een getal uit tussen 0 en " + opties.Count + " en druk op enter. ");
                keuze = int.Parse(Console.ReadLine() ?? "-1");
            }
            veld.DoeZet(new Zet(spelers.HuidigeSpeler, opties[keuze]));
            spelers.VolgendeSpeler();
        }
        Console.WriteLine("Het veld is nu: ");
        veld.Teken();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(veld.Winnaar()!.Naam + " heeft gewonnen!");
    }
}
class Spelers
{
    private char[] spelerLetters = new char[] { 'X', 'O', 'A', 'B' };
    private List<Speler> lijst = new List<Speler>();
    private int HuidigeSpelerIndex = 0;
    public Spelers(int aantalSpelers)
    {
        lijst = new List<Speler>();
        for (int i = 0; i < aantalSpelers; i++)
        {
            lijst.Add(new Speler(spelerLetters[i]));
        }
    }
    public void VolgendeSpeler()
    {
        HuidigeSpelerIndex = (HuidigeSpelerIndex + 1) % lijst.Count;
    }
    public Speler HuidigeSpeler
    {
        get
        {
            return lijst[HuidigeSpelerIndex];
        }
    }
}
class Speler
{
    public char Letter { get; private set; }
    public string Naam { get; private set; }
    public Speler(char letter)
    {
        Letter = letter;
        Console.WriteLine("Geef de naam van de speler die met " + Letter + " speelt: ");
        Naam = Console.ReadLine() ?? "Geen naam";
    }
}
class Veld
{
    private readonly int grootte;
    private Speler?[,] data;
    public Veld(int grootte)
    {
        this.grootte = grootte;
        data = new Speler?[grootte, grootte];
    }
    public Speler? Waarde(Coordinaat c)
    {
        return data[c.X, c.Y];
    }
    public void DoeZet(Zet zet)
    {
        data[zet.Coordinaat.X, zet.Coordinaat.Y] = zet.Speler;
    }
    public List<Coordinaat> Teken()
    {
        List<Coordinaat> opties = new List<Coordinaat>();
        int optieNr = 0;
        for (int y = 0; y < grootte; y++)
        {
            Console.WriteLine();
            for (int x = 0; x < grootte; x++)
            {
                var waarde = data[x, y];
                if (waarde == null)
                {
                    Console.Write(optieNr);
                    opties.Add(new Coordinaat(x, y));
                    optieNr++;
                }
                else Console.Write(waarde.Letter);
                Console.Write(" ");
            }
        }
        return opties;
    }
    public Speler? Winnaar()
    {
        var streaks = new List<(Coordinaat, Coordinaat)>();
        for (int i = 0; i < grootte; i++)
        {
            streaks.Add((new Coordinaat(0, i), new Coordinaat(1, 0)));
            streaks.Add((new Coordinaat(i, 0), new Coordinaat(0, 1)));
        }
        streaks.Add((new Coordinaat(0, 0), new Coordinaat(1, 1)));
        streaks.Add((new Coordinaat(0, grootte - 1), new Coordinaat(1, -1)));
        foreach (var (start, offset) in streaks)
        {
            bool gelukt = true;
            var speler = Waarde(start);
            if (speler == null)
                continue;
            for (int i = 0; i < grootte; i++)
                if (speler != Waarde(start + offset * i))
                    gelukt = false;
            if (gelukt)
                return speler;
        }
        return null;
    }
}
class Coordinaat
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Coordinaat(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static Coordinaat operator +(Coordinaat c1, Coordinaat c2)
    {
        return new Coordinaat(c1.X + c2.X, c1.Y + c2.Y);
    }
    public static Coordinaat operator *(Coordinaat c, int i)
    {
        return new Coordinaat(c.X * i, c.Y * i);
    }
}
class Zet
{
    public Speler Speler { get; private set; }
    public Coordinaat Coordinaat { get; private set; }
    public Zet(Speler speler, Coordinaat coordinaat)
    {
        Speler = speler;
        Coordinaat = coordinaat;
    }
}