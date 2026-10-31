#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime))]
internal sealed partial class CoreGameEngineProgramOwnerDeathDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> _resolve;
    private CoreGameEngineProgramOwnerDeathDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnerDeathDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> resolve) => new(resolve);
}
