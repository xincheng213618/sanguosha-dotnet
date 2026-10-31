#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime))]
internal sealed partial class CoreGameEngineRecordedSuitSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> _resolve;
    private CoreGameEngineRecordedSuitSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecordedSuitSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> resolve) => new(resolve);
}
