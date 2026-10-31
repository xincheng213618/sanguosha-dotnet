#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnerDeathDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> _resolve;
    private CoreGameEngineProgramOwnerDeathDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnerDeathDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> resolve) => new(resolve);
}
