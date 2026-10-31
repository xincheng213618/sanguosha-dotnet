#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouBaMobileShiEntries
{
    internal void AnswerShiXb(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiXb(selected);
    internal void AssertShiXb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShiXb(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiXb(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShiXb(input, op);
    internal global::CardGame.Core.PromptChoice ChooseShiXbAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiXbAi(decision, f);
    internal bool ContinuePaidShiXb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShiXb(f);
    internal void ContinueShiXb(long id) => _resolve().ContinueShiXb(id);
    internal void ContinueShiXbLibbing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiXbLibbing(f);
    internal void FinishShiXbOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiXbOperation(f);
    internal void IssueShiXbNativeUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueShiXbNativeUse(f);
    internal void PublishShiXb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiXb(f);
    internal bool ResumeShiXb(long id) => _resolve().ResumeShiXb(id);
    internal bool ResumeShiXbAttack() => _resolve().ResumeShiXbAttack();
    internal bool ReturnShiXbMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiXbMovement(f);
    internal bool ReturnShiXbNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnShiXbNativeUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiXbChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiXbChoices(f);
    internal bool ShiXbExemptDebit(long useId, int actor) => _resolve().ShiXbExemptDebit(useId, actor);
    internal int ShiXbLibbingDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShiXbLibbingDamageBonus(attack);
    internal bool ShiXbLibbingIsDamageUse(global::CardGame.Core.ProgramCardUseContext use) => _resolve().ShiXbLibbingIsDamageUse(use);
    internal int ShiXbLibbingRangeBonus(int seat) => _resolve().ShiXbLibbingRangeBonus(seat);
    internal (int Last, int Count1, int Count2, int Count3) ShiXbLibbingState(int ownerSeat) => _resolve().ShiXbLibbingState(ownerSeat);
    internal bool ShiXbNativeMaterialBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ShiXbNativeMaterialBatch(use, batch);
    internal bool ShiXbPoxiCannotRespond(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => _resolve().ShiXbPoxiCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal global::CardGame.Core.Card ShiXbPoxiCard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiXbPoxiCard(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiXbPoxiTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiXbPoxiTargets(f);
    internal global::CardGame.Core.ShiXiahouBaReceipt StartShiXbLibbing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiXbLibbing(f);
    internal global::CardGame.Core.ShiXiahouBaReceipt StartShiXbPoxi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartShiXbPoxi(f);
    internal void ValidateShiXbNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateShiXbNativeUse(use);
    internal bool HasShiXbCapability { get => _resolve().HasShiXbCapability; }
}
