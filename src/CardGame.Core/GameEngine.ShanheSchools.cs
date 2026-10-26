using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId) =>
        ShanheSchoolEffects.SchoolId(skillId) is not { } schoolId ||
        IsShanheSchoolComplete(ownerSeat, schoolId);

    private bool IsShanheSchoolComplete(int ownerSeat, int schoolId)
    {
        if (ownerSeat != 0 || _modeDefinition.Id != ShanheBattleSetup.BattleModeId ||
            _options.ShanheBattle is not { } setup || !_players[ownerSeat].IsAlive) return false;
        // A live campaign remains the authority after a purchase. The immutable
        // entry recipe is used only for encounters without a persistent market.
        return _shanheMarket is { } market
            ? market.GetSchoolProgress().Any(progress => progress.School.Id == schoolId && progress.IsComplete &&
                ShanheSchoolEffects.SkillId(progress.School) is not null)
            : setup.ActiveSchoolIds.Contains(schoolId);
    }

    private bool HasShanheSchoolEffect(int ownerSeat, int schoolId) =>
        IsShanheSchoolComplete(ownerSeat, schoolId) &&
        EnabledContentSkillIds(_players[ownerSeat]).Any(id => ShanheSchoolEffects.SchoolId(id) == schoolId);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 62031)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62031)]
    private bool AllowsShanheSchoolTrigger(ProgramTriggerLegalityContext gate) =>
        IsShanheSchoolSkillQualified(gate.OwnerSeat, gate.SkillId);
}
