#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.BeneficiarySuitShield> _beneficiarySuitShields { get; }

    ref long _beneficiarySuitShieldSequence { get; }
}
