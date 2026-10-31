#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5YuanshuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5YuanshuRuntime> _resolve;
    private CoreGameEngineShanheS5YuanshuEntries(Func<EngineOwners.CoreGameEngineShanheS5YuanshuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5YuanshuEntries Create(Func<EngineOwners.CoreGameEngineShanheS5YuanshuRuntime> resolve) => new(resolve);
}
