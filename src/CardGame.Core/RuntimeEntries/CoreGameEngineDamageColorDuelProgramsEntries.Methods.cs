#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageColorDuelProgramsEntries
{
    internal SkillProgramStepOutcome BeginDamageAppearanceDraw(ProgramSkillFrame input) => _resolve().BeginDamageAppearanceDraw(input);
    internal SkillProgramStepOutcome BeginSelectedActorDuel(ProgramSkillFrame input) => _resolve().BeginSelectedActorDuel(input);
    internal bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat) => _resolve().CanIssueSelectedActorDuel(ownerSeat, actorSeat);
    internal bool IsSelectedActorDuelUse(long id) => _resolve().IsSelectedActorDuelUse(id);
    internal void ReturnSelectedActorDuel(CardUseFrame use) => _resolve().ReturnSelectedActorDuel(use);
}
