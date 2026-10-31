#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSchoolsRuntime
{
    internal bool IsShanheSchoolSkillQualified(int ownerSeat, string skillId) => ShanheSchoolEffects.SchoolId(skillId)is not { } schoolId || IsShanheSchoolComplete(ownerSeat, schoolId);
    internal bool IsShanheSchoolComplete(int ownerSeat, int schoolId)
    {
        if (ownerSeat != 0 || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not { } setup || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        // A live campaign remains the authority after a purchase. The immutable
        // entry recipe is used only for encounters without a persistent market.
        return _shanheMarketOperations._shanheMarket is { } market ? market.GetSchoolProgress().Any(progress => progress.School.Id == schoolId && progress.IsComplete && ShanheSchoolEffects.SkillId(progress.School)is not null) : setup.ActiveSchoolIds.Contains(schoolId);
    }

    internal bool HasShanheSchoolEffect(int ownerSeat, int schoolId) => IsShanheSchoolComplete(ownerSeat, schoolId) && _coreNationalSkillsOperations.EnabledContentSkillIds(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Any(id => ShanheSchoolEffects.SchoolId(id) == schoolId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 62031)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62031)]
    internal bool AllowsShanheSchoolTrigger(ProgramTriggerLegalityContext gate) => IsShanheSchoolSkillQualified(gate.OwnerSeat, gate.SkillId);
}
