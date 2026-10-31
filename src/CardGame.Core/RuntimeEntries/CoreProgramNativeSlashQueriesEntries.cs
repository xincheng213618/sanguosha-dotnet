#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramNativeSlashQueriesRuntime))]
internal sealed partial class CoreProgramNativeSlashQueriesEntries
{
    private readonly Func<EngineOwners.CoreProgramNativeSlashQueriesRuntime> _resolve;
    private CoreProgramNativeSlashQueriesEntries(Func<EngineOwners.CoreProgramNativeSlashQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramNativeSlashQueriesEntries Create(Func<EngineOwners.CoreProgramNativeSlashQueriesRuntime> resolve) => new(resolve);
}
