#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanYinAnniversaryEntries
{
    internal bool AdvanceYuanYinChildren() => _resolve().AdvanceYuanYinChildren();
    internal void AnswerYuanYin(PromptChoice selected) => _resolve().AnswerYuanYin(selected);
    internal SkillProgramStepOutcome BeginYuanYinMoshou(ProgramSkillFrame supplied) => _resolve().BeginYuanYinMoshou(supplied);
    internal SkillProgramStepOutcome BeginYuanYinYunjiu(ProgramSkillFrame supplied) => _resolve().BeginYuanYinYunjiu(supplied);
    internal bool CanOfferYuanYin(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferYuanYin(c, t, w);
    internal bool CanOfferYuanYinYunjiu(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().CanOfferYuanYinYunjiu(c, w);
    internal PromptChoice ChooseYuanYin(PendingDecision p) => _resolve().ChooseYuanYin(p);
    internal void CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(ProgramKillTriggerWindowFrame window, DeathFrame death) => _resolve().CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(window, death);
    internal bool ContinueYuanYin(long id) => _resolve().ContinueYuanYin(id);
    internal void ContinueYuanYinYunjiu(ProgramSkillFrame supplied) => _resolve().ContinueYuanYinYunjiu(supplied);
    internal bool DrainYuanYin(ProgramSkillFrame f) => _resolve().DrainYuanYin(f);
    internal void FinishYuanYin(ProgramSkillFrame f) => _resolve().FinishYuanYin(f);
    internal void PublishYuanYin(ProgramSkillFrame f) => _resolve().PublishYuanYin(f);
    internal bool RetainYuanYin(ProgramSkillFrame f) => _resolve().RetainYuanYin(f);
    internal void ReturnYuanYinMoshouDraw(ProgramSkillFrame f, NativeDrawOutcome draw) => _resolve().ReturnYuanYinMoshouDraw(f, draw);
    internal bool ReturnYuanYinMovement(ProgramSkillFrame f) => _resolve().ReturnYuanYinMovement(f);
    internal bool TryBeginAnniversaryYuanYinDeathBeforeCleanup(long deathId) => _resolve().TryBeginAnniversaryYuanYinDeathBeforeCleanup(deathId);
    internal bool ValidYuanYin(ProgramSkillFrame f) => _resolve().ValidYuanYin(f);
    internal IReadOnlyList<PromptChoice> YuanYinChoices(ProgramSkillFrame f) => _resolve().YuanYinChoices(f);
    internal bool YuanYinDeathHasCards(int victim) => _resolve().YuanYinDeathHasCards(victim);
    internal bool YuanYinDying() => _resolve().YuanYinDying();
    internal bool YuanYinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().YuanYinFirstChild(f, child);
    internal bool YuanYinIssuedDeathWindow(ProgramKillTriggerWindowFrame window, DeathFrame death) => _resolve().YuanYinIssuedDeathWindow(window, death);
    internal bool YuanYinMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().YuanYinMovement(f, e, p);
    internal bool YuanYinObserverEdge(int i) => _resolve().YuanYinObserverEdge(i);
    internal ProgramSkillFrame? YuanYinObserverRoot() => _resolve().YuanYinObserverRoot();
    internal bool YuanYinOriginalParent(ProgramSkillFrame f) => _resolve().YuanYinOriginalParent(f);
    internal IEnumerable<int> YuanYinRecipients(int owner) => _resolve().YuanYinRecipients(owner);
    internal bool YuanYinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().YuanYinRecoveryParent(f, recovery);
    internal bool YuanYinTargetWindows(ProgramTargetCardWindowQuery q) => _resolve().YuanYinTargetWindows(q);
}
