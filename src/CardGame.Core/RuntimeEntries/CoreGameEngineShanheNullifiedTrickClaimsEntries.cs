#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime))]
internal sealed partial class CoreGameEngineShanheNullifiedTrickClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> _resolve;
    private CoreGameEngineShanheNullifiedTrickClaimsEntries(Func<EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheNullifiedTrickClaimsEntries Create(Func<EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> resolve) => new(resolve);
}
