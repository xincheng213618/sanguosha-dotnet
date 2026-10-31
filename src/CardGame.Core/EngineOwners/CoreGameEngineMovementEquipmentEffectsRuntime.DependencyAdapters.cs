#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreMovementEquipmentEffectsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default) => this._targetFactory().TryQueueRecoveryReplacement(parentFrameId, sourceSeat, targetSeat, amount, completion, happyQiaoRecoverToOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreMovementEquipmentEffectsCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyWeiYanBone? HappyBoneAt(int cardId, global::CardGame.Core.CardLocation location, int sequence) => this._targetFactory().HappyBoneAt(cardId, location, sequence);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreMovementEquipmentEffectsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveAllCards(global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveAllCards(from, to, reason, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime))]
internal sealed class CoreMovementEquipmentEffectsCoreNativeRecoveryFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCoreNativeRecoveryFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> _targetFactory = targetFactory;
    public long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount) => this._targetFactory().BeginRecovery(parentFrameId, sourceSeat, targetSeat, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreMovementEquipmentEffectsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreMovementEquipmentEffectsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool UsesFormalWoodenOx { get => this._targetFactory().UsesFormalWoodenOx; }
    public bool UsesFormalSilverLion { get => this._targetFactory().UsesFormalSilverLion; }
}
