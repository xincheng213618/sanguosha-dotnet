#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuCheErAnniversaryEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversaryHuCheErSlashForbiddenEvent> ActiveHuCheErSlashBans(int actor) => _resolve().ActiveHuCheErSlashBans(actor);
    internal bool AdvanceHuCheErChildren() => _resolve().AdvanceHuCheErChildren();
    internal global::CardGame.Core.PromptChoice AiHuCheEr(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHuCheEr(p, f);
    internal int AnniversaryHuCheErDrawBonus(global::CardGame.Core.CharacterState p) => _resolve().AnniversaryHuCheErDrawBonus(p);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> AnniversaryHuCheErHandLimitContributions(global::CardGame.Core.CharacterState p) => _resolve().AnniversaryHuCheErHandLimitContributions(p);
    internal void AnswerHuCheEr(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHuCheEr(choice);
    internal void AssertHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertHuCheEr(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHuCheEr(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginHuCheEr(input);
    internal bool CanOfferHuCheEr(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferHuCheEr(c, t, w);
    internal void ContinueHuCheEr(long id) => _resolve().ContinueHuCheEr(id);
    internal bool DrainHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHuCheEr(f);
    internal void FinishHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHuCheEr(f);
    internal bool HasAnniversaryHuCheErPrompt() => _resolve().HasAnniversaryHuCheErPrompt();
    internal bool HuCheErBeforeDamageObserver(long id) => _resolve().HuCheErBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HuCheErChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuCheErChoices(f);
    internal bool HuCheErDamageObserver(long id) => _resolve().HuCheErDamageObserver(id);
    internal bool HuCheErDyingObserver() => _resolve().HuCheErDyingObserver();
    internal bool HuCheErFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HuCheErFirstChild(f, child);
    internal global::CardGame.Core.CardUseFrame? HuCheErFirstWeapon(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().HuCheErFirstWeapon(owner, c);
    internal bool HuCheErGain(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().HuCheErGain(owner, c);
    internal bool HuCheErLogicalGainBatch(int owner, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HuCheErLogicalGainBatch(owner, batch);
    internal int[] HuCheErMaterials(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuCheErMaterials(f);
    internal bool HuCheErMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HuCheErMovement(f, e, p);
    internal bool HuCheErObserverEdge(int i) => _resolve().HuCheErObserverEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? HuCheErObserverRoot() => _resolve().HuCheErObserverRoot();
    internal void HuCheErOutgoingDistance(global::CardGame.Core.ProgramDistanceQuery q, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().HuCheErOutgoingDistance(q, terms);
    internal bool HuCheErParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuCheErParent(f);
    internal bool HuCheErResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().HuCheErResponseBan(q);
    internal bool HuCheErSlashBan(int actor, global::CardGame.Core.CardKind kind) => _resolve().HuCheErSlashBan(actor, kind);
    internal bool HuCheErUseBan(global::CardGame.Core.ProgramCardUseQuery q) => _resolve().HuCheErUseBan(q);
    internal int HuCheErZhong(global::CardGame.Core.CharacterState p) => _resolve().HuCheErZhong(p);
    internal bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId) => _resolve().IsAnniversaryHuCheErClaimedUseMaterial(useId, cardId);
    internal void PublishHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHuCheEr(f);
    internal global::CardGame.Core.SkillProgramStepOutcome RecordHuCheErGain(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RecordHuCheErGain(supplied);
    internal bool ResumeHuCheEr(long id) => _resolve().ResumeHuCheEr(id);
    internal bool RetainHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHuCheEr(f);
    internal bool ReturnHuCheErMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHuCheErMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SpendHuCheErFour(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SpendHuCheErFour(supplied);
    internal bool ValidHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHuCheEr(f);
    internal bool HasAnniversaryHuCheErRules { get => _resolve().HasAnniversaryHuCheErRules; }
}
