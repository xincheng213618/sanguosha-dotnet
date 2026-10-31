#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanYinAnniversaryEntries
{
    internal bool AdvanceYuanYinChildren() => _resolve().AdvanceYuanYinChildren();
    internal void AnswerYuanYin(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerYuanYin(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYuanYinMoshou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginYuanYinMoshou(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYuanYinYunjiu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginYuanYinYunjiu(supplied);
    internal bool CanOfferYuanYin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferYuanYin(c, t, w);
    internal bool CanOfferYuanYinYunjiu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferYuanYinYunjiu(c, w);
    internal global::CardGame.Core.PromptChoice ChooseYuanYin(global::CardGame.Core.PendingDecision p) => _resolve().ChooseYuanYin(p);
    internal void CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(global::CardGame.Core.ProgramKillTriggerWindowFrame window, global::CardGame.Core.DeathFrame death) => _resolve().CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(window, death);
    internal bool ContinueYuanYin(long id) => _resolve().ContinueYuanYin(id);
    internal void ContinueYuanYinYunjiu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ContinueYuanYinYunjiu(supplied);
    internal bool DrainYuanYin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainYuanYin(f);
    internal void FinishYuanYin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishYuanYin(f);
    internal void PublishYuanYin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYuanYin(f);
    internal bool RetainYuanYin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainYuanYin(f);
    internal void ReturnYuanYinMoshouDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnYuanYinMoshouDraw(f, draw);
    internal bool ReturnYuanYinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYuanYinMovement(f);
    internal bool TryBeginAnniversaryYuanYinDeathBeforeCleanup(long deathId) => _resolve().TryBeginAnniversaryYuanYinDeathBeforeCleanup(deathId);
    internal bool ValidYuanYin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidYuanYin(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YuanYinChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YuanYinChoices(f);
    internal bool YuanYinDeathHasCards(int victim) => _resolve().YuanYinDeathHasCards(victim);
    internal bool YuanYinDying() => _resolve().YuanYinDying();
    internal bool YuanYinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YuanYinFirstChild(f, child);
    internal bool YuanYinIssuedDeathWindow(global::CardGame.Core.ProgramKillTriggerWindowFrame window, global::CardGame.Core.DeathFrame death) => _resolve().YuanYinIssuedDeathWindow(window, death);
    internal bool YuanYinMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().YuanYinMovement(f, e, p);
    internal bool YuanYinObserverEdge(int i) => _resolve().YuanYinObserverEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? YuanYinObserverRoot() => _resolve().YuanYinObserverRoot();
    internal bool YuanYinOriginalParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YuanYinOriginalParent(f);
    internal global::System.Collections.Generic.IEnumerable<int> YuanYinRecipients(int owner) => _resolve().YuanYinRecipients(owner);
    internal bool YuanYinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().YuanYinRecoveryParent(f, recovery);
    internal bool YuanYinTargetWindows(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().YuanYinTargetWindows(q);
}
