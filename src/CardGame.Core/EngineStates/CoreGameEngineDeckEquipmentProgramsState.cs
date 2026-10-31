#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeckEquipmentProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess
{
    private long _beneficiarySuitShieldSequence = default !;
    internal ref long ReferenceToBeneficiarySuitShieldSequence => ref _beneficiarySuitShieldSequence;

    private global::System.Collections.Generic.List<global::CardGame.Core.BeneficiarySuitShield> _beneficiarySuitShields = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.BeneficiarySuitShield> ReferenceToBeneficiarySuitShields => ref _beneficiarySuitShields;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.BeneficiarySuitShield> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShields => ref _beneficiarySuitShields;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShieldSequence => ref _beneficiarySuitShieldSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.BeneficiarySuitShield> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess._beneficiarySuitShields => ref _beneficiarySuitShields;
}
