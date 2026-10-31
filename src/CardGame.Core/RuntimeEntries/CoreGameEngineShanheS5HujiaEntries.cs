#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5HujiaEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5HujiaRuntime> _resolve;
    private CoreGameEngineShanheS5HujiaEntries(Func<EngineOwners.CoreGameEngineShanheS5HujiaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5HujiaEntries Create(Func<EngineOwners.CoreGameEngineShanheS5HujiaRuntime> resolve) => new(resolve);
}
