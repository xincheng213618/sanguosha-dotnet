#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanHappyGodEntries
{
    internal global::CardGame.Core.PromptChoice AiHappyDc(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHappyDc(p, f);
    internal void AnswerHappyDc(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerHappyDc(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyDc(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginHappyDc(supplied, operation);
    internal bool CanOfferHappyMeihun(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferHappyMeihun(gate);
    internal bool CanStartHappyHuoxin(global::CardGame.Core.CharacterState actor) => _resolve().CanStartHappyHuoxin(actor);
    internal void ContinueHappyDc(long id) => _resolve().ContinueHappyDc(id);
    internal bool DrainHappyDc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHappyDc(f);
    internal void ExpireHappyDcCharm(global::CardGame.Core.IGameEvent payload) => _resolve().ExpireHappyDcCharm(payload);
    internal void FinishHappyDc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHappyDc(f);
    internal void GiveHappyDc(global::CardGame.Core.ProgramSkillFrame f, int giver, global::System.Collections.Generic.IReadOnlyList<int> proposed, bool take) => _resolve().GiveHappyDc(f, giver, proposed, take);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.HappyDiaoChanCharmIssuedEvent> HappyDcActiveCharms(int seat) => _resolve().HappyDcActiveCharms(seat);
    internal global::CardGame.Core.Card HappyDcCard(global::CardGame.Core.PromptChoice c, int seat) => _resolve().HappyDcCard(c, seat);
    internal global::CardGame.Core.ProgramSkillFrame HappyDcChildren(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HappyDiaoChanStage stage) => _resolve().HappyDcChildren(f, stage);
    internal global::CardGame.Core.PromptChoice HappyDcChoice(global::CardGame.Core.ProgramSkillFrame f, string mode, string text, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, string? value = default) => _resolve().HappyDcChoice(f, mode, text, targets, cards, value);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyDcChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyDcChoices(f);
    internal int HappyDcChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyDcChooser(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyDcCosts(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyDcCosts(f);
    internal bool HappyDcFirstSlash(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HappyDcFirstSlash(owner, context);
    internal bool HappyDcMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().HappyDcMaterialBan(q);
    internal bool HappyDcMaterialsForbidden(int seat, global::System.Collections.Generic.IEnumerable<int> material) => _resolve().HappyDcMaterialsForbidden(seat, material);
    internal bool HappyDcMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HappyDcMovement(f, effect, p);
    internal bool HappyDcOriginalCandidate(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyDcOriginalCandidate(f);
    internal global::System.Collections.Generic.IEnumerable<(int Source, int Opponent)> HappyDcPairs(int? discarded = default) => _resolve().HappyDcPairs(discarded);
    internal bool HappyDcProjectedUse(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().HappyDcProjectedUse(q);
    internal bool HappyDcResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().HappyDcResponseBan(q);
    internal global::CardGame.Core.CardConversionSource HappyDcSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyDcSource(f);
    internal void HappyDcSubmittedUse(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().HappyDcSubmittedUse(q);
    internal int[] HappyDcSuitCards(int seat, global::CardGame.Core.Suit suit) => _resolve().HappyDcSuitCards(seat, suit);
    internal bool HappyDcViewAsBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().HappyDcViewAsBan(q);
    internal bool HappyDiaoChanStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().HappyDiaoChanStructuralEdge(child, parent);
    internal bool IsHappyDiaoChanPindianSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsHappyDiaoChanPindianSettlementBatch(f, batch);
    internal bool IsHappyDiaoChanSuitForbidden(int seat, global::CardGame.Core.Suit suit) => _resolve().IsHappyDiaoChanSuitForbidden(seat, suit);
    internal bool KeepHappyDcPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepHappyDcPaid(f);
    internal void PublishHappyDc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyDc(f);
    internal bool ResumeHappyDc(long id) => _resolve().ResumeHappyDc(id);
    internal bool ReturnHappyDc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyDc(f);
    internal void SyncHappyDcCharmMarker(long frameId, int seat, global::CardGame.Core.Suit suit) => _resolve().SyncHappyDcCharmMarker(frameId, seat, suit);
    internal bool ValidHappyDc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyDc(f);
    internal bool WindowSettlement_IsHappyDiaoChanPindianSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsHappyDiaoChanPindianSettlementBatch(frame, batch);
    internal bool HasHappyDiaoChan { get => _resolve().HasHappyDiaoChan; }
}
