#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangJiOlEntries
{
    internal void AssertZhangJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertZhangJi(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhangJiOperation(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginZhangJiOperation(supplied, op);
    internal void BindZhangJiNativeUse(long useId) => _resolve().BindZhangJiNativeUse(useId);
    internal bool CanActivateZhangJi(int owner, global::CardGame.Core.SkillProgramActivation a) => _resolve().CanActivateZhangJi(owner, a);
    internal bool CanBeginZhangJiJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().CanBeginZhangJiJudgment(f, targetSeat, reason, bind, visibility);
    internal bool CanContinueZhangJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueZhangJi(f);
    internal void FinishZhangJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhangJi(f);
    internal bool HasZhangJiBeforeDamageObserver(long id) => _resolve().HasZhangJiBeforeDamageObserver(id);
    internal bool HasZhangJiDamageObserver(long id) => _resolve().HasZhangJiDamageObserver(id);
    internal bool IsZhangJiDying() => _resolve().IsZhangJiDying();
    internal bool IsZhangJiJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsZhangJiJudgment(j, f);
    internal bool IsZhangJiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsZhangJiMovement(f, e, p);
    internal void IssueZhangJiEquipment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueZhangJiEquipment(f);
    internal void IssueZhangJiOutcome(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueZhangJiOutcome(f);
    internal void PublishZhangJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangJi(f);
    internal void ResolveZhangJiChoice(global::CardGame.Core.PromptChoice supplied) => _resolve().ResolveZhangJiChoice(supplied);
    internal bool ResumeZhangJi(long id) => _resolve().ResumeZhangJi(id);
    internal void ResumeZhangJiCompletedNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ResumeZhangJiCompletedNativeUse(use);
    internal bool ReturnZhangJiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhangJiMovement(f);
    internal bool ReturnZhangJiNativeUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnZhangJiNativeUse(u);
    internal bool TryAdvanceZhangJiSubtree() => _resolve().TryAdvanceZhangJiSubtree();
    internal bool TryReturnZhangJiJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().TryReturnZhangJiJudgment(j);
    internal bool ValidZhangJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhangJi(f);
    internal bool ValidZhangJiNativeUse(global::CardGame.Core.CardUseFrame u) => _resolve().ValidZhangJiNativeUse(u);
    internal bool ZhangJiActivationTargetAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.SkillProgramActivation a) => _resolve().ZhangJiActivationTargetAllowed(owner, target, a);
    internal bool ZhangJiCardUseRidesOn(global::CardGame.Core.CardUseFrame u, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangJiCardUseRidesOn(u, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangJiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangJiChoices(f);
    internal bool ZhangJiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangJiFirstChild(f, child);
    internal int ZhangJiLueMingCount(int owner) => _resolve().ZhangJiLueMingCount(owner);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangJiObserverRoot() => _resolve().ZhangJiObserverRoot();
    internal global::CardGame.Core.SkillProgramEffect? ZhangJiPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangJiPausedEffect(f);
    internal global::CardGame.Core.CardConversionSource ZhangJiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangJiSource(f);
}
