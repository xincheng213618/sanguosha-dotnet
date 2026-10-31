#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvKuangLvXiangSelectionQueries(Func<RuntimeEntries.CharacterLvKuangLvXiangOlEntries> getCharacterLvKuangLvXiangOlRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : ILvKuangLvXiangSelectionQueries
{
    public bool Allows(int ownerSeat, int targetSeat, SkillProgramActivation activation) => getCharacterLvKuangLvXiangOlRuntime().LvKuangLvXiangActivationTargetAllowed(matchParticipantsState.ReferenceToPlayers[targetSeat], activation, ownerSeat);
}
