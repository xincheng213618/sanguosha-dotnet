namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 71100)]
    private bool MobileTaishiCiFailedPhaseForbidsSlash(ProgramCardUseQuery query) =>
        query.ActionType == CardActionType.Use && IsSlashCard(query.Kind) && IsValidPlayerSeat(query.ActorSeat) &&
        GetSkillBindingShard(_players[query.ActorSeat]).ProgramInstances.Any(instance => instance.SkillId == "mobile:taishi-ci-tianyi" &&
            GetProgramBooleanState(query.ActorSeat, instance.SkillId, instance.SkillInstanceId, "mobile-tianyi-failed-phase"));
}
