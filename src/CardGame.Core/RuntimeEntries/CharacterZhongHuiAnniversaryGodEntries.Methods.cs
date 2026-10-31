#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongHuiAnniversaryGodEntries
{
    internal void AnswerShenZh(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShenZh(choice);
    internal void BeginLishiJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginLishiJudgment(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenZh(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenZh(supplied, op);
    internal bool CanBeginLishiJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().CanBeginLishiJudgment(f, targetSeat, reason, bind, visibility);
    internal bool CanTriggerShenZh(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanTriggerShenZh(gate);
    internal global::CardGame.Core.PromptChoice ChooseShenZhAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShenZhAi(prompt, f);
    internal void ContinueLishiJudgments(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueLishiJudgments(f);
    internal void ContinueShenZh(long id) => _resolve().ContinueShenZh(id);
    internal bool DrainShenZh(long id) => _resolve().DrainShenZh(id);
    internal global::CardGame.Core.CardActionContext? DuzhangAction(global::CardGame.Core.ProgramSkillWindowContext? c) => _resolve().DuzhangAction(c);
    internal void FinishShenZh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenZh(f);
    internal bool HappyLishiAnyUseForbidden(global::CardGame.Core.ProgramCardUseQuery q) => _resolve().HappyLishiAnyUseForbidden(q);
    internal bool HasHappyLishiActualUseParticipant(int actor) => _resolve().HasHappyLishiActualUseParticipant(actor);
    internal bool IsLishiJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsLishiJudgment(j, f);
    internal bool IsLishiPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason) => _resolve().IsLishiPaidDiscardMovement(move, expectedSource, expectedReason);
    internal bool IsLishiPhaseSuppressed(int seat) => _resolve().IsLishiPhaseSuppressed(seat);
    internal bool IsLishiResponseUse(int actor, global::CardGame.Core.CardKind kind) => _resolve().IsLishiResponseUse(actor, kind);
    internal int LinCold(int seat) => _resolve().LinCold(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> LinjieDiscardCards(int owner, int target) => _resolve().LinjieDiscardCards(owner, target);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LishiPunishmentIssuedEvent> LishiActiveEffects(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().LishiActiveEffects(phase);
    internal global::CardGame.Core.LishiPunishmentIssuedEvent? LishiDiscardClaim(int actor) => _resolve().LishiDiscardClaim(actor);
    internal global::CardGame.Core.CardLocation? LishiDiscardOrigin(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason, long before) => _resolve().LishiDiscardOrigin(cardId, from, reason, before);
    internal bool LishiHandTypeSpent(int actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids, long? existing = default) => _resolve().LishiHandTypeSpent(actor, kind, ids, existing);
    internal string LishiJudgmentReason(global::CardGame.Core.ShenZhongHuiReceipt r) => _resolve().LishiJudgmentReason(r);
    internal bool LishiMaterialForbidden(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().LishiMaterialForbidden(q);
    internal bool LishiProjectedHandForbidden(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().LishiProjectedHandForbidden(q);
    internal bool LishiResponseHandForbidden(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().LishiResponseHandForbidden(q);
    internal void LishiSubmittedHandForbidden(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().LishiSubmittedHandForbidden(q);
    internal global::CardGame.Core.CardLocation NormalizeLishiDiscardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => _resolve().NormalizeLishiDiscardDestination(card, from, to, reason);
    internal void ObserveLishiPhase(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveLishiPhase(payload);
    internal void PublishShenZh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenZh(f);
    internal void RecordLishiDiscardRedirection(global::CardGame.Core.CardMovementRecord move) => _resolve().RecordLishiDiscardRedirection(move);
    internal void RecordLishiHandUse(global::CardGame.Core.CardActionContext action) => _resolve().RecordLishiHandUse(action);
    internal bool ResumeShenZh(long id) => _resolve().ResumeShenZh(id);
    internal bool RetainShenZh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainShenZh(f);
    internal void ReturnShenZhDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnShenZhDraw(supplied, o);
    internal bool ReturnShenZhMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenZhMovement(f);
    internal void SettleLishiDrawPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleLishiDrawPhase(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenZhChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenZhChoices(f);
    internal bool TryReturnLishiJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().TryReturnLishiJudgment(j);
    internal bool HasLishiCapability { get => _resolve().HasLishiCapability; }
}
