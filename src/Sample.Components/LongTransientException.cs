namespace Sample.Components;

using System;


[Serializable]
public class LongTransientException :
    Exception
{
    public LongTransientException()
    {
    }

    public LongTransientException(string message)
        : base(message)
    {
    }
}