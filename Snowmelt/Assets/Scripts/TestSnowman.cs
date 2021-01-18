using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSnowman : Snowman
{
    [SerializeField] private float damagedScalePercent = 1;
    [SerializeField] private float abilityCooldown = 1;
    private Vector3 maxScale = Vector3.zero;
    private float abilityTimer;

    protected override void Start()
    {
        base.Start();
        maxScale = transform.localScale;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 previousScale = transform.localScale;
        float percentDamaged = Mathf.InverseLerp(0, maxHealth, Health);
        transform.localScale = Vector3.Lerp(maxScale * damagedScalePercent, maxScale, percentDamaged);
        transform.position -= Vector3.up * ((previousScale.y - transform.localScale.y) / 2);

        abilityTimer += Time.deltaTime;
        if (abilityTimer > abilityCooldown)
        {
            abilityTimer = 0;
            UseAbility();
        }
    }

    protected override void UseAbility()
    {
        Debug.Log("Ability used\t使用される能力");
    }

    protected override void Die()
    {
        Debug.Log("I AM BECOME DIE\t私は死にます");
        base.Die();
    }
}