using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManager : MonoBehaviour
{
    [Tooltip("How much snow the player has left to spend.\n\n" +
        "プレイヤーが費やすために残した雪の量。")]
    [SerializeField] private int snowLeft = 500;
    /// <summary>
    /// How much snow the player has left to spend.<br/>
    /// プレイヤーが費やすために残した雪の量。
    /// </summary>
    public static int SnowLeft { get; private set; }

    private void Awake()
    {
        SnowLeft = snowLeft;
    }
    private void Update()
    {
        snowLeft = SnowLeft;
    }

    /// <summary>
    /// Attempt to spend <paramref name="spendAmount"/> snow. Snow will not be spent 
    /// if <see cref="SnowLeft"/> - <paramref name="spendAmount"/> < 0.<br/>
    /// <paramref name = "spendAmount" />雪を使ってみてください。 <see cref = "SnowLeft" />-
    /// <paramref name = "spendAmount" /> <0の場合、雪は消費されません。
    /// </summary>
    /// <param name="spendAmount">The amount of snow to try to spend.<br/>
    /// 費やそうとする雪の量。</param>
    /// <returns>Whether snow was successfully spent.<br/>
    /// 雪がうまく使われたかどうか。</returns>
    public static bool TrySpendSnow(int spendAmount)
    {
        if (SnowLeft - spendAmount >= 0)
        {
            SnowLeft -= spendAmount;
            return true;
        }
        else { return false; }
    }
}