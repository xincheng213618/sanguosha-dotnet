#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime))]
internal sealed partial class CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime> _resolve;
    private CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime> resolve) => new(resolve);
}
