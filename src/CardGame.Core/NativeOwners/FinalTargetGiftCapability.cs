#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetGiftCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFinalTargetGiftsRuntime>> getCoreGameEngineFinalTargetGiftsRuntime) : IFinalTargetGiftHost
{
    public SkillProgramStepOutcome GiveOwnedCardToOtherFinalTargetAndDraw(ProgramSkillFrame frame) => getCoreGameEngineFinalTargetGiftsRuntime().Value.BeginFinalTargetGift(frame);
}
