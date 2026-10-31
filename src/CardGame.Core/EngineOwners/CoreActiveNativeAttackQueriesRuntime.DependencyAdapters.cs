#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreActiveNativeAttackQueriesCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get => this._targetFactory().MouXiaoQiaoOl; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed class CoreActiveNativeAttackQueriesCharacterGuoSiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesCharacterGuoSiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _targetFactory = targetFactory;
    public bool IsSuspendedGuoSiCompletedParent(global::CardGame.Core.CardUseFrame original) => this._targetFactory().IsSuspendedGuoSiCompletedParent(original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed class CoreActiveNativeAttackQueriesCharacterLiuChengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesCharacterLiuChengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default) => this._targetFactory().IsMouLiuChengSuspendedCompletedAttack(original, requiredWindowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreActiveNativeAttackQueriesCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreActiveNativeAttackQueriesCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsDiouSuspendedRescueAttack(global::CardGame.Core.ResolutionFrame owner) => this._targetFactory().IsDiouSuspendedRescueAttack(owner);
    public bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default) => this._targetFactory().IsDiouSuspendedCompletedAttack(original, requiredWindowId);
}
