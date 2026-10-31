#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvKuangLvXiangSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLvKuangLvXiangOlEntries> getCharacterLvKuangLvXiangOlRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : ILvKuangLvXiangSelectionQueries
{
    public bool Allows(int ownerSeat, int targetSeat, SkillProgramActivation activation) => getCharacterLvKuangLvXiangOlRuntime().LvKuangLvXiangActivationTargetAllowed(matchParticipantsState.ReferenceToPlayers[targetSeat], activation, ownerSeat);
}
