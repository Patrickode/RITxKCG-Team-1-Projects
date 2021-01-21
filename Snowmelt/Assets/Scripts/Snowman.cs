using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Snowman : MonoBehaviour
{
    [SerializeField] private float health;
    [Tooltip("How much health this snowman will lose every MeltTime seconds.\n\n" +
        "この雪だるまがMeltTime秒ごとにどれだけの健康を失うか。")]
    [SerializeField] private float meltAmount;
    [Tooltip("This snowman will lose MeltAmount health every MeltTime seconds.\n\n" +
        "この雪だるまはMeltTime秒ごとにMeltAmountの体力を失います。")]
    [SerializeField] private float meltTime;
    [Tooltip("How much snow it will cost to build this snowman.\n\n" +
        "この雪だるまを作るのにどれくらいの雪がかかりますか。")]
    [SerializeField] private int costToBuild;
    [Tooltip("The minimum amount of snow the player can get when refunding this snowman.\n\n" +
        "この雪だるまに返金するときにプレイヤーが得ることができる雪の最小量。")]
    [SerializeField] private int minimumRefund;
    protected float maxHealth;
    protected float meltTimerProgress;

    public float Health
    {
        get { return health; }
        //When Health is changed,
        //ヘルスが変更されたとき、
        protected set
        {
            health = value;
            //If this snowman has no health left, make sure health isn't negative, then perform dying logic.
            //この雪だるまに体力が残っていない場合は、体力がマイナスになっていないことを確認してから、
            //死にかけているロジックを実行します。
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }
    /// <summary>
    /// <see cref="HealthPercentage"/> = <see cref="Health"/> / <see cref="maxHealth"/>
    /// </summary>
    public float HealthPercentage => Health / maxHealth;
    /// <summary>
    /// How much health this snowman will lose every <see cref="MeltTime"/> seconds.<br/>
    /// この雪だるまが<see cref="MeltTime"/>秒ごとにどれだけの健康を失うか。
    /// </summary>
    public float MeltAmount { get { return meltAmount; } protected set { meltAmount = value; } }
    /// <summary>
    /// This snowman will lose <see cref="MeltAmount"/> health every <see cref="MeltTime"/> seconds.<br/>
    /// この雪だるまは<see cref="MeltTime"/>秒ごとに<see cref="MeltAmount"/>の体力を失います。
    /// </summary>
    public float MeltTime { get { return meltTime; } protected set { meltTime = value; } }
    /// <summary>
    /// "How much snow it will cost to build this snowman.<br/>
    /// この雪だるまを作るのにどれくらいの雪がかかりますか。
    /// </summary>
    public int CostToBuild { get { return costToBuild; } protected set { costToBuild = value; } }

    protected virtual void Start()
    {
        maxHealth = Health;
        ModeManager.HealSnowman += TryHeal;
        ModeManager.RefundSnowman += Refund;
    }
    protected virtual void OnDestroy()
    {
        ModeManager.HealSnowman -= TryHeal;
        ModeManager.RefundSnowman -= Refund;
    }

    protected virtual void Update()
    {
        //Increment the melt timer.
        //メルトタイマーをインクリメントします。
        meltTimerProgress += Time.deltaTime / MeltTime;
        //If MeltTime seconds have passed,
        //MeltTime秒が経過した場合、
        if (meltTimerProgress >= 1)
        {
            //Reset the melt timer, then subtract MeltAmount health.
            //メルトタイマーをリセットしてから、MeltAmountヘルスを減算します。
            meltTimerProgress = 0;
            Health -= MeltAmount;
        }
    }

    protected virtual void UseAbility() { }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Try to heal this snowman back to full health. This method will fail if the player doesn't 
    /// have enough snow.<br/>
    /// この雪だるまを完全な健康状態に戻すようにしてください。 プレイヤーに十分な雪がない場合、この方法は失敗します。
    /// </summary>
    /// <param name="snowmanHealed">The transform of the snowman that should be healed.<br/>
    /// 癒されるべき雪だるまのtransform。</param>
    protected virtual void TryHeal(Transform snowmanHealed)
    {
        //All snowmen subscribe to this event, so make sure this snowman is the one that should be healed.
        //すべての雪だるまがこのイベントに登録しているので、この雪だるまが癒されるべきものであることを確認してください。
        if (transform.Equals(snowmanHealed))
        {
            float healthToRestore = maxHealth - Health;
            if (SnowManager.TrySpendSnow(Mathf.FloorToInt(healthToRestore)))
            {
                Health = maxHealth;
            }
        }
    }

    protected virtual void Refund(Transform snowmanRefunded)
    {
        if (transform.Equals(snowmanRefunded))
        {
            float refundAmount = Mathf.Lerp(minimumRefund, costToBuild, HealthPercentage);
            SnowManager.TrySpendSnow(-Mathf.CeilToInt(refundAmount));
            Die();
        }
    }
}