#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaiLingYunAnniversaryEntries
{
    internal bool AdvanceAnniversaryBlyChildren() => _resolve().AdvanceAnniversaryBlyChildren();
    internal IReadOnlyList<PromptChoice> AnniversaryBlyChoices(ProgramSkillFrame f) => _resolve().AnniversaryBlyChoices(f);
    internal bool AnniversaryBlyDyingObserver() => _resolve().AnniversaryBlyDyingObserver();
    internal bool AnniversaryBlyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnniversaryBlyFirstChild(f, child);
    internal bool AnniversaryBlyMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().AnniversaryBlyMovementProof(f, effect, pending);
    internal ProgramSkillFrame? AnniversaryBlyObserverRoot() => _resolve().AnniversaryBlyObserverRoot();
    internal bool AnniversaryBlyProcessingAllowance(IReadOnlyList<Card> cards) => _resolve().AnniversaryBlyProcessingAllowance(cards);
    internal ProgramSkillFrame? AnniversaryBlyQuery(int actor, int? card = default) => _resolve().AnniversaryBlyQuery(actor, card);
    internal bool AnniversaryBlyQueryableCard(ProgramSkillFrame f, int id) => _resolve().AnniversaryBlyQueryableCard(f, id);
    internal bool AnniversaryBlyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().AnniversaryBlyRecoveryParent(f, recovery);
    internal IReadOnlyList<LegalAction> AnniversaryBlyUseActions(ProgramSkillFrame f) => _resolve().AnniversaryBlyUseActions(f);
    internal bool AnniversaryBlyUsePaid(ProgramSkillFrame f) => _resolve().AnniversaryBlyUsePaid(f);
    internal void AnswerAnniversaryBly(PromptChoice choice) => _resolve().AnswerAnniversaryBly(choice);
    internal IReadOnlyList<Card> AppendAnniversaryBaiLingYunPlayableCards(CharacterState actor, IReadOnlyList<Card> cards) => _resolve().AppendAnniversaryBaiLingYunPlayableCards(actor, cards);
    internal SkillProgramStepOutcome BeginAnniversaryBly(ProgramSkillFrame supplied) => _resolve().BeginAnniversaryBly(supplied);
    internal void BindAnniversaryBlyNative(CardUseDeclaredEvent fact) => _resolve().BindAnniversaryBlyNative(fact);
    internal bool CanOfferAnniversaryBly(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferAnniversaryBly(c, t, w);
    internal PromptChoice ChooseAnniversaryBly(PendingDecision p) => _resolve().ChooseAnniversaryBly(p);
    internal bool ContinueAnniversaryBly(long id) => _resolve().ContinueAnniversaryBly(id);
    internal bool DrainAnniversaryBly(ProgramSkillFrame f) => _resolve().DrainAnniversaryBly(f);
    internal Card? FindAnniversaryBaiLingYunViewedCard(CharacterState actor, int id) => _resolve().FindAnniversaryBaiLingYunViewedCard(actor, id);
    internal void GainAndReturnAnniversaryBly(ProgramSkillFrame f) => _resolve().GainAndReturnAnniversaryBly(f);
    internal bool IsAnniversaryBaiLingYunExactCardQuery(CharacterState actor, int? selectedPhysicalCardId) => _resolve().IsAnniversaryBaiLingYunExactCardQuery(actor, selectedPhysicalCardId);
    internal bool IsAnniversaryBaiLingYunSlashSource(long? programId, int source, int target, Card card, CardKind kind, int provider, CardLocation from, IReadOnlyList<Card>? physical, bool counts) => _resolve().IsAnniversaryBaiLingYunSlashSource(programId, source, target, card, kind, provider, from, physical, counts);
    internal bool IsAnniversaryBlyNative(CardUseFrame use) => _resolve().IsAnniversaryBlyNative(use);
    internal void IssueAnniversaryBlyUse(ProgramSkillFrame f) => _resolve().IssueAnniversaryBlyUse(f);
    internal bool PayAnniversaryBlyProcessing(IReadOnlyList<Card> cards, int provider, CardLocation? from, CardMoveReason reason) => _resolve().PayAnniversaryBlyProcessing(cards, provider, from, reason);
    internal void PublishAnniversaryBly(ProgramSkillFrame f) => _resolve().PublishAnniversaryBly(f);
    internal bool RetainAnniversaryBly(ProgramSkillFrame f) => _resolve().RetainAnniversaryBly(f);
    internal bool ReturnAnniversaryBlyAttack() => _resolve().ReturnAnniversaryBlyAttack();
    internal bool ReturnAnniversaryBlyMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryBlyMovement(f);
    internal void ReturnAnniversaryBlyNative(CardUseFrame use) => _resolve().ReturnAnniversaryBlyNative(use);
    internal void SettleAnniversaryBlyNative(ProgramSkillFrame f) => _resolve().SettleAnniversaryBlyNative(f);
    internal bool ValidAnniversaryBly(ProgramSkillFrame f) => _resolve().ValidAnniversaryBly(f);
    internal void ValidateAnniversaryBlyNativeReturn(CardUseFrame use) => _resolve().ValidateAnniversaryBlyNativeReturn(use);
}
