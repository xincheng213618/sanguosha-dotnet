#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecordedSuitSlashRuntime))]
internal sealed partial class CoreGameEngineRecordedSuitSlashEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> _resolve;
    private CoreGameEngineRecordedSuitSlashEntries(Func<EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecordedSuitSlashEntries Create(Func<EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> resolve) => new(resolve);
}
