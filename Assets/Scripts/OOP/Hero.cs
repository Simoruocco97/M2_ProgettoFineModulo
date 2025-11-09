using UnityEngine;

[System.Serializable]

public class Hero
{
    [SerializeField] private string name;
    [SerializeField] private int hp;
    [SerializeField] private Stats baseStats;
    [SerializeField] private ELEMENT resistance;
    [SerializeField] private ELEMENT weakness;
    [SerializeField] private Weapon weapon;

    public Hero(string name, int hp, Stats baseStats, ELEMENT resistance, ELEMENT weakness, Weapon weapon)
    {
        this.name = name;
        this.hp = hp;
        this.baseStats = baseStats;
        this.resistance = resistance;
        this.weakness = weakness;
        this.weapon = weapon;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("Il nome del giocatore non puo' essere vuoto! Impostato 'Marcellino' di default");
            this.name = "Marcellino";
        }
        this.name = name;
    }

    public int GetHp()
    {
        return this.hp;
    }

    public void SetHp(int hp)
    {
        if (hp < 0) 
        {
            Debug.LogWarning("Il valore degli HP non puo' scendere sotto lo 0! Reimpostati a 0.");
            this.hp = 0;
        }
        else if (hp > 100)
        {
            Debug.LogWarning("Il valore degli HP non puo' superare i 100! Reimpostati a 100.");
            this.hp = 100;
        }
            this.hp = hp;
    }

    public Stats BaseStats { get { return this.baseStats; } set { this.baseStats = value; } }

    public ELEMENT Resistance { get { return this.resistance; } set { this.resistance = value; } }

    public ELEMENT Weakness { get { return this.weakness; } set { this.weakness = value; } }

    public Weapon Weapon { get { return this.weapon; } set { this.weapon = value; } }

    public void AddHp(int amount)
    {
        SetHp(this.hp + amount);
    }

    public void TakeDamage(int damage)
    {
        AddHp(-damage);
    }

    public bool IsAlive()
    {
        if (this.hp > 0)
        {
            return true;
        }
        return false;
    }
}
