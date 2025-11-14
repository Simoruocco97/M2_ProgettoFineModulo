using UnityEngine;

public class M1ProjectTest : MonoBehaviour
{
    [SerializeField] Hero a;
    [SerializeField] Hero b;

    void Update()
    {
        GameFormulas.FightCheck(a, b);
    }
}