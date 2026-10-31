#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangOriginalSkillChoicesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreWushuangOriginalSkillChoicesCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EnsureDrawPile();
}

internal interface ICoreWushuangOriginalSkillChoicesCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreWushuangOriginalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertWushuangOriginalSkill(global::CardGame.Core.ProgramSkillFrame f);
    void FinishWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, bool completed = true);
    global::CardGame.Core.ProgramSkillFrame UpdateWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WushuangOriginalSkillReceipt r);
    bool RunWushuangOriginalSkill(long id);
    void DrawWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next);
    global::CardGame.Core.ProgramSkillFrame WushuangOriginalStage(global::CardGame.Core.ProgramSkillFrame f, string stage);
    void PublishWushuangOriginalChoice(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices);
    global::CardGame.Core.PromptChoice WushuangOriginalChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanduAllies(int first, int second);
}

internal interface ICoreWushuangOriginalSkillChoicesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreWushuangOriginalSkillFlowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void DiscardWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, int owner, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreWushuangOriginalSkillChoicesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreWushuangOriginalSkillChoicesCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
    global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreWushuangOriginalSkillChoicesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreWushuangOriginalSkillChoicesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWushuangOriginalSkillChoicesCoreCharacterHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> AcquireRuntimeSkills(global::CardGame.Core.CharacterState player, string sourceSkillId, global::System.Collections.Generic.IReadOnlyList<string> skillIds);
}

internal interface ICoreWushuangOriginalSkillChoicesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}
