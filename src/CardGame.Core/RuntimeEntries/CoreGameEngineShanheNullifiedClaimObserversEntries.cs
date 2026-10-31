#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime))]
internal sealed partial class CoreGameEngineShanheNullifiedClaimObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> _resolve;
    private CoreGameEngineShanheNullifiedClaimObserversEntries(Func<EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheNullifiedClaimObserversEntries Create(Func<EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> resolve) => new(resolve);
}
