#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5YuanshuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YuanshuRuntime> _resolve;
    private CoreGameEngineShanheS5YuanshuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YuanshuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5YuanshuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YuanshuRuntime> resolve) => new(resolve);
}
