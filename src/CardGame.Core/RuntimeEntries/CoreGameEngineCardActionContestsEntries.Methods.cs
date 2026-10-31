#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardActionContestsEntries
{
    internal bool CanOfferCardActionContest(CharacterState owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferCardActionContest(owner, trigger, context);
    internal void ReplaceAllProgramSlashTargets(ProgramSkillFrame frame, string bind) => _resolve().ReplaceAllProgramSlashTargets(frame, bind);
    internal SkillProgramStepOutcome StartProgramCardActionPindian(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().StartProgramCardActionPindian(frame, effect);
}
