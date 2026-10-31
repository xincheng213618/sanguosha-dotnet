#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardActionContestsEntries
{
    internal bool CanOfferCardActionContest(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCardActionContest(owner, trigger, context);
    internal bool HasCardActionPindianContinuation() => _resolve().HasCardActionPindianContinuation();
    internal void ReplaceAllProgramSlashTargets(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().ReplaceAllProgramSlashTargets(frame, bind);
    internal global::CardGame.Core.SkillProgramStepOutcome StartProgramCardActionPindian(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().StartProgramCardActionPindian(frame, effect);
}
