#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuanJieMobileShiEntries
{
    internal bool AdvanceShiHjChildren() => _resolve().AdvanceShiHjChildren();
    internal void AnswerShiHj(PromptChoice choice) => _resolve().AnswerShiHj(choice);
    internal SkillProgramStepOutcome BeginShiHj(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginShiHj(input, op);
    internal bool CanOfferShiHj(ProgramTriggerLegalityContext q) => _resolve().CanOfferShiHj(q);
    internal PromptChoice ChooseShiHjAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShiHjAi(p, f);
    internal void ContinueShiHj(long id) => _resolve().ContinueShiHj(id);
    internal bool DrainShiHj(long id) => _resolve().DrainShiHj(id);
    internal void FinishShiHj(ProgramSkillFrame f) => _resolve().FinishShiHj(f);
    internal void GrantShiHjPaidTurnSkills(ProgramSkillFrame f) => _resolve().GrantShiHjPaidTurnSkills(f);
    internal bool KeepPaidShiHj(ProgramSkillFrame f) => _resolve().KeepPaidShiHj(f);
    internal void PublishShiHj(ProgramSkillFrame f) => _resolve().PublishShiHj(f);
    internal void RequireShiHj(ProgramSkillFrame f) => _resolve().RequireShiHj(f);
    internal bool ResumeShiHj(long id) => _resolve().ResumeShiHj(id);
    internal void ReturnShiHjDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnShiHjDraw(f, outcome);
    internal bool ReturnShiHjMovement(ProgramSkillFrame f) => _resolve().ReturnShiHjMovement(f);
    internal void SealShiHj(long id) => _resolve().SealShiHj(id);
    internal bool ShiHjChildEdge(int i) => _resolve().ShiHjChildEdge(i);
    internal bool ShiHjChildSuffix(int n) => _resolve().ShiHjChildSuffix(n);
    internal IReadOnlyList<PromptChoice> ShiHjChoices(ProgramSkillFrame f) => _resolve().ShiHjChoices(f);
    internal bool ShiHjDyingChild() => _resolve().ShiHjDyingChild();
    internal bool ShiHjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiHjFirstChild(f, child);
    internal bool ShiHjMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().ShiHjMovementProof(f, effect, p);
    internal ProgramSkillFrame? ShiHjObserverRoot() => _resolve().ShiHjObserverRoot();
    internal bool ShiHjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiHjRecoveryParent(f, recovery);
}
