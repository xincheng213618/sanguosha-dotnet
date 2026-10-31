#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoCaoOlMagicCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterCaoCaoOlMagicMoCaoCaoFuzaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MoCaoCaoFuzaiRuntime Get();
}

internal interface ICharacterCaoCaoOlMagicCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterCaoCaoOlMagicMoCaoCaoBachaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MoCaoCaoBachaoRuntime Get();
}
