#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundFirstSlashTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime> _resolve;
    private CoreGameEngineRoundFirstSlashTargetsEntries(Func<EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundFirstSlashTargetsEntries Create(Func<EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime> resolve) => new(resolve);
}
