#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CorePhaseHandSeizureReceiptsCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CorePhaseHandSeizureReceiptsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed class CorePhaseHandSeizureReceiptsCorePrivateOfferReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCorePrivateOfferReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetPausedPrivateOfferEffect(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CorePhaseHandSeizureReceiptsCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().IsEphemeralCardDestroyedMaterial(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed class CorePhaseHandSeizureReceiptsCorePhaseHandSeizureAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCorePhaseHandSeizureOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _targetFactory = targetFactory;
    public bool PhaseHandSeizureTerminalState { get => this._targetFactory().PhaseHandSeizureTerminalState; }

    public global::CardGame.Core.CardConversionSource PhaseHandSeizureSource(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().PhaseHandSeizureSource(frame);
    public global::CardGame.Core.PhaseHandSeizureIssuedEvent RequirePhaseHandDebtParent(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().RequirePhaseHandDebtParent(frame);
    public global::CardGame.Core.Card[] PhaseHandDebtCards(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().PhaseHandDebtCards(frame);
    public bool CanRunIssuedPhaseHandDebt(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunIssuedPhaseHandDebt(candidate, context);
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures() => this._targetFactory().UnsettledPhaseHandSeizures();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PhaseHandDebtChoices(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().PhaseHandDebtChoices(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CorePhaseHandSeizureReceiptsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhaseHandSeizureReceiptsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}
