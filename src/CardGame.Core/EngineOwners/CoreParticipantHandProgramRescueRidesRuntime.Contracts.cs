#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreParticipantHandProgramRescueRidesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreParticipantHandProgramRescueRidesCoreRoundPricedPileAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreParticipantHandProgramRescueRidesCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue);
}

internal interface ICoreParticipantHandProgramRescueRidesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreParticipantHandProgramRescueRidesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
