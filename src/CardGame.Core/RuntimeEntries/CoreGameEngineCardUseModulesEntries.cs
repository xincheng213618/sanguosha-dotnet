#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed partial class CoreGameEngineCardUseModulesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _resolve;
    private CoreGameEngineCardUseModulesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseModulesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> resolve) => new(resolve);
}
