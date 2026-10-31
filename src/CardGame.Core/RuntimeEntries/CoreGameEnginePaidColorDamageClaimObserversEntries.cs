#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime))]
internal sealed partial class CoreGameEnginePaidColorDamageClaimObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> _resolve;
    private CoreGameEnginePaidColorDamageClaimObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidColorDamageClaimObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> resolve) => new(resolve);
}
