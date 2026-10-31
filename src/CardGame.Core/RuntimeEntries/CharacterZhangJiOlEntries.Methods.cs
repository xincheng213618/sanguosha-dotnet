#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangJiOlEntries
{
    internal SkillProgramStepOutcome BeginZhangJiOperation(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginZhangJiOperation(supplied, op);
    internal void BindZhangJiNativeUse(long useId) => _resolve().BindZhangJiNativeUse(useId);
    internal bool CanActivateZhangJi(int owner, SkillProgramActivation a) => _resolve().CanActivateZhangJi(owner, a);
    internal bool CanBeginZhangJiJudgment(ProgramSkillFrame f, int targetSeat, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().CanBeginZhangJiJudgment(f, targetSeat, reason, bind, visibility);
    internal bool CanContinueZhangJi(ProgramSkillFrame f) => _resolve().CanContinueZhangJi(f);
    internal void FinishZhangJi(ProgramSkillFrame f) => _resolve().FinishZhangJi(f);
    internal bool IsZhangJiJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsZhangJiJudgment(j, f);
    internal bool IsZhangJiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsZhangJiMovement(f, e, p);
    internal void IssueZhangJiEquipment(ProgramSkillFrame f) => _resolve().IssueZhangJiEquipment(f);
    internal void IssueZhangJiOutcome(ProgramSkillFrame f) => _resolve().IssueZhangJiOutcome(f);
    internal void PublishZhangJi(ProgramSkillFrame f) => _resolve().PublishZhangJi(f);
    internal void ResolveZhangJiChoice(PromptChoice supplied) => _resolve().ResolveZhangJiChoice(supplied);
    internal bool ResumeZhangJi(long id) => _resolve().ResumeZhangJi(id);
    internal void ResumeZhangJiCompletedNativeUse(CardUseFrame use) => _resolve().ResumeZhangJiCompletedNativeUse(use);
    internal bool ReturnZhangJiMovement(ProgramSkillFrame f) => _resolve().ReturnZhangJiMovement(f);
    internal bool ReturnZhangJiNativeUse(CardUseFrame u) => _resolve().ReturnZhangJiNativeUse(u);
    internal bool TryAdvanceZhangJiSubtree() => _resolve().TryAdvanceZhangJiSubtree();
    internal bool TryReturnZhangJiJudgment(JudgmentFrame j) => _resolve().TryReturnZhangJiJudgment(j);
    internal bool ValidZhangJi(ProgramSkillFrame f) => _resolve().ValidZhangJi(f);
    internal bool ValidZhangJiNativeUse(CardUseFrame u) => _resolve().ValidZhangJiNativeUse(u);
    internal bool ZhangJiActivationTargetAllowed(CharacterState owner, CharacterState target, SkillProgramActivation a) => _resolve().ZhangJiActivationTargetAllowed(owner, target, a);
    internal bool ZhangJiCardUseRidesOn(CardUseFrame u, ProgramSkillFrame f) => _resolve().ZhangJiCardUseRidesOn(u, f);
    internal IReadOnlyList<PromptChoice> ZhangJiChoices(ProgramSkillFrame f) => _resolve().ZhangJiChoices(f);
    internal bool ZhangJiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangJiFirstChild(f, child);
    internal int ZhangJiLueMingCount(int owner) => _resolve().ZhangJiLueMingCount(owner);
    internal ProgramSkillFrame? ZhangJiObserverRoot() => _resolve().ZhangJiObserverRoot();
    internal SkillProgramEffect? ZhangJiPausedEffect(ProgramSkillFrame f) => _resolve().ZhangJiPausedEffect(f);
    internal CardConversionSource ZhangJiSource(ProgramSkillFrame f) => _resolve().ZhangJiSource(f);
}
