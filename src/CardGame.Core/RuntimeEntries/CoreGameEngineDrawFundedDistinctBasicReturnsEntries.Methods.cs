#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicReturnsEntries
{
    internal void AssertDrawFundedDistinctBasicUseCompletion(CardUseFrame use) => _resolve().AssertDrawFundedDistinctBasicUseCompletion(use);
    internal bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, DyingFrame dying) => _resolve().DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    internal bool DrawFundedDistinctBasicProgramActor(ProgramSkillFrame parent, DrawFundedDistinctBasicPayment payment) => _resolve().DrawFundedDistinctBasicProgramActor(parent, payment);
    internal void EmitDrawFundedDistinctBasicReturned(DrawFundedDistinctBasicUseReceipt receipt) => _resolve().EmitDrawFundedDistinctBasicReturned(receipt);
    internal bool IsDrawFundedDistinctBasicProgramSelection(ProgramSkillFrame parent, SkillProgramEffect effect, DrawFundedDistinctBasicFrame paid) => _resolve().IsDrawFundedDistinctBasicProgramSelection(parent, effect, paid);
    internal Card ReadDrawFundedDistinctBasicUseAppearance(long ownerFrameId, CardAppearanceReference appearance) => _resolve().ReadDrawFundedDistinctBasicUseAppearance(ownerFrameId, appearance);
    internal void ReturnDrawFundedDistinctBasicUse(CardUseFrame completed) => _resolve().ReturnDrawFundedDistinctBasicUse(completed);
    internal bool SkipDrawFundedDistinctBasicFinishedMovement(long frameId, Card card) => _resolve().SkipDrawFundedDistinctBasicFinishedMovement(frameId, card);
}
