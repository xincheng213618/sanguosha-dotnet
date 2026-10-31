#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess : IProgramRuntimeOperations
{
    ref readonly List<BeneficiarySuitShield> _beneficiarySuitShields { get; }

    ref long _beneficiarySuitShieldSequence { get; }
}
