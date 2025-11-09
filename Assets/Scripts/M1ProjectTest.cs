using UnityEngine;

public class M1ProjectTest : MonoBehaviour
{
    [SerializeField] Hero a;
    [SerializeField] Hero b;

    void Start()
    {

    }

    void Update()
    {
        if (!GameFormulas.inizia || !GameFormulas.continua)
        {
            return;
        }

        GameFormulas.CheckForDuel(a, b);

        if (!GameFormulas.continua || !GameFormulas.inizia)
        {
            return;
        }

        GameFormulas.DuelStart(a, b);
        GameFormulas.GetWinner(a, b);
    }
}
