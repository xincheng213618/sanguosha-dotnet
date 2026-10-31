#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuCheErAnniversaryEntries
{
    internal IEnumerable<AnniversaryHuCheErSlashForbiddenEvent> ActiveHuCheErSlashBans(int actor) => _resolve().ActiveHuCheErSlashBans(actor);
    internal bool AdvanceHuCheErChildren() => _resolve().AdvanceHuCheErChildren();
    internal PromptChoice AiHuCheEr(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHuCheEr(p, f);
    internal int AnniversaryHuCheErDrawBonus(CharacterState p) => _resolve().AnniversaryHuCheErDrawBonus(p);
    internal IEnumerable<RuleQueryContribution> AnniversaryHuCheErHandLimitContributions(CharacterState p) => _resolve().AnniversaryHuCheErHandLimitContributions(p);
    internal void AnswerHuCheEr(PromptChoice choice) => _resolve().AnswerHuCheEr(choice);
    internal SkillProgramStepOutcome BeginHuCheEr(ProgramSkillFrame input) => _resolve().BeginHuCheEr(input);
    internal bool CanOfferHuCheEr(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferHuCheEr(c, t, w);
    internal void ContinueHuCheEr(long id) => _resolve().ContinueHuCheEr(id);
    internal bool DrainHuCheEr(ProgramSkillFrame f) => _resolve().DrainHuCheEr(f);
    internal void FinishHuCheEr(ProgramSkillFrame f) => _resolve().FinishHuCheEr(f);
    internal IReadOnlyList<PromptChoice> HuCheErChoices(ProgramSkillFrame f) => _resolve().HuCheErChoices(f);
    internal bool HuCheErDyingObserver() => _resolve().HuCheErDyingObserver();
    internal bool HuCheErFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HuCheErFirstChild(f, child);
    internal CardUseFrame? HuCheErFirstWeapon(int owner, ProgramSkillWindowContext c) => _resolve().HuCheErFirstWeapon(owner, c);
    internal bool HuCheErGain(int owner, ProgramSkillWindowContext c) => _resolve().HuCheErGain(owner, c);
    internal bool HuCheErLogicalGainBatch(int owner, CardMovementBatchContext batch) => _resolve().HuCheErLogicalGainBatch(owner, batch);
    internal int[] HuCheErMaterials(ProgramSkillFrame f) => _resolve().HuCheErMaterials(f);
    internal bool HuCheErMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().HuCheErMovement(f, e, p);
    internal bool HuCheErObserverEdge(int i) => _resolve().HuCheErObserverEdge(i);
    internal ProgramSkillFrame? HuCheErObserverRoot() => _resolve().HuCheErObserverRoot();
    internal void HuCheErOutgoingDistance(ProgramDistanceQuery q, List<RuleQueryBaseTerm> terms) => _resolve().HuCheErOutgoingDistance(q, terms);
    internal bool HuCheErParent(ProgramSkillFrame f) => _resolve().HuCheErParent(f);
    internal bool HuCheErResponseBan(ProgramNativeResponseQuery q) => _resolve().HuCheErResponseBan(q);
    internal bool HuCheErSlashBan(int actor, CardKind kind) => _resolve().HuCheErSlashBan(actor, kind);
    internal bool HuCheErUseBan(ProgramCardUseQuery q) => _resolve().HuCheErUseBan(q);
    internal int HuCheErZhong(CharacterState p) => _resolve().HuCheErZhong(p);
    internal bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId) => _resolve().IsAnniversaryHuCheErClaimedUseMaterial(useId, cardId);
    internal void PublishHuCheEr(ProgramSkillFrame f) => _resolve().PublishHuCheEr(f);
    internal SkillProgramStepOutcome RecordHuCheErGain(ProgramSkillFrame supplied) => _resolve().RecordHuCheErGain(supplied);
    internal bool ResumeHuCheEr(long id) => _resolve().ResumeHuCheEr(id);
    internal bool RetainHuCheEr(ProgramSkillFrame f) => _resolve().RetainHuCheEr(f);
    internal bool ReturnHuCheErMovement(ProgramSkillFrame f) => _resolve().ReturnHuCheErMovement(f);
    internal SkillProgramStepOutcome SpendHuCheErFour(ProgramSkillFrame supplied) => _resolve().SpendHuCheErFour(supplied);
    internal bool ValidHuCheEr(ProgramSkillFrame f) => _resolve().ValidHuCheEr(f);
    internal bool HasAnniversaryHuCheErRules { get => _resolve().HasAnniversaryHuCheErRules; }
}
