#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTeamActivityProgramsEntries
{
    internal bool CanOfferTeamActivity(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferTeamActivity(c, trigger, context);
    internal SkillProgramStepOutcome ExecuteTeamActivity(ProgramSkillFrame input, SkillProgramEffect effect) => _resolve().ExecuteTeamActivity(input, effect);
    internal bool OtherRevealedPublicTeamAlly(CharacterState owner, CharacterState target) => _resolve().OtherRevealedPublicTeamAlly(owner, target);
    internal bool PublicTeamEnemy(int owner, CharacterState target) => _resolve().PublicTeamEnemy(owner, target);
}
