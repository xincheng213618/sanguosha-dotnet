#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime))]
internal sealed partial class CoreGameEngineUniqueHpTargetAnnouncementsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> _resolve;
    private CoreGameEngineUniqueHpTargetAnnouncementsEntries(Func<EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueHpTargetAnnouncementsEntries Create(Func<EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> resolve) => new(resolve);
}
