#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGuanduGeneralNativeCardsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreGuanduGeneralNativeCardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreGuanduGeneralNativeCardsCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduActivity { get; }
}

internal interface ICoreGuanduGeneralNativeCardsCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreGuanduGeneralNativeCardsCoreGuanduGeneralsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GuanduStage(global::CardGame.Core.ProgramSkillFrame f, string stage);
    global::CardGame.Core.Card[] GuanduOwnedCards(int seat, bool judgment = false);
    void FinishGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, bool completed = true);
    void DrawGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next);
    bool RunGuanduGeneral(long id);
}

internal interface ICoreGuanduGeneralNativeCardsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreGuanduGeneralNativeCardsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreGuanduGeneralNativeCardsCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
}

internal interface ICoreGuanduGeneralNativeCardsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreGuanduGeneralNativeCardsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreGuanduGeneralNativeCardsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreGuanduGeneralNativeCardsCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource HuangQuanSource(global::CardGame.Core.ProgramSkillFrame f);
    void PublishJianjiChoice(global::CardGame.Core.ProgramSkillFrame f);
    bool SameJianjiReceipt(global::CardGame.Core.JianjiNativeReceipt a, global::CardGame.Core.JianjiNativeReceipt b);
}

internal interface ICoreGuanduGeneralNativeCardsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreGuanduGeneralNativeCardsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreGuanduGeneralNativeCardsCoreGuanduGeneralChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishGuanduGeneralChoice(global::CardGame.Core.ProgramSkillFrame f, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices);
    global::CardGame.Core.PromptChoice GuanduChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduOwnCardChoices(global::CardGame.Core.ProgramSkillFrame f, int seat, string option, bool discard = false, bool allowSkip = true, bool nonEquipment = false, bool handOnly = false);
}

internal interface ICoreGuanduGeneralNativeCardsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}
