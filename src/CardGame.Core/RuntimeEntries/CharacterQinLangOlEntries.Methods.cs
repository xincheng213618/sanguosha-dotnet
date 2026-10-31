#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQinLangOlEntries
{
    internal bool AdvanceQinLangChildren() => _resolve().AdvanceQinLangChildren();
    internal global::CardGame.Core.PromptChoice AiQinLang(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiQinLang(p, f);
    internal void AnswerQinLang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerQinLang(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginQinLang(global::CardGame.Core.ProgramSkillFrame input, bool ending) => _resolve().BeginQinLang(input, ending);
    internal void BeginQinLangXianyingUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.QinLangXianyingUseOrigin origin, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().BeginQinLangXianyingUse(f, origin, targets);
    internal bool CollectQinLang(global::CardGame.Core.SkillProgramTrigger t) => _resolve().CollectQinLang(t);
    internal bool ContinuePaidQinLang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidQinLang(f);
    internal void ContinueQinLang(long id) => _resolve().ContinueQinLang(id);
    internal bool ContinueQinLangXianyingSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueQinLangXianyingSimpleUse(id, continuation);
    internal bool DrainQinLang(long id) => _resolve().DrainQinLang(id);
    internal void ExpireQinLangXianyingTurnEndWine() => _resolve().ExpireQinLangXianyingTurnEndWine();
    internal void FinishQinLang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishQinLang(f);
    internal bool FinishQinLangXianyingAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishQinLangXianyingAttack(attack, out paused);
    internal bool HasIssuedQinLangXianyingUse(long id) => _resolve().HasIssuedQinLangXianyingUse(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeQinLangXianyingAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeQinLangXianyingAfterTurnEndedItems(parent, ordinary);
    internal bool IsIssuedQinLangXianyingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedQinLangXianyingCandidate(c, context);
    internal bool IsQinLangDiscardMovement(global::CardGame.Core.QinLangXianyingReceipt r, global::CardGame.Core.CardMovementRecord m) => _resolve().IsQinLangDiscardMovement(r, m);
    internal bool IsQinLangRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsQinLangRecoveryParent(f, recovery);
    internal bool IsQinLangXianyingSlashQuery(global::CardGame.Core.CharacterState actor) => _resolve().IsQinLangXianyingSlashQuery(actor);
    internal bool IsQinLangXianyingUse(long id) => _resolve().IsQinLangXianyingUse(id);
    internal bool MatchesQinLangXianying(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.QinLangXianyingUseOrigin o) => _resolve().MatchesQinLangXianying(f, o);
    internal bool MatchesQinLangXianyingUseAction(long id, global::CardGame.Core.CardActionContext a, int cardId) => _resolve().MatchesQinLangXianyingUseAction(id, a, cardId);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.QinLangXianyingGrantedEvent> OutstandingQinLangGrants(int turn) => _resolve().OutstandingQinLangGrants(turn);
    internal void PayQinLang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayQinLang(f);
    internal void PublishQinLang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishQinLang(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QinLangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QinLangChoices(f);
    internal bool QinLangCountUsed(int owner, int round, int count) => _resolve().QinLangCountUsed(owner, round, count);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.QinLangXianyingCard> QinLangDiscardable(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QinLangDiscardable(f);
    internal bool QinLangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().QinLangFirstChild(f, child);
    internal bool QinLangGrantConsumed(long id) => _resolve().QinLangGrantConsumed(id);
    internal global::CardGame.Core.ProgramSkillFrame? QinLangObserverRoot() => _resolve().QinLangObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.QinLangUseOption> QinLangUseOptions(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.QinLangXianyingGrantedEvent grant) => _resolve().QinLangUseOptions(f, grant);
    internal bool QinLangXianyingSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().QinLangXianyingSlashQuotaBeforeLimit(query);
    internal bool ResumeQinLang(long id) => _resolve().ResumeQinLang(id);
    internal bool ReturnQinLangAttack() => _resolve().ReturnQinLangAttack();
    internal void ReturnQinLangUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnQinLangUse(use);
    internal bool ValidQinLang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidQinLang(f);
    internal void ValidateQinLangUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateQinLangUse(use);
    internal bool HasQinLangCapability { get => _resolve().HasQinLangCapability; }
}
