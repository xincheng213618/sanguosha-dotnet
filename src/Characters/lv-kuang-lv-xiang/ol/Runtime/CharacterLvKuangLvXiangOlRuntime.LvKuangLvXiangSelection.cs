#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    bool ILvKuangLvXiangSelectionQueries.Allows(int ownerSeat, int targetSeat, SkillProgramActivation activation) => LvKuangLvXiangActivationTargetAllowed(_coreMatchParticipantsPlayersAccess._players[targetSeat], activation, ownerSeat);
}
