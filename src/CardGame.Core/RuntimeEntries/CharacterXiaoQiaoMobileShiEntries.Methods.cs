#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoMobileShiEntries
{
    internal bool AdvanceShiXqChildren() => _resolve().AdvanceShiXqChildren();
    internal void AnswerShiXq(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiXq(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiXq(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginShiXq(input, operation);
    internal bool CanActivateShiXqHeyun(global::CardGame.Core.CharacterState owner) => _resolve().CanActivateShiXqHeyun(owner);
    internal bool CanOfferShiXqYinhui(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShiXqYinhui(c, t, context);
    internal global::CardGame.Core.PromptChoice ChooseShiXq(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiXq(p, f);
    internal void ContinueShiXq(long id) => _resolve().ContinueShiXq(id);
    internal bool DrainShiXq(long id) => _resolve().DrainShiXq(id);
    internal void FinishShiXq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiXq(f);
    internal bool KeepShiXqPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepShiXqPaid(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiXiaoQiaoPublicStates(global::CardGame.Core.CharacterState owner, string id) => _resolve().MobileShiXiaoQiaoPublicStates(owner, id);
    internal void PublishShiXq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiXq(f);
    internal void PublishShiXqSkillMutation(long id) => _resolve().PublishShiXqSkillMutation(id);
    internal bool ResumeShiXq(long id) => _resolve().ResumeShiXq(id);
    internal void ReturnShiXqDraw(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiXqDraw(input, outcome);
    internal bool ReturnShiXqMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiXqMovement(f);
    internal bool ShiXqBeforeDamageObserver(long id) => _resolve().ShiXqBeforeDamageObserver(id);
    internal bool ShiXqChildEdge(int i) => _resolve().ShiXqChildEdge(i);
    internal bool ShiXqChildSuffix(int i) => _resolve().ShiXqChildSuffix(i);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiXqChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiXqChoices(f);
    internal (int Seat, string Skill)[] ShiXqCopyOptions(int owner) => _resolve().ShiXqCopyOptions(owner);
    internal bool ShiXqDamageObserver(long id) => _resolve().ShiXqDamageObserver(id);
    internal bool ShiXqDyingObserver() => _resolve().ShiXqDyingObserver();
    internal bool ShiXqFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiXqFirstChild(f, child);
    internal global::CardGame.Core.ShiXqCopiedEvent[] ShiXqGrantLedger(int owner) => _resolve().ShiXqGrantLedger(owner);
    internal int[] ShiXqHeyunTargets(int owner) => _resolve().ShiXqHeyunTargets(owner);
    internal global::CardGame.Core.ShiXqCopiedEvent[] ShiXqLiveCopies(int owner) => _resolve().ShiXqLiveCopies(owner);
    internal string[] ShiXqLosableSkills(int owner) => _resolve().ShiXqLosableSkills(owner);
    internal bool ShiXqMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiXqMovementProof(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShiXqObserverRoot() => _resolve().ShiXqObserverRoot();
    internal bool ShiXqRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiXqRecoveryParent(f, recovery);
    internal string[] ShiXqVisibleOwnedSkills(int viewer, int seat) => _resolve().ShiXqVisibleOwnedSkills(viewer, seat);
    internal bool ValidShiXq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidShiXq(f);
}
