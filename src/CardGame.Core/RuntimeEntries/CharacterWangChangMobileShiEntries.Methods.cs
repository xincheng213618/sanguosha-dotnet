#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangChangMobileShiEntries
{
    internal bool AdvanceShiKaijiChildren() => _resolve().AdvanceShiKaijiChildren();
    internal void AnswerShiWangChang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiWangChang(choice);
    internal bool CanOfferShiWangChang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShiWangChang(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseShiWangChangAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiWangChangAi(decision, f);
    internal void ContinueShiKaiji(long id) => _resolve().ContinueShiKaiji(id);
    internal bool KeepPaidShiWangChang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidShiWangChang(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiWangChangPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiWangChangPublicStates(owner, skillId);
    internal void PublishShiWangChang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiWangChang(f);
    internal void RequireShiWangChang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShiWangChang(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShiWangChang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveShiWangChang(supplied, op);
    internal bool ResumeShiKaiji(long id) => _resolve().ResumeShiKaiji(id);
    internal void ReturnShiKaijiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiKaijiDraw(f, outcome);
    internal bool ReturnShiKaijiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiKaijiMovement(f);
    internal bool ShiKaijiDying() => _resolve().ShiKaijiDying();
    internal bool ShiKaijiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiKaijiFirstChild(f, child);
    internal int ShiKaijiMaximum() => _resolve().ShiKaijiMaximum();
    internal global::CardGame.Core.ProgramSkillFrame? ShiKaijiObserverRoot() => _resolve().ShiKaijiObserverRoot();
    internal bool ShiKaijiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiKaijiRecoveryParent(f, recovery);
    internal global::CardGame.Core.CardUseFrame? ShiShepanUse(global::CardGame.Core.ProgramSkillWindowContext c, int owner) => _resolve().ShiShepanUse(c, owner);
    internal global::CardGame.Core.PromptChoice ShiWangChangChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? target = default) => _resolve().ShiWangChangChoice(f, branch, label, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiWangChangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiWangChangChoices(f);
}
