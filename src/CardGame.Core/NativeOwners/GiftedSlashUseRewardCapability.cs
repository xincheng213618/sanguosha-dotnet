#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GiftedSlashUseRewardCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime>> getCoreGameEngineGiftedSlashUseRewardsRuntime) : IGiftedSlashUseRewardHost
{
    public SkillProgramStepOutcome BeginGiftedSlashGift(SkillProgramEffect effect, ProgramSkillFrame frame, int recipientSeat) => getCoreGameEngineGiftedSlashUseRewardsRuntime().Value.BeginGiftedSlashGift(effect, frame, recipientSeat);
    public SkillProgramStepOutcome BeginGiftedSlashUseReward(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineGiftedSlashUseRewardsRuntime().Value.BeginGiftedSlashUseReward(effect, frame);
}
