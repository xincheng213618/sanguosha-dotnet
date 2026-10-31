#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouBaMobileShiEntries
{
    internal void AnswerShiXb(PromptChoice selected) => _resolve().AnswerShiXb(selected);
    internal SkillProgramStepOutcome BeginShiXb(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginShiXb(input, op);
    internal PromptChoice ChooseShiXbAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiXbAi(decision, f);
    internal bool ContinuePaidShiXb(ProgramSkillFrame f) => _resolve().ContinuePaidShiXb(f);
    internal void ContinueShiXb(long id) => _resolve().ContinueShiXb(id);
    internal void ContinueShiXbLibbing(ProgramSkillFrame f) => _resolve().ContinueShiXbLibbing(f);
    internal void FinishShiXbOperation(ProgramSkillFrame f) => _resolve().FinishShiXbOperation(f);
    internal void IssueShiXbNativeUse(ProgramSkillFrame f) => _resolve().IssueShiXbNativeUse(f);
    internal void PublishShiXb(ProgramSkillFrame f) => _resolve().PublishShiXb(f);
    internal bool ResumeShiXb(long id) => _resolve().ResumeShiXb(id);
    internal bool ResumeShiXbAttack() => _resolve().ResumeShiXbAttack();
    internal bool ReturnShiXbMovement(ProgramSkillFrame f) => _resolve().ReturnShiXbMovement(f);
    internal bool ReturnShiXbNativeUse(CardUseFrame use) => _resolve().ReturnShiXbNativeUse(use);
    internal IReadOnlyList<PromptChoice> ShiXbChoices(ProgramSkillFrame f) => _resolve().ShiXbChoices(f);
    internal bool ShiXbExemptDebit(long useId, int actor) => _resolve().ShiXbExemptDebit(useId, actor);
    internal int ShiXbLibbingDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShiXbLibbingDamageBonus(attack);
    internal bool ShiXbLibbingIsDamageUse(ProgramCardUseContext use) => _resolve().ShiXbLibbingIsDamageUse(use);
    internal int ShiXbLibbingRangeBonus(int seat) => _resolve().ShiXbLibbingRangeBonus(seat);
    internal (int Last, int Count1, int Count2, int Count3) ShiXbLibbingState(int ownerSeat) => _resolve().ShiXbLibbingState(ownerSeat);
    internal bool ShiXbNativeMaterialBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().ShiXbNativeMaterialBatch(use, batch);
    internal bool ShiXbPoxiCannotRespond(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats) => _resolve().ShiXbPoxiCannotRespond(sourceSeat, responderSeat, incomingKind, targetSeats);
    internal Card ShiXbPoxiCard(ProgramSkillFrame f) => _resolve().ShiXbPoxiCard(f);
    internal IReadOnlyList<int> ShiXbPoxiTargets(ProgramSkillFrame f) => _resolve().ShiXbPoxiTargets(f);
    internal ShiXiahouBaReceipt StartShiXbLibbing(ProgramSkillFrame f) => _resolve().StartShiXbLibbing(f);
    internal ShiXiahouBaReceipt StartShiXbPoxi(ProgramSkillFrame f) => _resolve().StartShiXbPoxi(f);
    internal void ValidateShiXbNativeUse(CardUseFrame use) => _resolve().ValidateShiXbNativeUse(use);
    internal bool HasShiXbCapability { get => _resolve().HasShiXbCapability; }
}
