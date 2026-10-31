#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaiLingYunAnniversaryEntries
{
    internal bool AdvanceAnniversaryBlyChildren() => _resolve().AdvanceAnniversaryBlyChildren();
    internal bool AnniversaryBlyBeforeDamageObserver(long id) => _resolve().AnniversaryBlyBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversaryBlyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryBlyChoices(f);
    internal bool AnniversaryBlyDamageObserver(long id) => _resolve().AnniversaryBlyDamageObserver(id);
    internal bool AnniversaryBlyDyingObserver() => _resolve().AnniversaryBlyDyingObserver();
    internal bool AnniversaryBlyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryBlyFirstChild(f, child);
    internal bool AnniversaryBlyMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().AnniversaryBlyMovementProof(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryBlyObserverRoot() => _resolve().AnniversaryBlyObserverRoot();
    internal bool AnniversaryBlyProcessingAllowance(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().AnniversaryBlyProcessingAllowance(cards);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryBlyQuery(int actor, int? card = default) => _resolve().AnniversaryBlyQuery(actor, card);
    internal bool AnniversaryBlyQueryableCard(global::CardGame.Core.ProgramSkillFrame f, int id) => _resolve().AnniversaryBlyQueryableCard(f, id);
    internal bool AnniversaryBlyRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().AnniversaryBlyRecoveryParent(f, recovery);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> AnniversaryBlyUseActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryBlyUseActions(f);
    internal bool AnniversaryBlyUsePaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryBlyUsePaid(f);
    internal void AnswerAnniversaryBly(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryBly(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendAnniversaryBaiLingYunPlayableCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().AppendAnniversaryBaiLingYunPlayableCards(actor, cards);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryBly(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginAnniversaryBly(supplied);
    internal void BindAnniversaryBlyNative(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindAnniversaryBlyNative(fact);
    internal bool CanOfferAnniversaryBly(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferAnniversaryBly(c, t, w);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryBly(global::CardGame.Core.PendingDecision p) => _resolve().ChooseAnniversaryBly(p);
    internal bool ContinueAnniversaryBly(long id) => _resolve().ContinueAnniversaryBly(id);
    internal bool DrainAnniversaryBly(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainAnniversaryBly(f);
    internal global::CardGame.Core.Card? FindAnniversaryBaiLingYunViewedCard(global::CardGame.Core.CharacterState actor, int id) => _resolve().FindAnniversaryBaiLingYunViewedCard(actor, id);
    internal void GainAndReturnAnniversaryBly(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GainAndReturnAnniversaryBly(f);
    internal bool IsAnniversaryBaiLingYunExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId) => _resolve().IsAnniversaryBaiLingYunExactCardQuery(actor, selectedPhysicalCardId);
    internal bool IsAnniversaryBaiLingYunSlashSource(long? programId, int source, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int provider, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physical, bool counts) => _resolve().IsAnniversaryBaiLingYunSlashSource(programId, source, target, card, kind, provider, from, physical, counts);
    internal bool IsAnniversaryBlyNative(global::CardGame.Core.CardUseFrame use) => _resolve().IsAnniversaryBlyNative(use);
    internal void IssueAnniversaryBlyUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueAnniversaryBlyUse(f);
    internal bool PayAnniversaryBlyProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int provider, global::CardGame.Core.CardLocation? from, global::CardGame.Core.CardMoveReason reason) => _resolve().PayAnniversaryBlyProcessing(cards, provider, from, reason);
    internal void PublishAnniversaryBly(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversaryBly(f);
    internal bool RetainAnniversaryBly(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainAnniversaryBly(f);
    internal bool ReturnAnniversaryBlyAttack() => _resolve().ReturnAnniversaryBlyAttack();
    internal bool ReturnAnniversaryBlyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryBlyMovement(f);
    internal void ReturnAnniversaryBlyNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnAnniversaryBlyNative(use);
    internal void SettleAnniversaryBlyNative(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleAnniversaryBlyNative(f);
    internal bool ValidAnniversaryBly(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidAnniversaryBly(f);
    internal void ValidateAnniversaryBlyNativeReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateAnniversaryBlyNativeReturn(use);
}
