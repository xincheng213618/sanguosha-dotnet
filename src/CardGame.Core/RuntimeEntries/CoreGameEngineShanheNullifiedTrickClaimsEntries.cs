#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheNullifiedTrickClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> _resolve;
    private CoreGameEngineShanheNullifiedTrickClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheNullifiedTrickClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> resolve) => new(resolve);
}
