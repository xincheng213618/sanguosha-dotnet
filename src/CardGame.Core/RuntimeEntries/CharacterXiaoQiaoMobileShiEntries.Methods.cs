#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoMobileShiEntries
{
    internal bool AdvanceShiXqChildren() => _resolve().AdvanceShiXqChildren();
    internal void AnswerShiXq(PromptChoice selected) => _resolve().AnswerShiXq(selected);
    internal SkillProgramStepOutcome BeginShiXq(ProgramSkillFrame input, SkillProgramEffectOp operation) => _resolve().BeginShiXq(input, operation);
    internal bool CanActivateShiXqHeyun(CharacterState owner) => _resolve().CanActivateShiXqHeyun(owner);
    internal bool CanOfferShiXqYinhui(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferShiXqYinhui(c, t, context);
    internal PromptChoice ChooseShiXq(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShiXq(p, f);
    internal void ContinueShiXq(long id) => _resolve().ContinueShiXq(id);
    internal bool DrainShiXq(long id) => _resolve().DrainShiXq(id);
    internal void FinishShiXq(ProgramSkillFrame f) => _resolve().FinishShiXq(f);
    internal bool KeepShiXqPaid(ProgramSkillFrame f) => _resolve().KeepShiXqPaid(f);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiXiaoQiaoPublicStates(CharacterState owner, string id) => _resolve().MobileShiXiaoQiaoPublicStates(owner, id);
    internal void PublishShiXq(ProgramSkillFrame f) => _resolve().PublishShiXq(f);
    internal void PublishShiXqSkillMutation(long id) => _resolve().PublishShiXqSkillMutation(id);
    internal bool ResumeShiXq(long id) => _resolve().ResumeShiXq(id);
    internal void ReturnShiXqDraw(ProgramSkillFrame input, NativeDrawOutcome outcome) => _resolve().ReturnShiXqDraw(input, outcome);
    internal bool ReturnShiXqMovement(ProgramSkillFrame f) => _resolve().ReturnShiXqMovement(f);
    internal bool ShiXqChildEdge(int i) => _resolve().ShiXqChildEdge(i);
    internal bool ShiXqChildSuffix(int i) => _resolve().ShiXqChildSuffix(i);
    internal IReadOnlyList<PromptChoice> ShiXqChoices(ProgramSkillFrame f) => _resolve().ShiXqChoices(f);
    internal (int Seat, string Skill)[] ShiXqCopyOptions(int owner) => _resolve().ShiXqCopyOptions(owner);
    internal bool ShiXqDyingObserver() => _resolve().ShiXqDyingObserver();
    internal bool ShiXqFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiXqFirstChild(f, child);
    internal ShiXqCopiedEvent[] ShiXqGrantLedger(int owner) => _resolve().ShiXqGrantLedger(owner);
    internal int[] ShiXqHeyunTargets(int owner) => _resolve().ShiXqHeyunTargets(owner);
    internal ShiXqCopiedEvent[] ShiXqLiveCopies(int owner) => _resolve().ShiXqLiveCopies(owner);
    internal string[] ShiXqLosableSkills(int owner) => _resolve().ShiXqLosableSkills(owner);
    internal bool ShiXqMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().ShiXqMovementProof(f, e, p);
    internal ProgramSkillFrame? ShiXqObserverRoot() => _resolve().ShiXqObserverRoot();
    internal bool ShiXqRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiXqRecoveryParent(f, recovery);
    internal string[] ShiXqVisibleOwnedSkills(int viewer, int seat) => _resolve().ShiXqVisibleOwnedSkills(viewer, seat);
    internal bool ValidShiXq(ProgramSkillFrame f) => _resolve().ValidShiXq(f);
}
