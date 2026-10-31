#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime))]
internal sealed partial class CoreGameEnginePindianMaximumSlashClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> _resolve;
    private CoreGameEnginePindianMaximumSlashClaimsEntries(Func<EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianMaximumSlashClaimsEntries Create(Func<EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> resolve) => new(resolve);
}
