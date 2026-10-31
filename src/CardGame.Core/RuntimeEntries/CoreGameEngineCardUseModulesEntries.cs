#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUseModulesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _resolve;
    private CoreGameEngineCardUseModulesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseModulesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> resolve) => new(resolve);
}
