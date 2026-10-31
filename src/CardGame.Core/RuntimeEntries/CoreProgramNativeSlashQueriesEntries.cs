#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramNativeSlashQueriesRuntime))]
internal sealed partial class CoreProgramNativeSlashQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramNativeSlashQueriesRuntime> _resolve;
    private CoreProgramNativeSlashQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramNativeSlashQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramNativeSlashQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramNativeSlashQueriesRuntime> resolve) => new(resolve);
}
