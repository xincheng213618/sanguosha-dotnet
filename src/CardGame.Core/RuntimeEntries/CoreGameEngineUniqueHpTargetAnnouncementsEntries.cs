#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime))]
internal sealed partial class CoreGameEngineUniqueHpTargetAnnouncementsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> _resolve;
    private CoreGameEngineUniqueHpTargetAnnouncementsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueHpTargetAnnouncementsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpTargetAnnouncementsRuntime> resolve) => new(resolve);
}
