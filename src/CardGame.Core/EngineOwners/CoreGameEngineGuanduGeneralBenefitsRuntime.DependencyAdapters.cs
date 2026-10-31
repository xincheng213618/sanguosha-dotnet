#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreGuanduActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreGuanduActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsGuanduCommander(int seat) => this._targetFactory().IsGuanduCommander(seat);
    public bool GuanduAllies(int first, int second) => this._targetFactory().GuanduAllies(first, second);
    public bool IsGuanduActivity { get => this._targetFactory().IsGuanduActivity; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreGuanduGeneralChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreGuanduGeneralChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> _targetFactory = targetFactory;
    public void PublishGuanduGeneralChoice(global::CardGame.Core.ProgramSkillFrame f, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => this._targetFactory().PublishGuanduGeneralChoice(f, chooser, choices);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduOwnCardChoices(global::CardGame.Core.ProgramSkillFrame f, int seat, string option, bool discard = false, bool allowSkip = true, bool nonEquipment = false, bool handOnly = false) => this._targetFactory().GuanduOwnCardChoices(f, seat, option, discard, allowSkip, nonEquipment, handOnly);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduForeignGainChoices(global::CardGame.Core.ProgramSkillFrame f, int seat) => this._targetFactory().GuanduForeignGainChoices(f, seat);
    public global::CardGame.Core.PromptChoice GuanduChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => this._targetFactory().GuanduChoice(f, option, text, cards, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreGuanduGeneralsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreGuanduGeneralsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime> _targetFactory = targetFactory;
    public bool GuanduShicaiReady(int ownerSeat, string instance) => this._targetFactory().GuanduShicaiReady(ownerSeat, instance);
    public void FinishGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, bool completed = true) => this._targetFactory().FinishGuanduGeneral(f, completed);
    public global::CardGame.Core.ProgramSkillFrame GuanduStage(global::CardGame.Core.ProgramSkillFrame f, string stage) => this._targetFactory().GuanduStage(f, stage);
    public bool RunGuanduGeneral(long id) => this._targetFactory().RunGuanduGeneral(id);
    public global::CardGame.Core.ProgramSkillFrame GuanduUpdate(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GuanduGeneralReceipt r) => this._targetFactory().GuanduUpdate(f, r);
    public global::CardGame.Core.Card[] GuanduOwnedCards(int seat, bool judgment = false) => this._targetFactory().GuanduOwnedCards(seat, judgment);
    public void DrawGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next) => this._targetFactory().DrawGuanduGeneral(f, recipient, count, next);
    public bool GuanduSkillEnabled(global::CardGame.Core.CharacterState owner, string skill) => this._targetFactory().GuanduSkillEnabled(owner, skill);
    public bool GuanduTurnDiscarded(int seat) => this._targetFactory().GuanduTurnDiscarded(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => this._targetFactory().CurrentActualDiscardRecoveryPhase();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void AdjustProgramNormalDraw(global::CardGame.Core.ProgramSkillFrame frame, int amount) => this._targetFactory().AdjustProgramNormalDraw(frame, amount);
    public void ChangeProgramMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int amount, string? sourceSkillId = default) => this._targetFactory().ChangeProgramMaximumHp(frame, amount, sourceSkillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreStrategicProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreStrategicProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> _targetFactory = targetFactory;
    public void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().SetStrategicMarker(seat, marker, sourceSeat, count, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreGuanduGeneralBenefitsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreGuanduGeneralBenefitsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}
