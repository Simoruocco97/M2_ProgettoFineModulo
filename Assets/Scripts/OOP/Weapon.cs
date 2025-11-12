using UnityEngine;

[System.Serializable]

public class Weapon
{
    public enum DAMAGE_TYPE
    {
        PHYSICAL,
        MAGICAL,
    }

    [SerializeField] private string name;
    [SerializeField] private DAMAGE_TYPE dmgType;
    [SerializeField] private ELEMENT elem;
    [SerializeField] private Stats bonusStats;

    public Weapon(string name, DAMAGE_TYPE dmgType, ELEMENT elem, Stats bonusStats)
    {
        this.name = name;
        this.dmgType = dmgType;
        this.elem = elem;
        this.bonusStats = bonusStats;
    }

    public string GetName() => this.name;
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("Il nome dell'arma non puo' essere vuoto. Impostata 'spada lunga' di default.");
            this.name = "Spada Lunga";
        }
        else
        {
            this.name = name;
        }
    }

    public DAMAGE_TYPE DmgType { get { return dmgType; } set { dmgType = value; } }

    public ELEMENT Elem { get { return elem; } set { elem = value; } }

    public Stats BonusStats { get { return bonusStats; } set { bonusStats = value; } }
}