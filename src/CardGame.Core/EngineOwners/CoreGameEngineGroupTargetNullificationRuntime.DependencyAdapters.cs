#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreGroupTargetNullificationCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreGroupTargetNullificationCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool IsLeChengyanEffectUse(long id) => this._targetFactory().IsLeChengyanEffectUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreGroupTargetNullificationCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag) => this._targetFactory().HasCurrentCardEnhancement(frameId, flag);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime))]
internal sealed class CoreGroupTargetNullificationCoreNativeHarvestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreNativeHarvestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _targetFactory = targetFactory;
    public void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().BeginHarvestSelection(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreGroupTargetNullificationCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime))]
internal sealed class CoreGroupTargetNullificationCoreNativeGroupCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreNativeGroupCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime> _targetFactory = targetFactory;
    public void FinishGroupRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().FinishGroupRecovery(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreGroupTargetNullificationCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime))]
internal sealed class CoreGroupTargetNullificationCoreNativeGroupAttackAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreNativeGroupAttackOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> _targetFactory = targetFactory;
    public void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().BeginGroupAttackResponse(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreGroupTargetNullificationCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreGroupTargetNullificationCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat) => this._targetFactory().BuildNullificationCandidateSeats(startAfterSeat);
    public void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ContinueNullificationWindow(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreGroupTargetNullificationCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreGroupTargetNullificationCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsIssuedCardUnrespondable(long frameId) => this._targetFactory().IsIssuedCardUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime))]
internal sealed class CoreGroupTargetNullificationCoreUnnullifiableOrdinaryTricksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreUnnullifiableOrdinaryTricksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> _targetFactory = targetFactory;
    public bool IsOriginalOrdinaryTrickCannotNullify(long useId) => this._targetFactory().IsOriginalOrdinaryTrickCannotNullify(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreGroupTargetNullificationCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreGroupTargetNullificationCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public bool IsYingboUnrespondable(long resolutionId) => this._targetFactory().IsYingboUnrespondable(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime))]
internal sealed class CoreGroupTargetNullificationCoreOrderedSelectedActorDuelsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGroupTargetNullificationCoreOrderedSelectedActorDuelsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> _targetFactory = targetFactory;
    public bool IsOrderedSelectedActorDuelCannotNullify(long useId) => this._targetFactory().IsOrderedSelectedActorDuelCannotNullify(useId);
}
