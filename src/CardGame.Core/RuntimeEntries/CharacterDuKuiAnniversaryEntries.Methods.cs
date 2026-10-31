#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuKuiAnniversaryEntries
{
    internal PromptChoice AiDuKuiFanyin(PendingDecision p) => _resolve().AiDuKuiFanyin(p);
    internal PromptChoice AiDuKuiPeiqi(PendingDecision p) => _resolve().AiDuKuiPeiqi(p);
    internal void AnswerDuKui(PromptChoice choice) => _resolve().AnswerDuKui(choice);
    internal IReadOnlyList<Card> AppendDuKuiFanyinPlayableCards(CharacterState actor, IReadOnlyList<Card> cards) => _resolve().AppendDuKuiFanyinPlayableCards(actor, cards);
    internal SkillProgramStepOutcome BeginDuKui(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginDuKui(supplied, op);
    internal void BindDuKuiNativeUse(CardUseDeclaredEvent fact) => _resolve().BindDuKuiNativeUse(fact);
    internal bool CanOfferDuKui(ProgramTriggerLegalityContext q) => _resolve().CanOfferDuKui(q);
    internal bool ContinueDuKui(long id) => _resolve().ContinueDuKui(id);
    internal bool ContinuePaidDuKui(ProgramSkillFrame f) => _resolve().ContinuePaidDuKui(f);
    internal bool DrainDuKui(ProgramSkillFrame f) => _resolve().DrainDuKui(f);
    internal bool DuKuiAllMutualRanges() => _resolve().DuKuiAllMutualRanges();
    internal bool DuKuiCardDistance(ProgramCardDistanceQuery q) => _resolve().DuKuiCardDistance(q);
    internal PromptChoice DuKuiChoice(ProgramSkillFrame f, string verb, string suffix, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().DuKuiChoice(f, verb, suffix, label, cards, targets);
    internal IReadOnlyList<PromptChoice> DuKuiChoices(ProgramSkillFrame f) => _resolve().DuKuiChoices(f);
    internal IEnumerable<PromptChoice> DuKuiFieldChoices(ProgramSkillFrame? f) => _resolve().DuKuiFieldChoices(f);
    internal bool DuKuiMovementReturned(ProgramSkillFrame f) => _resolve().DuKuiMovementReturned(f);
    internal ProgramSkillFrame? DuKuiQuery(int actor, int? id = default) => _resolve().DuKuiQuery(actor, id);
    internal bool DuKuiRevealedProcessing(IReadOnlyList<Card> cards) => _resolve().DuKuiRevealedProcessing(cards);
    internal bool DuKuiShownPaid(ProgramSkillFrame f) => _resolve().DuKuiShownPaid(f);
    internal bool DuKuiSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DuKuiSlashDistance(q);
    internal IReadOnlyList<LegalAction> DuKuiUseActions(ProgramSkillFrame f) => _resolve().DuKuiUseActions(f);
    internal Card? FindDuKuiFanyinRevealedCard(CharacterState actor, int cardId) => _resolve().FindDuKuiFanyinRevealedCard(actor, cardId);
    internal void FinishDuKui(ProgramSkillFrame f) => _resolve().FinishDuKui(f);
    internal bool IsDuKuiFanyinSlashSource(long? programId, int source, int target, Card card, CardKind kind, int provider, CardLocation from, IReadOnlyList<Card>? physical, bool counts) => _resolve().IsDuKuiFanyinSlashSource(programId, source, target, card, kind, provider, from, physical, counts);
    internal bool IsDuKuiNativeUse(CardUseFrame use) => _resolve().IsDuKuiNativeUse(use);
    internal void IssueDuKuiUse(ProgramSkillFrame f, LegalAction selected) => _resolve().IssueDuKuiUse(f, selected);
    internal void MutateDuKui(ProgramSkillFrame f, DuKuiStage stage, Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _resolve().MutateDuKui(f, stage, card, from, to, reason);
    internal bool PayDuKuiProcessingMaterial(IReadOnlyList<Card> cards, int provider, CardLocation? from, CardMoveReason reason) => _resolve().PayDuKuiProcessingMaterial(cards, provider, from, reason);
    internal void PublishDuKui(ProgramSkillFrame f) => _resolve().PublishDuKui(f);
    internal bool ReturnDuKuiNativeAttack() => _resolve().ReturnDuKuiNativeAttack();
    internal void ReturnDuKuiNativeUse(CardUseFrame use) => _resolve().ReturnDuKuiNativeUse(use);
    internal void RevealNextDuKuiCard(ProgramSkillFrame f) => _resolve().RevealNextDuKuiCard(f);
    internal void SettleDuKuiNativeUse(ProgramSkillFrame f) => _resolve().SettleDuKuiNativeUse(f);
    internal bool ValidDuKuiFrame(ProgramSkillFrame f) => _resolve().ValidDuKuiFrame(f);
    internal void ValidateDuKuiNativeReturn(CardUseFrame use) => _resolve().ValidateDuKuiNativeReturn(use);
}
