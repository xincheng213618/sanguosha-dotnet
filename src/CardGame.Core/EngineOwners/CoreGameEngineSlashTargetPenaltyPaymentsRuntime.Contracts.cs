#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSlashTargetPenaltyPaymentsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
