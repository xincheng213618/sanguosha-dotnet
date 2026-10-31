#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetGiftCapability(Func<RuntimeEntries.CoreGameEngineFinalTargetGiftsEntries> getCoreGameEngineFinalTargetGiftsRuntime) : IFinalTargetGiftHost
{
    public SkillProgramStepOutcome GiveOwnedCardToOtherFinalTargetAndDraw(ProgramSkillFrame frame) => getCoreGameEngineFinalTargetGiftsRuntime().BeginFinalTargetGift(frame);
}
