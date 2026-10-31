#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed class CoreTargetedTrickHandHpAiCoreTargetedOrdinaryTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTargetedTrickHandHpAiCoreTargetedOrdinaryTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _targetFactory = targetFactory;
    public bool TryGetTargetedOrdinaryTrickUse(global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().TryGetTargetedOrdinaryTrickUse(context, out use, out window);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TargetedTrickChoices(long frameId, global::CardGame.Core.CardUseFrame use, bool removeTargets = false) => this._targetFactory().TargetedTrickChoices(frameId, use, removeTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreTargetedTrickHandHpAiCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTargetedTrickHandHpAiCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
