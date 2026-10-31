#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangChangMobileShiEntries
{
    internal bool AdvanceShiKaijiChildren() => _resolve().AdvanceShiKaijiChildren();
    internal void AnswerShiWangChang(PromptChoice choice) => _resolve().AnswerShiWangChang(choice);
    internal bool CanOfferShiWangChang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShiWangChang(candidate, trigger, context);
    internal PromptChoice ChooseShiWangChangAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiWangChangAi(decision, f);
    internal void ContinueShiKaiji(long id) => _resolve().ContinueShiKaiji(id);
    internal bool KeepPaidShiWangChang(ProgramSkillFrame f) => _resolve().KeepPaidShiWangChang(f);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiWangChangPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiWangChangPublicStates(owner, skillId);
    internal void PublishShiWangChang(ProgramSkillFrame f) => _resolve().PublishShiWangChang(f);
    internal void RequireShiWangChang(ProgramSkillFrame f) => _resolve().RequireShiWangChang(f);
    internal SkillProgramStepOutcome ResolveShiWangChang(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveShiWangChang(supplied, op);
    internal bool ResumeShiKaiji(long id) => _resolve().ResumeShiKaiji(id);
    internal void ReturnShiKaijiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnShiKaijiDraw(f, outcome);
    internal bool ReturnShiKaijiMovement(ProgramSkillFrame f) => _resolve().ReturnShiKaijiMovement(f);
    internal bool ShiKaijiDying() => _resolve().ShiKaijiDying();
    internal bool ShiKaijiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiKaijiFirstChild(f, child);
    internal int ShiKaijiMaximum() => _resolve().ShiKaijiMaximum();
    internal ProgramSkillFrame? ShiKaijiObserverRoot() => _resolve().ShiKaijiObserverRoot();
    internal bool ShiKaijiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiKaijiRecoveryParent(f, recovery);
    internal CardUseFrame? ShiShepanUse(ProgramSkillWindowContext c, int owner) => _resolve().ShiShepanUse(c, owner);
    internal PromptChoice ShiWangChangChoice(ProgramSkillFrame f, string branch, string label, int? target = default) => _resolve().ShiWangChangChoice(f, branch, label, target);
    internal IReadOnlyList<PromptChoice> ShiWangChangChoices(ProgramSkillFrame f) => _resolve().ShiWangChangChoices(f);
}
