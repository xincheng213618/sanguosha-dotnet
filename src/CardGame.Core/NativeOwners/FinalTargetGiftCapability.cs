#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetGiftCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetGiftsEntries> getCoreGameEngineFinalTargetGiftsRuntime) : IFinalTargetGiftHost
{
    public SkillProgramStepOutcome GiveOwnedCardToOtherFinalTargetAndDraw(ProgramSkillFrame frame) => getCoreGameEngineFinalTargetGiftsRuntime().BeginFinalTargetGift(frame);
}
