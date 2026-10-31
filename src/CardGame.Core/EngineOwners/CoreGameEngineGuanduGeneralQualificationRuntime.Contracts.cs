#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGuanduGeneralQualificationCoreGuanduGeneralNativeCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind? GuanduLastDamageCard(int seat);
    bool GuanduMoushiQualifies(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext c);
}

internal interface ICoreGuanduGeneralQualificationCoreGuanduGeneralsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduGeneralContextAllowed(string skillId, global::CardGame.Core.SkillProgramEffectOp operation);
    bool GuanduShicaiReady(int ownerSeat, string instance);
    global::CardGame.Core.Card[] GuanduDiscardable(int chooser, int owner);
    bool GuanduGeneralGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    bool GuanduTurnDiscarded(int seat);
    global::CardGame.Core.Card[] GuanduOwnedCards(int seat, bool judgment = false);
    bool GuanduSkillEnabled(global::CardGame.Core.CharacterState owner, string skill);
}

internal interface ICoreGuanduGeneralQualificationCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame);
}

internal interface ICoreGuanduGeneralQualificationCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreGuanduGeneralQualificationCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreGuanduGeneralQualificationCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreGuanduGeneralQualificationCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreGuanduGeneralQualificationCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GuanduEquipmentSkillGrantId(global::CardGame.Core.Card card);
    global::CardGame.Core.Card[] GuanduChariotDiscardable(int actor, int target);
}

internal interface ICoreGuanduGeneralQualificationCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGuanduGeneralQualificationCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduActivity { get; }
}

internal interface ICoreGuanduGeneralQualificationKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreGuanduGeneralQualificationCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreGuanduGeneralQualificationCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreGuanduGeneralQualificationCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreGuanduGeneralQualificationCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreGuanduGeneralQualificationCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGuanduGeneralQualificationCoreGuanduGeneralBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GuanduDiscardFactEvent[] GuanduClaimableDiscards(int target);
}

internal interface ICoreGuanduGeneralQualificationCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}
