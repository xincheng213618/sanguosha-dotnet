#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicReturnsEntries
{
    internal void AssertDrawFundedDistinctBasicUseCompletion(global::CardGame.Core.CardUseFrame use) => _resolve().AssertDrawFundedDistinctBasicUseCompletion(use);
    internal bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    internal bool DrawFundedDistinctBasicProgramActor(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.DrawFundedDistinctBasicPayment payment) => _resolve().DrawFundedDistinctBasicProgramActor(parent, payment);
    internal void EmitDrawFundedDistinctBasicReturned(global::CardGame.Core.DrawFundedDistinctBasicUseReceipt receipt) => _resolve().EmitDrawFundedDistinctBasicReturned(receipt);
    internal bool IsDrawFundedDistinctBasicAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsDrawFundedDistinctBasicAttackConsistent(attack, processing);
    internal bool IsDrawFundedDistinctBasicProgramSelection(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => _resolve().IsDrawFundedDistinctBasicProgramSelection(parent, effect, paid);
    internal global::CardGame.Core.Card ReadDrawFundedDistinctBasicUseAppearance(long ownerFrameId, global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadDrawFundedDistinctBasicUseAppearance(ownerFrameId, appearance);
    internal void ReturnDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame completed) => _resolve().ReturnDrawFundedDistinctBasicUse(completed);
    internal bool SkipDrawFundedDistinctBasicFinishedMovement(long frameId, global::CardGame.Core.Card card) => _resolve().SkipDrawFundedDistinctBasicFinishedMovement(frameId, card);
}
