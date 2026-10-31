#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedRecipientBenefitsEntries
{
    internal bool CanRunFixedRecipientBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunFixedRecipientBenefit(candidate, trigger, context);
    internal bool ExactFixedRecipientDeath(ProgramSkillFrame f, scoped out ProgramDeathTriggerWindowFrame window) => _resolve().ExactFixedRecipientDeath(f, out window);
    internal bool ExactFixedRecipientEnding(ProgramSkillFrame f) => _resolve().ExactFixedRecipientEnding(f);
    internal SkillProgramStepOutcome IssueFixedRecipientBenefit(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().IssueFixedRecipientBenefit(supplied, effect);
    internal FixedRecipientBenefitIssuedEvent? OriginalFixedRecipient(int owner, string skill, string hash, string state) => _resolve().OriginalFixedRecipient(owner, skill, hash, state);
    internal IReadOnlyList<int>? PublishedFixedRecipientForAi(CharacterState owner, IReadOnlyList<SkillProgramEffect> effects, ProgramSkillWindowContext? context) => _resolve().PublishedFixedRecipientForAi(owner, effects, context);
    internal SkillProgramStepOutcome SelectIssuedFixedRecipient(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().SelectIssuedFixedRecipient(supplied, effect);
    internal bool ValidFixedRecipientProgress(ProgramSkillFrame f, ProgramFixedRecipientReceipt r) => _resolve().ValidFixedRecipientProgress(f, r);
    internal bool ValidFixedRecipientReceipt(ProgramSkillFrame f) => _resolve().ValidFixedRecipientReceipt(f);
}
