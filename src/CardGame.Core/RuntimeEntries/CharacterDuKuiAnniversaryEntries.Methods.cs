#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuKuiAnniversaryEntries
{
    internal global::CardGame.Core.PromptChoice AiDuKuiFanyin(global::CardGame.Core.PendingDecision p) => _resolve().AiDuKuiFanyin(p);
    internal global::CardGame.Core.PromptChoice AiDuKuiPeiqi(global::CardGame.Core.PendingDecision p) => _resolve().AiDuKuiPeiqi(p);
    internal void AnswerDuKui(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerDuKui(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendDuKuiFanyinPlayableCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().AppendDuKuiFanyinPlayableCards(actor, cards);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDuKui(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginDuKui(supplied, op);
    internal void BindDuKuiNativeUse(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindDuKuiNativeUse(fact);
    internal bool CanOfferDuKui(global::CardGame.Core.ProgramTriggerLegalityContext q) => _resolve().CanOfferDuKui(q);
    internal bool ContinueDuKui(long id) => _resolve().ContinueDuKui(id);
    internal bool ContinuePaidDuKui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidDuKui(f);
    internal bool DrainDuKui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainDuKui(f);
    internal bool DuKuiAllMutualRanges() => _resolve().DuKuiAllMutualRanges();
    internal bool DuKuiCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DuKuiCardDistance(q);
    internal global::CardGame.Core.PromptChoice DuKuiChoice(global::CardGame.Core.ProgramSkillFrame f, string verb, string suffix, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().DuKuiChoice(f, verb, suffix, label, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DuKuiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuKuiChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> DuKuiFieldChoices(global::CardGame.Core.ProgramSkillFrame? f) => _resolve().DuKuiFieldChoices(f);
    internal bool DuKuiMovementReturned(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuKuiMovementReturned(f);
    internal global::CardGame.Core.ProgramSkillFrame? DuKuiQuery(int actor, int? id = default) => _resolve().DuKuiQuery(actor, id);
    internal bool DuKuiRevealedProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().DuKuiRevealedProcessing(cards);
    internal bool DuKuiShownPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuKuiShownPaid(f);
    internal bool DuKuiSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DuKuiSlashDistance(q);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> DuKuiUseActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuKuiUseActions(f);
    internal global::CardGame.Core.Card? FindDuKuiFanyinRevealedCard(global::CardGame.Core.CharacterState actor, int cardId) => _resolve().FindDuKuiFanyinRevealedCard(actor, cardId);
    internal void FinishDuKui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishDuKui(f);
    internal bool IsDuKuiFanyinSlashSource(long? programId, int source, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int provider, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physical, bool counts) => _resolve().IsDuKuiFanyinSlashSource(programId, source, target, card, kind, provider, from, physical, counts);
    internal bool IsDuKuiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsDuKuiNativeUse(use);
    internal void IssueDuKuiUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction selected) => _resolve().IssueDuKuiUse(f, selected);
    internal void MutateDuKui(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.DuKuiStage stage, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => _resolve().MutateDuKui(f, stage, card, from, to, reason);
    internal bool PayDuKuiProcessingMaterial(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int provider, global::CardGame.Core.CardLocation? from, global::CardGame.Core.CardMoveReason reason) => _resolve().PayDuKuiProcessingMaterial(cards, provider, from, reason);
    internal void PublishDuKui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDuKui(f);
    internal bool ReturnDuKuiNativeAttack() => _resolve().ReturnDuKuiNativeAttack();
    internal void ReturnDuKuiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnDuKuiNativeUse(use);
    internal void RevealNextDuKuiCard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RevealNextDuKuiCard(f);
    internal void SettleDuKuiNativeUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleDuKuiNativeUse(f);
    internal bool ValidDuKuiFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDuKuiFrame(f);
    internal void ValidateDuKuiNativeReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateDuKuiNativeReturn(use);
}
