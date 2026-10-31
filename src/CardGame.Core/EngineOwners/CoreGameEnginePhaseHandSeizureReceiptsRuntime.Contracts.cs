#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePhaseHandSeizureReceiptsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICorePhaseHandSeizureReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICorePhaseHandSeizureReceiptsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePhaseHandSeizureReceiptsCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePhaseHandSeizureReceiptsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICorePhaseHandSeizureReceiptsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePhaseHandSeizureReceiptsCorePhaseHandSeizureOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PhaseHandSeizureTerminalState { get; }

    global::CardGame.Core.CardConversionSource PhaseHandSeizureSource(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.PhaseHandSeizureIssuedEvent RequirePhaseHandDebtParent(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.Card[] PhaseHandDebtCards(global::CardGame.Core.ProgramSkillFrame frame);
    bool CanRunIssuedPhaseHandDebt(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PhaseHandDebtChoices(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICorePhaseHandSeizureReceiptsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICorePhaseHandSeizureReceiptsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
