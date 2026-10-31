#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianClaimsEntries(Func<EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianClaimsEntries Create(Func<EngineOwners.CoreGameEngineSimultaneousPindianClaimsRuntime> resolve) => new(resolve);
}
