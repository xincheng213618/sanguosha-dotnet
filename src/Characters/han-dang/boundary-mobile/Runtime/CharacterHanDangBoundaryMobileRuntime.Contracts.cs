#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHanDangBoundaryMobileCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
