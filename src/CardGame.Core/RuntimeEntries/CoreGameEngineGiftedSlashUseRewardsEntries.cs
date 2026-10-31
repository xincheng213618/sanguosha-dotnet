#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime))]
internal sealed partial class CoreGameEngineGiftedSlashUseRewardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> _resolve;
    private CoreGameEngineGiftedSlashUseRewardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGiftedSlashUseRewardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> resolve) => new(resolve);
}
