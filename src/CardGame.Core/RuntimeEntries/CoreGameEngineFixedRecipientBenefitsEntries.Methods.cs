#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedRecipientBenefitsEntries
{
    internal bool CanRunFixedRecipientBenefit(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunFixedRecipientBenefit(candidate, trigger, context);
    internal bool ExactFixedRecipientDeath(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramDeathTriggerWindowFrame window) => _resolve().ExactFixedRecipientDeath(f, out window);
    internal bool ExactFixedRecipientEnding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactFixedRecipientEnding(f);
    internal global::CardGame.Core.SkillProgramStepOutcome IssueFixedRecipientBenefit(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().IssueFixedRecipientBenefit(supplied, effect);
    internal global::CardGame.Core.FixedRecipientBenefitIssuedEvent? OriginalFixedRecipient(int owner, string skill, string hash, string state) => _resolve().OriginalFixedRecipient(owner, skill, hash, state);
    internal global::System.Collections.Generic.IReadOnlyList<int>? PublishedFixedRecipientForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().PublishedFixedRecipientForAi(owner, effects, context);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectIssuedFixedRecipient(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectIssuedFixedRecipient(supplied, effect);
    internal bool ValidFixedRecipientProgress(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramFixedRecipientReceipt r) => _resolve().ValidFixedRecipientProgress(f, r);
    internal bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidFixedRecipientReceipt(f);
}
