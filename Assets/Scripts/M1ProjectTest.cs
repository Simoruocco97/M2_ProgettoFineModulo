using UnityEngine;

public class M1ProjectTest : MonoBehaviour
{
    [SerializeField] Hero a;
    [SerializeField] Hero b;

    void Update()
    {
        if (!a.IsAlive() || !b.IsAlive())
        {
            return;
        }

        GameFormulas.CheckForDuel(a, b);

        if (!a.IsAlive() || !b.IsAlive())
        {
            return;
        }

        GameFormulas.DuelStart(a, b);

        if (!a.IsAlive() || !b.IsAlive())
        {
            return;
        }
    }
}
