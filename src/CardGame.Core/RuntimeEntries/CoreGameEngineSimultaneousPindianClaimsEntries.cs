#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> resolve) => new(resolve);
}
