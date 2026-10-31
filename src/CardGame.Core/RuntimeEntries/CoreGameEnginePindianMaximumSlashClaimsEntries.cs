#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime))]
internal sealed partial class CoreGameEnginePindianMaximumSlashClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> _resolve;
    private CoreGameEnginePindianMaximumSlashClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianMaximumSlashClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> resolve) => new(resolve);
}
