using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPile : MonoBehaviour
{
    [Tooltip("The amount of snow in this pile.\n\n" +
        "このパイルの雪の量。")]
    [SerializeField] private float amountOfSnow = 100;
    [Tooltip("How much snow is depleted from this pile per second, when the player is taking from it.\n\n" +
        "プレイヤーがこのパイルから取っているときに、このパイルから1秒間にどれだけの雪が枯渇するか。")]
    [SerializeField] private float depleteRate = 25;
    [SerializeField] private float depletedScalePercent = 0.5f;
    private float maxAmountOfSnow;
    /// <summary>
    /// Whatever <see cref="amountOfSnow"/> was when it was last set.
    /// </summary>
    private float previousAmountOfSnow;
    private Vector3 initialScale;

    public float AmountOfSnow
    {
        get { return amountOfSnow; }
        set
        {
            amountOfSnow = value;

            //If amountOfSnow ever passes a whole number threshold (Example: 5.24 -> 4.68), then add the whole 
            //number difference between the previous amountOfSnow value and the current one.
            //amountOfSnowが整数のしきい値を超えた場合（例：5.24-> 4.68）、
            //前のamountOfSnow値と現在の値の整数の差を加算します。
            if (Mathf.Floor(amountOfSnow) < Mathf.Floor(previousAmountOfSnow))
            {
                SnowManager.TrySpendSnow(Mathf.FloorToInt(amountOfSnow) - Mathf.FloorToInt(previousAmountOfSnow));
            }

            previousAmountOfSnow = amountOfSnow;
        }
    }

    private void Awake()
    {
        maxAmountOfSnow = amountOfSnow;
        initialScale = transform.localScale;

        ModeManager.TakeFromSnowPile += TryDepleteSnow;
    }
    private void OnDestroy() { ModeManager.TakeFromSnowPile -= TryDepleteSnow; }

    private void TryDepleteSnow(Transform depletingPile)
    {
        if (transform.Equals(depletingPile))
        {
            AmountOfSnow -= depleteRate * Time.deltaTime;
            transform.localScale = Vector3.Lerp(
                initialScale * depletedScalePercent,
                initialScale,
                AmountOfSnow / maxAmountOfSnow
            );

            if (AmountOfSnow < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
