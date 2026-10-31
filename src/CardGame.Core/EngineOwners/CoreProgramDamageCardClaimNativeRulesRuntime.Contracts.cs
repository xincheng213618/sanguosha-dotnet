#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDamageCardClaimNativeRulesCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
}

internal interface ICoreProgramDamageCardClaimNativeRulesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreProgramDamageCardClaimNativeRulesCoreExtraEffectMaterialClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureExtraEffectMaterialClaims(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, long before, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreProgramDamageCardClaimNativeRulesCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}
