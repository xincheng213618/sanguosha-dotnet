#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreOwnTrickDrawAlcoholObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnTrickDrawAlcoholObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime))]
internal sealed class CoreOwnTrickDrawAlcoholObserversCoreLegacyDyingAlcoholReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnTrickDrawAlcoholObserversCoreLegacyDyingAlcoholReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLegacyDyingAlcoholReturnsRuntime> _targetFactory = targetFactory;
    public bool ExactLegacyDyingAlcoholReturnRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().ExactLegacyDyingAlcoholReturnRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime))]
internal sealed class CoreOwnTrickDrawAlcoholObserversCoreOwnTrickDrawTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnTrickDrawAlcoholObserversCoreOwnTrickDrawTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> _targetFactory = targetFactory;
    public bool OwnTrickDrawMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent) => this._targetFactory().OwnTrickDrawMatches(root, parent);
    public bool OwnTrickDrawFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OwnTrickDrawFirstChild(f, child);
}
