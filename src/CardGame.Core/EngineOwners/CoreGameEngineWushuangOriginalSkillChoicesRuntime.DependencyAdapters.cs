#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public bool EnsureDrawPile() => this._targetFactory().EnsureDrawPile();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default) => this._targetFactory().BeginProgramSkillDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreWushuangOriginalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreWushuangOriginalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillsRuntime> _targetFactory = targetFactory;
    public void AssertWushuangOriginalSkill(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertWushuangOriginalSkill(f);
    public void FinishWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, bool completed = true) => this._targetFactory().FinishWushuangOriginal(f, completed);
    public global::CardGame.Core.ProgramSkillFrame UpdateWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WushuangOriginalSkillReceipt r) => this._targetFactory().UpdateWushuangOriginal(f, r);
    public bool RunWushuangOriginalSkill(long id) => this._targetFactory().RunWushuangOriginalSkill(id);
    public void DrawWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next) => this._targetFactory().DrawWushuangOriginal(f, recipient, count, next);
    public global::CardGame.Core.ProgramSkillFrame WushuangOriginalStage(global::CardGame.Core.ProgramSkillFrame f, string stage) => this._targetFactory().WushuangOriginalStage(f, stage);
    public void PublishWushuangOriginalChoice(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => this._targetFactory().PublishWushuangOriginalChoice(f, choices);
    public global::CardGame.Core.PromptChoice WushuangOriginalChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => this._targetFactory().WushuangOriginalChoice(f, option, label, cards, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreGuanduActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreGuanduActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _targetFactory = targetFactory;
    public bool GuanduAllies(int first, int second) => this._targetFactory().GuanduAllies(first, second);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreWushuangOriginalSkillFlowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreWushuangOriginalSkillFlowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillFlowsRuntime> _targetFactory = targetFactory;
    public void DiscardWushuangOriginal(global::CardGame.Core.ProgramSkillFrame f, int owner, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().DiscardWushuangOriginal(f, owner, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
    public global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().AdvancedEffectiveHandKind(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCoreCharacterHostAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCoreCharacterHostOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<string> AcquireRuntimeSkills(global::CardGame.Core.CharacterState player, string sourceSkillId, global::System.Collections.Generic.IReadOnlyList<string> skillIds) => this._targetFactory().AcquireRuntimeSkills(player, sourceSkillId, skillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreWushuangOriginalSkillChoicesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillChoicesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}
