#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeSkillContextQueriesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetHandLimit(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeSkillContextQueriesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPlayCardHistory { get; }

    bool? PreviousPlayCardIsBasicFor(int actorSeat);
}

internal interface ICoreNativeSkillContextQueriesCorePassiveDamageCapsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? GetIncomingFixedDamageCap(global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICoreNativeSkillContextQueriesCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyDictionary<string, bool>? GetPublicProgramBooleanStates(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyDictionary<string, int>? GetProgramPublicCounters(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeSkillContextQueriesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeSkillContextQueriesCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesClassicBattleRules { get; }
}

internal interface ICoreNativeSkillContextQueriesCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeSkillContextQueriesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeSkillContextQueriesCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeSkillContextQueriesCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActuallyUsableHandCard(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeSkillContextQueriesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeSkillContextQueriesCorePublicDamagePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPredictionHpPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.PlayerSkillContext context, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICoreNativeSkillContextQueriesCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }

    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}
