#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime))]
internal sealed partial class CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime> _resolve;
    private CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries(Func<EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDelayedNullifiedTrickClaimsEntries Create(Func<EngineOwners.CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime> resolve) => new(resolve);
}
