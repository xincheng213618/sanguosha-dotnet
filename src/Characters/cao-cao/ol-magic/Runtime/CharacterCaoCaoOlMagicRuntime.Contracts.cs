#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoCaoOlMagicCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterCaoCaoOlMagicMoCaoCaoFuzaiOperations : IProgramRuntimeOperations
{
    MoCaoCaoFuzaiRuntime Get();
}

internal interface ICharacterCaoCaoOlMagicCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterCaoCaoOlMagicMoCaoCaoBachaoOperations : IProgramRuntimeOperations
{
    MoCaoCaoBachaoRuntime Get();
}
