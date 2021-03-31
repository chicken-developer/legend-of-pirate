
public class MTimer
{
    float mTimer = 0;

    public void Update(float deltaTime)
    {
        if(mTimer < 0)
        {
            mTimer = 0;
            return;
        }
        mTimer -= deltaTime;
    }

    public void ResetTimer()
    {
        mTimer = 0;
    }

    public void SetDuration(float duration)
    {
        mTimer = duration;
    }

    public float GetTimer()
    {
        return mTimer;
    }

    public bool isDone()
    {
        return mTimer == 0;
    }
}
