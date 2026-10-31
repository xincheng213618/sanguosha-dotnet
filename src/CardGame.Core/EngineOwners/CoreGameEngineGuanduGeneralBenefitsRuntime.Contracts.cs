#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGuanduGeneralBenefitsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreGuanduGeneralBenefitsKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreGuanduGeneralBenefitsCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduCommander(int seat);
    bool GuanduAllies(int first, int second);
    bool IsGuanduActivity { get; }
}

internal interface ICoreGuanduGeneralBenefitsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreGuanduGeneralBenefitsCoreGuanduGeneralChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishGuanduGeneralChoice(global::CardGame.Core.ProgramSkillFrame f, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduOwnCardChoices(global::CardGame.Core.ProgramSkillFrame f, int seat, string option, bool discard = false, bool allowSkip = true, bool nonEquipment = false, bool handOnly = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduForeignGainChoices(global::CardGame.Core.ProgramSkillFrame f, int seat);
    global::CardGame.Core.PromptChoice GuanduChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default);
}

internal interface ICoreGuanduGeneralBenefitsCoreGuanduGeneralsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanduShicaiReady(int ownerSeat, string instance);
    void FinishGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, bool completed = true);
    global::CardGame.Core.ProgramSkillFrame GuanduStage(global::CardGame.Core.ProgramSkillFrame f, string stage);
    bool RunGuanduGeneral(long id);
    global::CardGame.Core.ProgramSkillFrame GuanduUpdate(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GuanduGeneralReceipt r);
    global::CardGame.Core.Card[] GuanduOwnedCards(int seat, bool judgment = false);
    void DrawGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next);
    bool GuanduSkillEnabled(global::CardGame.Core.CharacterState owner, string skill);
    bool GuanduTurnDiscarded(int seat);
}

internal interface ICoreGuanduGeneralBenefitsCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
}

internal interface ICoreGuanduGeneralBenefitsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreGuanduGeneralBenefitsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdjustProgramNormalDraw(global::CardGame.Core.ProgramSkillFrame frame, int amount);
    void ChangeProgramMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int amount, string? sourceSkillId = default);
}

internal interface ICoreGuanduGeneralBenefitsCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreGuanduGeneralBenefitsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGuanduGeneralBenefitsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreGuanduGeneralBenefitsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreGuanduGeneralBenefitsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreGuanduGeneralBenefitsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}
