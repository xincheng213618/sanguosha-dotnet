#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeSlashTargetDuelRuntime))]
internal sealed partial class CoreNativeSlashTargetDuelEntries
{
    private readonly Func<EngineOwners.CoreNativeSlashTargetDuelRuntime> _resolve;
    private CoreNativeSlashTargetDuelEntries(Func<EngineOwners.CoreNativeSlashTargetDuelRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSlashTargetDuelEntries Create(Func<EngineOwners.CoreNativeSlashTargetDuelRuntime> resolve) => new(resolve);
}
