using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHp;
    public int maxHp;

    private void Start()
    {
        curHp = maxHp;
    }

    public void OnDamage(int damage)
    {
        curHp -= damage;
    }
}
