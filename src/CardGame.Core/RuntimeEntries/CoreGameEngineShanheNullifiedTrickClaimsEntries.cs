#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime))]
internal sealed partial class CoreGameEngineShanheNullifiedTrickClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> _resolve;
    private CoreGameEngineShanheNullifiedTrickClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheNullifiedTrickClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> resolve) => new(resolve);
}
