#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed partial class CoreGameEngineGuanHaiBridgesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _resolve;
    private CoreGameEngineGuanHaiBridgesEntries(Func<EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanHaiBridgesEntries Create(Func<EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> resolve) => new(resolve);
}
