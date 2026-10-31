#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreProgramDamageCardClaimNativeRulesCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDamageCardClaimNativeRulesCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame) => this._targetFactory().GetDamageTriggerAttack(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreProgramDamageCardClaimNativeRulesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDamageCardClaimNativeRulesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime))]
internal sealed class CoreProgramDamageCardClaimNativeRulesCoreExtraEffectMaterialClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDamageCardClaimNativeRulesCoreExtraEffectMaterialClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> _targetFactory = targetFactory;
    public void CaptureExtraEffectMaterialClaims(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, long before, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().CaptureExtraEffectMaterialClaims(producer, attack, cards, before, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreProgramDamageCardClaimNativeRulesCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDamageCardClaimNativeRulesCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}
