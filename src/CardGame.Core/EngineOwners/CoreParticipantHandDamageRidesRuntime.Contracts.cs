#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreParticipantHandDamageRidesCoreParticipantHandProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect ParticipantHandPaused(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreParticipantHandDamageRidesCharacterZhaoYunOlGod2016Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidHpScaledViewAsAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreParticipantHandDamageRidesCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
    global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false);
}

internal interface ICoreParticipantHandDamageRidesCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreParticipantHandDamageRidesCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreParticipantHandDamageRidesCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRequestedDeckBasicRescueCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreParticipantHandDamageRidesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreParticipantHandDamageRidesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreParticipantHandDamageRidesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreParticipantHandDamageRidesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreParticipantHandDamageRidesCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}
