#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GiftedSlashUseRewardCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGiftedSlashUseRewardsEntries> getCoreGameEngineGiftedSlashUseRewardsRuntime) : IGiftedSlashUseRewardHost
{
    public SkillProgramStepOutcome BeginGiftedSlashGift(SkillProgramEffect effect, ProgramSkillFrame frame, int recipientSeat) => getCoreGameEngineGiftedSlashUseRewardsRuntime().BeginGiftedSlashGift(effect, frame, recipientSeat);
    public SkillProgramStepOutcome BeginGiftedSlashUseReward(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineGiftedSlashUseRewardsRuntime().BeginGiftedSlashUseReward(effect, frame);
}
