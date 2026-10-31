#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime))]
internal sealed partial class CoreGameEngineShanheNullifiedClaimObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> _resolve;
    private CoreGameEngineShanheNullifiedClaimObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheNullifiedClaimObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedClaimObserversRuntime> resolve) => new(resolve);
}
