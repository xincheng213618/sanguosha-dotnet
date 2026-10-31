#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryHuaTuoSelection(Func<RuntimeEntries.CharacterHuaTuoBoundaryMobileEntries> getCharacterHuaTuoBoundaryMobileRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IBoundaryHuaTuoSelection
{
    public int PhaseLimit(int owner) => getCharacterHuaTuoBoundaryMobileRuntime().BoundaryHuaTuoCurrentPlay(owner)is { } phase ? 1 + getCharacterHuaTuoBoundaryMobileRuntime().BoundaryHuaTuoPayments(owner).Count(e => e.Payment.Phase == phase && e.Payment.Red) : 0;
    public bool MaySelect(int owner, int target) => !getCharacterHuaTuoBoundaryMobileRuntime().BoundaryHuaTuoPayments(owner).Any(e => e.Payment.Phase.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && e.Payment.Target == target);
}
