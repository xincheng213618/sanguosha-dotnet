#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnequalHandHpEndingEntries
{
    internal SkillProgramStepOutcome BeginUnequalHandHpEnding(ProgramSkillFrame supplied) => _resolve().BeginUnequalHandHpEnding(supplied);
    internal bool CanContinueUnequalHandHp(ProgramSkillFrame f) => _resolve().CanContinueUnequalHandHp(f);
    internal bool CanOfferUnequalHandHp(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int owner) => _resolve().CanOfferUnequalHandHp(trigger, context, owner);
    internal void CompleteUnequalHandHp(ProgramSkillFrame f) => _resolve().CompleteUnequalHandHp(f);
    internal bool DrainUnequalHandHpChildren(ProgramSkillFrame f) => _resolve().DrainUnequalHandHpChildren(f);
    internal void IssueUnequalHandHpMovement(ProgramSkillFrame supplied, string step, int subject, int? cardId, CardLocation to, UnequalHandHpStage stage) => _resolve().IssueUnequalHandHpMovement(supplied, step, subject, cardId, to, stage);
    internal void PublishUnequalHandHpChoice(ProgramSkillFrame supplied) => _resolve().PublishUnequalHandHpChoice(supplied);
    internal void RequireUnequalHandHpReceipt(ProgramSkillFrame f) => _resolve().RequireUnequalHandHpReceipt(f);
    internal void ResolveUnequalHandHpChoice(PromptChoice choice) => _resolve().ResolveUnequalHandHpChoice(choice);
    internal bool ResumeUnequalHandHp(long id) => _resolve().ResumeUnequalHandHp(id);
    internal bool ReturnUnequalHandHpMovement(ProgramSkillFrame f) => _resolve().ReturnUnequalHandHpMovement(f);
    internal PromptChoice SelectAiUnequalHandHp(PendingDecision prompt, ProgramSkillFrame f) => _resolve().SelectAiUnequalHandHp(prompt, f);
    internal CardConversionSource TargetedTrickHandHpSource(ProgramSkillFrame f) => _resolve().TargetedTrickHandHpSource(f);
    internal IReadOnlyList<PromptChoice> UnequalHandHpChoices(ProgramSkillFrame f) => _resolve().UnequalHandHpChoices(f);
    internal int[] UnequalHandHpTargets(int owner) => _resolve().UnequalHandHpTargets(owner);
}
