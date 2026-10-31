#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GiftedSlashUseRewardCapability(Func<RuntimeEntries.CoreGameEngineGiftedSlashUseRewardsEntries> getCoreGameEngineGiftedSlashUseRewardsRuntime) : IGiftedSlashUseRewardHost
{
    public SkillProgramStepOutcome BeginGiftedSlashGift(SkillProgramEffect effect, ProgramSkillFrame frame, int recipientSeat) => getCoreGameEngineGiftedSlashUseRewardsRuntime().BeginGiftedSlashGift(effect, frame, recipientSeat);
    public SkillProgramStepOutcome BeginGiftedSlashUseReward(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineGiftedSlashUseRewardsRuntime().BeginGiftedSlashUseReward(effect, frame);
}
