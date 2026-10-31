#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaishiCiMobileRuntime
{
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 71100)]
    internal bool MobileTaishiCiFailedPhaseForbidsSlash(ProgramCardUseQuery query) => query.ActionType == CardActionType.Use && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(query.ActorSeat) && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[query.ActorSeat]).ProgramInstances.Any(instance => instance.SkillId == "mobile:taishi-ci-tianyi" && _coreProgramPersistentStateOperations.GetProgramBooleanState2(query.ActorSeat, instance.SkillId, instance.SkillInstanceId, "mobile-tianyi-failed-phase"));
}
