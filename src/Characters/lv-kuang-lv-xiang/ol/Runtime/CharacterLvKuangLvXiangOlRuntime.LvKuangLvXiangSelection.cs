#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    bool global::CardGame.Core.ILvKuangLvXiangSelectionQueries.Allows(int ownerSeat, int targetSeat, SkillProgramActivation activation) => LvKuangLvXiangActivationTargetAllowed(_coreMatchParticipantsPlayersAccess._players[targetSeat], activation, ownerSeat);
}
