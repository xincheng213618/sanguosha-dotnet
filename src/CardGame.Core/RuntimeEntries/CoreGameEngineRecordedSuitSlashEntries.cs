#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecordedSuitSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> _resolve;
    private CoreGameEngineRecordedSuitSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecordedSuitSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> resolve) => new(resolve);
}
