using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Snowman : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float meltAmount;
    [SerializeField] private float meltTime;
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
    /// How much health this snowman will lose every <see cref="MeltTime"/> seconds.<br/>
    /// この雪だるまが<see cref="MeltTime"/>秒ごとにどれだけの健康を失うか。
    /// </summary>
    public float MeltAmount { get { return meltAmount; } protected set { meltAmount = value; } }
    /// <summary>
    /// This snowman will lose <see cref="MeltAmount"/> health every <see cref="MeltTime"/> seconds.<br/>
    /// この雪だるまは<see cref="MeltTime"/>秒ごとに<see cref="MeltAmount"/>の体力を失います。
    /// </summary>
    public float MeltTime { get { return meltTime; } protected set { meltTime = value; } }

    protected virtual void Start()
    {
        maxHealth = Health;
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

    abstract protected void UseAbility();

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}