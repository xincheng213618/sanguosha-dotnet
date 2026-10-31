#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageCardClaimNativeRulesRuntime
{
    DamageTriggerWindowFrame? global::CardGame.Core.IProgramDamageCardClaimNativeRules.CurrentDamageWindow => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    ProgramDamageCardOwner global::CardGame.Core.IProgramDamageCardClaimNativeRules.SelectAttackOwner(DamageTriggerWindowFrame window)
    {
        global::CardGame.Core.GameEngine.IDamageAttempt attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        return new(attack.ResolutionId, attack is global::CardGame.Core.NativeOwners.CardAttackHandle);
    }

    long? global::CardGame.Core.IProgramDamageCardClaimNativeRules.ActiveGroupOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.OwnerFrameId;

    void global::CardGame.Core.IProgramDamageCardClaimNativeRules.CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards, long sequenceBefore, CardMoveReason reason) => _coreExtraEffectMaterialClaimsOperations.CaptureExtraEffectMaterialClaims(frame, _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window), cards, sequenceBefore, reason);
}
