using UnityEngine;

public static class GameFormulas
{

    public static bool HasElementAdvantage(ELEMENT attackElement, Hero defender)
    {
        if (attackElement == defender.Weakness)
        {
            Debug.Log($"RESIST! {defender.GetName()} e' resistente a {attackElement}.");
            return true;
        }
        return false;
    }

    public static bool HasElementDisvantage(ELEMENT attackElement, Hero defender)
    {
        if (attackElement == defender.Resistance)
        {
            Debug.Log($"WEAKNESS! {defender.GetName()} e' debole a {attackElement}.");
            return true;
        }
        return false;
    }

    public static float EvaluateElementalModifier(ELEMENT attackElement, Hero defender)
    {
        if (HasElementDisvantage(attackElement, defender))
        {
            return 0.5f;
        }
        else if (HasElementAdvantage(attackElement, defender))
        {
            return 1.5f;
        }
        else
        {
            return 1f;
        }
    }

    public static bool HasHit(Stats attacker, Stats defender)
    {
        int hitChance = attacker.aim - defender.eva;
        int random = Random.Range(0, 99);
        if (random > hitChance)
        {
            Debug.Log("MISS");
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool IsCrit(int critValue)
    {
        int random = Random.Range(0, 99);
        if (random < critValue)
        {
            Debug.Log("CRIT");
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int CalculateDamage(Hero attacker, Hero defender)
    {
        Stats attackerTot = Stats.Sum(attacker.BaseStats, attacker.Weapon.BonusStats);
        Stats defenderTot = Stats.Sum(defender.BaseStats, defender.Weapon.BonusStats);

        int damage;
        float modifier;

        if (attacker.Weapon.DmgType == Weapon.DAMAGE_TYPE.PHYSICAL)
        {
            damage = attackerTot.atk - defenderTot.def;
        }
        else // quindi se (attacker.Weapon.DmgType == Weapon.DAMAGE_TYPE.MAGICAL)
        {
            damage = attackerTot.atk - defenderTot.res;
        }

        if (damage < 0)  //ho spostato il punto finale (di restituire 0 se il numero dovesse essere negativo)
        {                //uscendo prima dal codice
            Debug.Log($"{attacker.GetName()} colpisce {defender.GetName()} infliggendo {damage} danni!");
            return 0;
        }

        modifier = EvaluateElementalModifier(attacker.Weapon.Elem, defender);
        damage = (int)(damage * modifier);

        if (IsCrit(attackerTot.crt))
        {
            damage *= 2;
        }
        Debug.Log($"{attacker.GetName()} colpisce {defender.GetName()} infliggendo {damage} danni!");
        return damage;
    }

    //opzionali

    public static void CheckForDuel(Hero hero, Hero hero2)
    {
        if (!hero.IsAlive() || !hero2.IsAlive())
        {
            Debug.Log("Uno dei due eroi e' gia' morto!");
            inizia = false;
            return;
        }
    }

    public static void DuelStart(Hero hero, Hero hero2)
    {
        if (hero.BaseStats.spd > hero2.BaseStats.spd)
        {
            Debug.Log($"{hero.GetName()} attacca per primo, {hero2.GetName()} e' pronto a difendersi!");
            hero2.TakeDamage(CalculateDamage(hero, hero2));

            if (hero2.IsAlive())
            {
                Debug.Log($"{hero2.GetName()} attacca {hero.GetName()} che e' pronto a difendersi!");
                hero.TakeDamage(CalculateDamage(hero2, hero));
            }
        }
        else
        {
            Debug.Log($"{hero2.GetName()} attacca per primo, {hero.GetName()} e' pronto a difendersi!");
            hero.TakeDamage(CalculateDamage(hero2, hero));

            if (hero.IsAlive())
            {
                Debug.Log($"{hero.GetName()} attacca {hero2.GetName()} che e' pronto a difendersi!");
                hero2.TakeDamage(CalculateDamage(hero, hero2));
            }
        }
    }

    public static void GetWinner(Hero hero, Hero hero2)
    {
        if (hero.GetHp() <= 0)
        {
            Debug.Log($"{hero.GetName()} e' morto! {hero2.GetName()} vince il duello.");
            continua = false;
            return;
        }
        else if (hero2.GetHp() <= 0)
        {
            Debug.Log($"{hero2.GetName()} e' morto! {hero.GetName()} vince il duello.");
            continua = false;
            return;
        }
        else
        {
            Debug.Log("I combattenti sono ancora vivi, il duello continua!");
        }
    }


    public static bool inizia = true;
    public static bool continua = true;  //bool per fermare l'Update() in determinati casi

}

