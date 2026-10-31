#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime))]
internal sealed partial class CoreGameEngineGiftedSlashUseRewardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> _resolve;
    private CoreGameEngineGiftedSlashUseRewardsEntries(Func<EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGiftedSlashUseRewardsEntries Create(Func<EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> resolve) => new(resolve);
}
