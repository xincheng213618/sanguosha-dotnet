#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTurnDrawDebtsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreTurnDrawDebtsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreTurnDrawDebtsCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome SelectProgramOwnedCards(global::CardGame.Core.ProgramSkillFrame frame, int cardOwnerSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? expression, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> suits);
}

internal interface ICoreTurnDrawDebtsCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualTurnLegacyVirtualUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreTurnDrawDebtsCoreBoundCardMovementContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat);
}

internal interface ICoreTurnDrawDebtsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreTurnDrawDebtsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
    void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default);
}

internal interface ICoreTurnDrawDebtsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat);
}

internal interface ICoreTurnDrawDebtsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreTurnDrawDebtsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreTurnDrawDebtsCorePublicFactionCountsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetPublicLivingFactionCount();
}
