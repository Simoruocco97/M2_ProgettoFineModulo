[System.Serializable]

public struct Stats
{
    public int atk, def, res, spd, crt, aim, eva;

    public Stats(int atk, int def, int res, int spd, int crt, int aim, int eva)
    {
        this.atk = atk;
        this.def = def;
        this.res = res;
        this.spd = spd;
        this.crt = crt;
        this.aim = aim;
        this.eva = eva;
    }

    public static Stats Sum(Stats stat1, Stats stat2)
    {
        Stats finalStats;

        finalStats.atk = stat1.atk + stat2.atk; 
        finalStats.def = stat1.def + stat2.def;
        finalStats.res = stat1.res + stat2.res;
        finalStats.spd = stat1.spd + stat2.spd;
        finalStats.crt = stat1.crt + stat2.crt;
        finalStats.aim = stat1.aim + stat2.aim;
        finalStats.eva = stat1.eva + stat2.eva;

        return finalStats;
    }

}