#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{
    internal bool HasFeiYiHezhong => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RevealSingletonAndArmRankEffects);

    internal bool FeiYiDirectionAvailable(int owner, bool greater) => !_coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiHezhongPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Greater == greater);
}
