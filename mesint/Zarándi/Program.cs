Console.WriteLine("Adja meg a gépek számát");
int gep = int.Parse(Console.ReadLine());
Console.WriteLine("Adja meg a munkák számát");
int munk = int.Parse(Console.ReadLine());
int[] gepek = new int[] { 5, 10, 20 };
int[] munkak = new int[] { 10, 20, 50, 100, 200, 500 };
for (int i = 0; i < gepek.Length; i++)
{
    for (int j = 0; j < munkak.Length; j++)
    {
        futat(gepek[i], munkak[j]);
    }
}

static void futat(int gepek, int job)
{
    Random rnd = new Random();
    int[,] p = new int[job, gepek];
    for (int i = 0; i < p.GetLength(0); i++)
    {
        for (int j = 0; j < p.GetLength(1); j++)
        {
            p[i, j] = rnd.Next(1, 10);
        }
    }
    int[] hatarid = new int[p.GetLength(0)];
    int[] sorrrend = new int[p.GetLength(0)];
    for (int i = 0; i < sorrrend.Length; i++)
    {
        hatarid[i] = rnd.Next(10, 30);
        sorrrend[i] = i;
    }

    sorrrend = sorrrend.OrderBy(x => rnd.Next()).ToArray();
    int a = p.GetLength(0) * p.GetLength(1) *1000 ;
    szimhut(sorrrend, p, hatarid, a);

}

static List<int[]> szomszedok(int[] mostanisor)
{

    List<int[]> szomszed = new List<int[]>();
    for (int i = 0; i < mostanisor.Length; i++)
    {
        int seged;
        for (int j = i + 1; j < mostanisor.Length; j++)
        {
            seged = mostanisor[i];
            mostanisor[i] = mostanisor[j];
            mostanisor[j] = seged;
            szomszed.Add(mostanisor.ToArray());
            seged = mostanisor[i];
            mostanisor[i] = mostanisor[j];
            mostanisor[j] = seged;
        }
    }
    return szomszed;

}
static void szimhut(int[] sorrrend, int[,] p, int[] hatarid, int iter)
{

    int ered1 = 0;
    Random rnd = new Random();
    List<int[]> list = new List<int[]>();
    int[] mostanisor = sorrrend;
    int vegered = 0;
    for (int i = iter; i > 1; i--)
    {
        const double k = 1.3807e-16; //BOLTZMAN
        list = szomszedok(mostanisor);
        int rand = rnd.Next(0, list.Count);
        ered1 = eredmeny(p, list[rand]);
        int ered2 = eredmeny(p, mostanisor);
        int osz = ered1 - ered2;
        if (osz < 0)
        {
            mostanisor = list[rand];
            vegered = ered1;

        }
        else
        {
            double z;
            double prob;
            z = -1 * (osz / (k * i));
            prob = Math.Exp(z);
            prob = prob * 100;
            int a = Convert.ToInt32(Math.Truncate(prob));
            if (rnd.Next(0, 100) < a)
            {
                mostanisor = list[rand];
                vegered = ered1;
            }

        }
    }
    Console.WriteLine("A jód megoldási sorozat: ");
    foreach (var i in mostanisor)
    {
        Console.Write($"{i},");
    }
    Console.WriteLine($"A legjobb megoldás: {ered1}");


}

static int[,] sorcsere(int[,] eredet, int[] sor)
{

    int[,] valasz = new int[eredet.GetLength(0), eredet.GetLength(1)];
    for (int i = 0; i < eredet.GetLength(0); i++)
    {
        for (int j = 0; j < eredet.GetLength(1); j++)
        {
            valasz[i, j] = eredet[sor[i], j];
        }

    }
    return valasz;
}
static int eredmeny(int[,] gepfel, int[] soroz)
{

    gepfel = sorcsere(gepfel, soroz);
    int[,] bef = new int[gepfel.GetLength(0), gepfel.GetLength(1)];
    int[,] varak = new int[gepfel.GetLength(0), gepfel.GetLength(1)];

    for (int oszlop = 0; oszlop < gepfel.GetLength(1); oszlop++)
    {
        for (int sor = 0; sor < gepfel.GetLength(0); sor++)
        {
            if (oszlop == 0)            //az első oszlop generálás
            {
                int osz = 0;
                for (int k = sor; k >= 0; k--)
                {
                    osz += gepfel[k, oszlop];
                }
                bef[sor, oszlop] = osz;
            }
            else
            {
                int osz = bef[0, oszlop - 1]; //mindig azzal kezdünk amivel az elzö 1. feladat befejezési ideje 
                int osz2 = 0;
                int varakozas = 0;
                int beflast = 0;
                for (int k = 0; k <= sor; k++)
                {
                    osz += gepfel[k, oszlop];
                    if (k != gepfel.GetLength(0) - 1 && sor != 0)
                    {
                        beflast = bef[k + 1, oszlop - 1];
                        osz2 = osz;
                        varakozas = bef[k + 1, oszlop - 1] - osz;
                        if (bef[k + 1, oszlop - 1] - varak[k + 1, oszlop - 1] > osz)
                        {
                            osz = bef[k + 1, oszlop - 1];
                        }
                    }
                }
                if (osz == beflast) varak[sor, oszlop] = beflast - osz2;
                bef[sor, oszlop] = osz;
            }

        }
    }


   
    return bef[bef.GetLength(0) - 1, bef.GetLength(1) - 1];
}