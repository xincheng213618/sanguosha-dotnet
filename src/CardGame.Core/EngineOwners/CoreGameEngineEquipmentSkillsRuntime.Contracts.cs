#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEquipmentSkillsCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyWeiYanBone(int id);
}

internal interface ICoreEquipmentSkillsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreEquipmentSkillsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreEquipmentSkillsCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDynamicEquipmentSkillGrants(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}
