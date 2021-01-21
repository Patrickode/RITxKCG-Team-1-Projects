using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSnowman : Snowman
{
    [SerializeField] private float damagedScalePercent = 1;
    [SerializeField] private float abilityCooldown = 1;
    private Vector3 initialScale = Vector3.zero;
    private float abilityTimer;

    protected override void Start()
    {
        base.Start();
        initialScale = transform.localScale;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 previousScale = transform.localScale;
        transform.localScale = Vector3.Lerp(initialScale * damagedScalePercent, initialScale, HealthPercentage);
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
        Debug.Log($"{gameObject.name}: Ability used\t使用される能力");
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name}: I AM BECOME DIE\t私は死にます");
        base.Die();
    }
}