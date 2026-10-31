#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime))]
internal sealed class CoreParticipantHandProgramRescueRidesCoreRoundPricedPileAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantHandProgramRescueRidesCoreRoundPricedPileAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _targetFactory = targetFactory;
    public bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsRoundPricedPileAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CoreParticipantHandProgramRescueRidesCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreParticipantHandProgramRescueRidesCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue) => this._targetFactory().ParticipantHandRescueObserverRide(frame, parent, rescue);
}
