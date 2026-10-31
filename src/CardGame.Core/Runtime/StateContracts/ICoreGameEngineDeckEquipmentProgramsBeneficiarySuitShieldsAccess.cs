#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess : IProgramRuntimeOperations
{
    ref readonly List<BeneficiarySuitShield> _beneficiarySuitShields { get; }
}
