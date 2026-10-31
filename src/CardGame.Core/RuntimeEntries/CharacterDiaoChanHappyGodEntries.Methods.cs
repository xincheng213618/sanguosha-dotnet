#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanHappyGodEntries
{
    internal PromptChoice AiHappyDc(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHappyDc(p, f);
    internal void AnswerHappyDc(PromptChoice answer) => _resolve().AnswerHappyDc(answer);
    internal SkillProgramStepOutcome BeginHappyDc(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginHappyDc(supplied, operation);
    internal bool CanOfferHappyMeihun(ProgramTriggerLegalityContext gate) => _resolve().CanOfferHappyMeihun(gate);
    internal bool CanStartHappyHuoxin(CharacterState actor) => _resolve().CanStartHappyHuoxin(actor);
    internal void ContinueHappyDc(long id) => _resolve().ContinueHappyDc(id);
    internal bool DrainHappyDc(ProgramSkillFrame f) => _resolve().DrainHappyDc(f);
    internal void ExpireHappyDcCharm(IGameEvent payload) => _resolve().ExpireHappyDcCharm(payload);
    internal void FinishHappyDc(ProgramSkillFrame f) => _resolve().FinishHappyDc(f);
    internal void GiveHappyDc(ProgramSkillFrame f, int giver, IReadOnlyList<int> proposed, bool take) => _resolve().GiveHappyDc(f, giver, proposed, take);
    internal IEnumerable<HappyDiaoChanCharmIssuedEvent> HappyDcActiveCharms(int seat) => _resolve().HappyDcActiveCharms(seat);
    internal Card HappyDcCard(PromptChoice c, int seat) => _resolve().HappyDcCard(c, seat);
    internal ProgramSkillFrame HappyDcChildren(ProgramSkillFrame f, HappyDiaoChanStage stage) => _resolve().HappyDcChildren(f, stage);
    internal PromptChoice HappyDcChoice(ProgramSkillFrame f, string mode, string text, IReadOnlyList<int>? targets = default, IReadOnlyList<int>? cards = default, string? value = default) => _resolve().HappyDcChoice(f, mode, text, targets, cards, value);
    internal IReadOnlyList<PromptChoice> HappyDcChoices(ProgramSkillFrame f) => _resolve().HappyDcChoices(f);
    internal int HappyDcChooser(ProgramSkillFrame f) => _resolve().HappyDcChooser(f);
    internal IReadOnlyList<PromptChoice> HappyDcCosts(ProgramSkillFrame f) => _resolve().HappyDcCosts(f);
    internal bool HappyDcFirstSlash(int owner, ProgramSkillWindowContext context) => _resolve().HappyDcFirstSlash(owner, context);
    internal bool HappyDcMaterialBan(ProgramCardMaterialQuery q) => _resolve().HappyDcMaterialBan(q);
    internal bool HappyDcMaterialsForbidden(int seat, IEnumerable<int> material) => _resolve().HappyDcMaterialsForbidden(seat, material);
    internal bool HappyDcOriginalCandidate(ProgramSkillFrame f) => _resolve().HappyDcOriginalCandidate(f);
    internal IEnumerable<(int Source, int Opponent)> HappyDcPairs(int? discarded = default) => _resolve().HappyDcPairs(discarded);
    internal bool HappyDcProjectedUse(ProgramLegalActionQuery q) => _resolve().HappyDcProjectedUse(q);
    internal bool HappyDcResponseBan(ProgramNativeResponseQuery q) => _resolve().HappyDcResponseBan(q);
    internal CardConversionSource HappyDcSource(ProgramSkillFrame f) => _resolve().HappyDcSource(f);
    internal void HappyDcSubmittedUse(ProgramSubmittedUseQuery q) => _resolve().HappyDcSubmittedUse(q);
    internal int[] HappyDcSuitCards(int seat, Suit suit) => _resolve().HappyDcSuitCards(seat, suit);
    internal bool HappyDcViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().HappyDcViewAsBan(q);
    internal bool HappyDiaoChanStructuralEdge(ResolutionFrame child, ResolutionFrame parent) => _resolve().HappyDiaoChanStructuralEdge(child, parent);
    internal bool IsHappyDiaoChanPindianSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().IsHappyDiaoChanPindianSettlementBatch(f, batch);
    internal bool IsHappyDiaoChanSuitForbidden(int seat, Suit suit) => _resolve().IsHappyDiaoChanSuitForbidden(seat, suit);
    internal bool KeepHappyDcPaid(ProgramSkillFrame f) => _resolve().KeepHappyDcPaid(f);
    internal void PublishHappyDc(ProgramSkillFrame f) => _resolve().PublishHappyDc(f);
    internal bool ResumeHappyDc(long id) => _resolve().ResumeHappyDc(id);
    internal bool ReturnHappyDc(ProgramSkillFrame f) => _resolve().ReturnHappyDc(f);
    internal void SyncHappyDcCharmMarker(long frameId, int seat, Suit suit) => _resolve().SyncHappyDcCharmMarker(frameId, seat, suit);
    internal bool ValidHappyDc(ProgramSkillFrame f) => _resolve().ValidHappyDc(f);
    internal bool WindowSettlement_IsHappyDiaoChanPindianSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsHappyDiaoChanPindianSettlementBatch(frame, batch);
    internal bool HasHappyDiaoChan { get => _resolve().HasHappyDiaoChan; }
}
