#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongHuiAnniversaryGodEntries
{
    internal void AnswerShenZh(PromptChoice choice) => _resolve().AnswerShenZh(choice);
    internal void BeginLishiJudgment(ProgramSkillFrame f) => _resolve().BeginLishiJudgment(f);
    internal SkillProgramStepOutcome BeginShenZh(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenZh(supplied, op);
    internal bool CanBeginLishiJudgment(ProgramSkillFrame f, int targetSeat, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().CanBeginLishiJudgment(f, targetSeat, reason, bind, visibility);
    internal bool CanTriggerShenZh(ProgramTriggerLegalityContext gate) => _resolve().CanTriggerShenZh(gate);
    internal PromptChoice ChooseShenZhAi(PendingDecision prompt, ProgramSkillFrame f) => _resolve().ChooseShenZhAi(prompt, f);
    internal void ContinueLishiJudgments(ProgramSkillFrame f) => _resolve().ContinueLishiJudgments(f);
    internal void ContinueShenZh(long id) => _resolve().ContinueShenZh(id);
    internal bool DrainShenZh(long id) => _resolve().DrainShenZh(id);
    internal CardActionContext? DuzhangAction(ProgramSkillWindowContext? c) => _resolve().DuzhangAction(c);
    internal void FinishShenZh(ProgramSkillFrame f) => _resolve().FinishShenZh(f);
    internal bool HappyLishiAnyUseForbidden(ProgramCardUseQuery q) => _resolve().HappyLishiAnyUseForbidden(q);
    internal bool HasHappyLishiActualUseParticipant(int actor) => _resolve().HasHappyLishiActualUseParticipant(actor);
    internal bool IsLishiJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsLishiJudgment(j, f);
    internal bool IsLishiPaidDiscardMovement(CardMovementRecord move, CardLocation expectedSource, CardMoveReason expectedReason) => _resolve().IsLishiPaidDiscardMovement(move, expectedSource, expectedReason);
    internal bool IsLishiResponseUse(int actor, CardKind kind) => _resolve().IsLishiResponseUse(actor, kind);
    internal int LinCold(int seat) => _resolve().LinCold(seat);
    internal IReadOnlyList<Card> LinjieDiscardCards(int owner, int target) => _resolve().LinjieDiscardCards(owner, target);
    internal IEnumerable<LishiPunishmentIssuedEvent> LishiActiveEffects(ActualDiscardRecoveryPhaseKey phase) => _resolve().LishiActiveEffects(phase);
    internal LishiPunishmentIssuedEvent? LishiDiscardClaim(int actor) => _resolve().LishiDiscardClaim(actor);
    internal CardLocation? LishiDiscardOrigin(int cardId, CardLocation from, CardMoveReason reason, long before) => _resolve().LishiDiscardOrigin(cardId, from, reason, before);
    internal bool LishiHandTypeSpent(int actor, CardKind kind, IReadOnlyList<int> ids, long? existing = default) => _resolve().LishiHandTypeSpent(actor, kind, ids, existing);
    internal string LishiJudgmentReason(ShenZhongHuiReceipt r) => _resolve().LishiJudgmentReason(r);
    internal bool LishiMaterialForbidden(ProgramCardMaterialQuery q) => _resolve().LishiMaterialForbidden(q);
    internal bool LishiProjectedHandForbidden(ProgramLegalActionQuery q) => _resolve().LishiProjectedHandForbidden(q);
    internal bool LishiResponseHandForbidden(ProgramNativeResponseQuery q) => _resolve().LishiResponseHandForbidden(q);
    internal void LishiSubmittedHandForbidden(ProgramSubmittedUseQuery q) => _resolve().LishiSubmittedHandForbidden(q);
    internal CardLocation NormalizeLishiDiscardDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _resolve().NormalizeLishiDiscardDestination(card, from, to, reason);
    internal void ObserveLishiPhase(IGameEvent payload) => _resolve().ObserveLishiPhase(payload);
    internal void PublishShenZh(ProgramSkillFrame f) => _resolve().PublishShenZh(f);
    internal void RecordLishiDiscardRedirection(CardMovementRecord move) => _resolve().RecordLishiDiscardRedirection(move);
    internal void RecordLishiHandUse(CardActionContext action) => _resolve().RecordLishiHandUse(action);
    internal bool ResumeShenZh(long id) => _resolve().ResumeShenZh(id);
    internal bool RetainShenZh(ProgramSkillFrame f) => _resolve().RetainShenZh(f);
    internal void ReturnShenZhDraw(ProgramSkillFrame supplied, NativeDrawOutcome o) => _resolve().ReturnShenZhDraw(supplied, o);
    internal bool ReturnShenZhMovement(ProgramSkillFrame f) => _resolve().ReturnShenZhMovement(f);
    internal void SettleLishiDrawPhase(ProgramSkillFrame f) => _resolve().SettleLishiDrawPhase(f);
    internal IReadOnlyList<PromptChoice> ShenZhChoices(ProgramSkillFrame f) => _resolve().ShenZhChoices(f);
    internal bool TryReturnLishiJudgment(JudgmentFrame j) => _resolve().TryReturnLishiJudgment(j);
    internal bool HasLishiCapability { get => _resolve().HasLishiCapability; }
}
