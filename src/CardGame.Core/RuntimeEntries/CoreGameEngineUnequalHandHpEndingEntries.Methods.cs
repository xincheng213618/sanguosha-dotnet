#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnequalHandHpEndingEntries
{
    internal void AssertRegisteredUnequalHandHpReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredUnequalHandHpReceipt(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginUnequalHandHpEnding(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginUnequalHandHpEnding(supplied);
    internal bool CanContinueUnequalHandHp(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueUnequalHandHp(f);
    internal bool CanOfferUnequalHandHp(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int owner) => _resolve().CanOfferUnequalHandHp(trigger, context, owner);
    internal void CompleteUnequalHandHp(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteUnequalHandHp(f);
    internal bool DrainUnequalHandHpChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainUnequalHandHpChildren(f);
    internal bool IsUnequalHandHpMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsUnequalHandHpMovement(f, effect, pending);
    internal void IssueUnequalHandHpMovement(global::CardGame.Core.ProgramSkillFrame supplied, string step, int subject, int? cardId, global::CardGame.Core.CardLocation to, global::CardGame.Core.UnequalHandHpStage stage) => _resolve().IssueUnequalHandHpMovement(supplied, step, subject, cardId, to, stage);
    internal void PublishUnequalHandHpChoice(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PublishUnequalHandHpChoice(supplied);
    internal void RequireUnequalHandHpReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireUnequalHandHpReceipt(f);
    internal void ResolveUnequalHandHpChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveUnequalHandHpChoice(choice);
    internal bool ResumeUnequalHandHp(long id) => _resolve().ResumeUnequalHandHp(id);
    internal bool ReturnUnequalHandHpMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnUnequalHandHpMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiUnequalHandHp(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiUnequalHandHp(prompt, f);
    internal global::CardGame.Core.CardConversionSource TargetedTrickHandHpSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TargetedTrickHandHpSource(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> UnequalHandHpChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().UnequalHandHpChoices(f);
    internal int[] UnequalHandHpTargets(int owner) => _resolve().UnequalHandHpTargets(owner);
}
