using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<HelloWorldService>(Lifetime.Singleton);
    }
}

public class HelloWorldService
{
    public void Hello()
    {
        UnityEngine.Debug.Log("Hello world");
    }
}