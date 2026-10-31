#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianClaimsRuntime))]
internal sealed partial class CoreGameEnginePindianClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianClaimsRuntime> _resolve;
    private CoreGameEnginePindianClaimsEntries(Func<EngineOwners.CoreGameEnginePindianClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianClaimsEntries Create(Func<EngineOwners.CoreGameEnginePindianClaimsRuntime> resolve) => new(resolve);
}
