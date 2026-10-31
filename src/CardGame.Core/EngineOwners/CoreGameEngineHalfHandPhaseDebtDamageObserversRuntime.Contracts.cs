#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHalfHandPhaseDebtDamageObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreHalfHandPhaseDebtDamageObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCorePublicPileEquipmentSequencesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreRecipientCategoryMarkObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreOffTurnUsedCardObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OrderedPrintedSkillLossStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreJoinedTrickDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool JoinedTrickDamageRewardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreRoundGainedEquipmentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RoundGainedEquipmentDrawStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCorePairedColorResponseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PairedColorDispositionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreHalfHandPhaseDebtObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPhaseFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ForeignSelectedCardSlashStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreMatchingRecastObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchingRecastStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OverflowTargetCancellationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHalfHandPhaseDebtDamageObserversCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ChainedStateBasicStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}
