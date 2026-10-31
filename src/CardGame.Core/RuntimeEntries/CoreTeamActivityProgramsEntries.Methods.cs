#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTeamActivityProgramsEntries
{
    internal bool CanOfferTeamActivity(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferTeamActivity(c, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteTeamActivity(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteTeamActivity(input, effect);
    internal bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target) => _resolve().OtherRevealedPublicTeamAlly(owner, target);
    internal bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target) => _resolve().PublicTeamEnemy(owner, target);
}
