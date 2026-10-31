#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQinLangOlEntries
{
    internal bool AdvanceQinLangChildren() => _resolve().AdvanceQinLangChildren();
    internal PromptChoice AiQinLang(PendingDecision p, ProgramSkillFrame f) => _resolve().AiQinLang(p, f);
    internal void AnswerQinLang(PromptChoice choice) => _resolve().AnswerQinLang(choice);
    internal SkillProgramStepOutcome BeginQinLang(ProgramSkillFrame input, bool ending) => _resolve().BeginQinLang(input, ending);
    internal void BeginQinLangXianyingUse(ProgramSkillFrame f, QinLangXianyingUseOrigin origin, IReadOnlyList<int> targets) => _resolve().BeginQinLangXianyingUse(f, origin, targets);
    internal bool CollectQinLang(SkillProgramTrigger t) => _resolve().CollectQinLang(t);
    internal bool ContinuePaidQinLang(ProgramSkillFrame f) => _resolve().ContinuePaidQinLang(f);
    internal void ContinueQinLang(long id) => _resolve().ContinueQinLang(id);
    internal bool ContinueQinLangXianyingSimpleUse(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueQinLangXianyingSimpleUse(id, continuation);
    internal bool DrainQinLang(long id) => _resolve().DrainQinLang(id);
    internal void ExpireQinLangXianyingTurnEndWine() => _resolve().ExpireQinLangXianyingTurnEndWine();
    internal void FinishQinLang(ProgramSkillFrame f) => _resolve().FinishQinLang(f);
    internal bool FinishQinLangXianyingAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishQinLangXianyingAttack(attack, out paused);
    internal bool HasIssuedQinLangXianyingUse(long id) => _resolve().HasIssuedQinLangXianyingUse(id);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeQinLangXianyingAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeQinLangXianyingAfterTurnEndedItems(parent, ordinary);
    internal bool IsIssuedQinLangXianyingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedQinLangXianyingCandidate(c, context);
    internal bool IsQinLangDiscardMovement(QinLangXianyingReceipt r, CardMovementRecord m) => _resolve().IsQinLangDiscardMovement(r, m);
    internal bool IsQinLangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsQinLangRecoveryParent(f, recovery);
    internal bool IsQinLangXianyingSlashQuery(CharacterState actor) => _resolve().IsQinLangXianyingSlashQuery(actor);
    internal bool IsQinLangXianyingUse(long id) => _resolve().IsQinLangXianyingUse(id);
    internal bool MatchesQinLangXianying(ProgramSkillFrame f, QinLangXianyingUseOrigin o) => _resolve().MatchesQinLangXianying(f, o);
    internal bool MatchesQinLangXianyingUseAction(long id, CardActionContext a, int cardId) => _resolve().MatchesQinLangXianyingUseAction(id, a, cardId);
    internal IEnumerable<QinLangXianyingGrantedEvent> OutstandingQinLangGrants(int turn) => _resolve().OutstandingQinLangGrants(turn);
    internal void PayQinLang(ProgramSkillFrame f) => _resolve().PayQinLang(f);
    internal void PublishQinLang(ProgramSkillFrame f) => _resolve().PublishQinLang(f);
    internal IReadOnlyList<PromptChoice> QinLangChoices(ProgramSkillFrame f) => _resolve().QinLangChoices(f);
    internal bool QinLangCountUsed(int owner, int round, int count) => _resolve().QinLangCountUsed(owner, round, count);
    internal IReadOnlyList<QinLangXianyingCard> QinLangDiscardable(ProgramSkillFrame f) => _resolve().QinLangDiscardable(f);
    internal bool QinLangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().QinLangFirstChild(f, child);
    internal bool QinLangGrantConsumed(long id) => _resolve().QinLangGrantConsumed(id);
    internal ProgramSkillFrame? QinLangObserverRoot() => _resolve().QinLangObserverRoot();
    internal IReadOnlyList<GameEngine.QinLangUseOption> QinLangUseOptions(ProgramSkillFrame f, QinLangXianyingGrantedEvent grant) => _resolve().QinLangUseOptions(f, grant);
    internal bool QinLangXianyingSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().QinLangXianyingSlashQuotaBeforeLimit(query);
    internal bool ResumeQinLang(long id) => _resolve().ResumeQinLang(id);
    internal bool ReturnQinLangAttack() => _resolve().ReturnQinLangAttack();
    internal void ReturnQinLangUse(CardUseFrame use) => _resolve().ReturnQinLangUse(use);
    internal bool ValidQinLang(ProgramSkillFrame f) => _resolve().ValidQinLang(f);
    internal void ValidateQinLangUse(CardUseFrame use) => _resolve().ValidateQinLangUse(use);
    internal bool HasQinLangCapability { get => _resolve().HasQinLangCapability; }
}
