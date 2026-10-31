#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime))]
internal sealed partial class CoreGameEnginePaidColorDamageClaimObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> _resolve;
    private CoreGameEnginePaidColorDamageClaimObserversEntries(Func<EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidColorDamageClaimObserversEntries Create(Func<EngineOwners.CoreGameEnginePaidColorDamageClaimObserversRuntime> resolve) => new(resolve);
}
