#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeckEquipmentProgramsState : Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess, Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess
{
    private long _beneficiarySuitShieldSequence = default !;
    internal ref long ReferenceToBeneficiarySuitShieldSequence => ref _beneficiarySuitShieldSequence;

    private List<BeneficiarySuitShield> _beneficiarySuitShields = [];
    internal ref readonly List<BeneficiarySuitShield> ReferenceToBeneficiarySuitShields => ref _beneficiarySuitShields;

    ref readonly List<BeneficiarySuitShield> Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShields => ref _beneficiarySuitShields;

    ref long Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShieldSequence => ref _beneficiarySuitShieldSequence;

    ref readonly List<BeneficiarySuitShield> Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess._beneficiarySuitShields => ref _beneficiarySuitShields;
}
