#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed partial class CoreGameEngineGuanHaiBridgesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _resolve;
    private CoreGameEngineGuanHaiBridgesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanHaiBridgesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> resolve) => new(resolve);
}
