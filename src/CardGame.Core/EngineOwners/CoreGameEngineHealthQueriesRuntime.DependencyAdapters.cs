#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed class CoreHealthQueriesCharacterFeiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesCharacterFeiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _targetFactory = targetFactory;
    public int NativeExtraEffectRoundOrdinal(long id) => this._targetFactory().NativeExtraEffectRoundOrdinal(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreHealthQueriesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed class CoreHealthQueriesCoreCardDamageMaximumHpReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesCoreCardDamageMaximumHpReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _targetFactory = targetFactory;
    public int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CalculateCardDamageLayer(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreHealthQueriesCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyQiaoHealth(int seat) => this._targetFactory().HasHappyQiaoHealth(seat);
    public bool TryBeginHappyQiaoHealthChoice(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int amount, global::CardGame.Core.HappyQiaoHealthReturnKind returnKind, string occurrenceKey, int? sourceSeat = default, long? attackResolutionId = default) => this._targetFactory().TryBeginHappyQiaoHealthChoice(parentFrameId, targetSeat, kind, amount, returnKind, occurrenceKey, sourceSeat, attackResolutionId);
    public void RequireHappyQiaoHealthChoice(global::CardGame.Core.HappyQiaoHealthChoiceFrame f) => this._targetFactory().RequireHappyQiaoHealthChoice(f);
    public bool TryBeginHappyQiaoQuery(global::CardGame.Core.ResolutionFrame supplied, int subjectSeat, string queryKey) => this._targetFactory().TryBeginHappyQiaoQuery(supplied, subjectSeat, queryKey);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime))]
internal sealed class CoreHealthQueriesCoreLastDamageSourceReciprocityObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthQueriesCoreLastDamageSourceReciprocityObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? LastDamageSourceOriginalAttack(long id) => this._targetFactory().LastDamageSourceOriginalAttack(id);
}
