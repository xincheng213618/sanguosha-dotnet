#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecipientCategoryMarkObserversCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRecipientCategoryMarkObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecipientCategoryMarkObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreRecipientCategoryMarkObserversCoreRecipientCategorySlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetRecipientCategorySlashUse(int owner, global::CardGame.Core.ProgramSkillWindowContext c, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsRecipientCategorySlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecipientCategorySlashTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreRecipientCategoryMarkObserversCoreTurnDefaultStatsValidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactTurnDefaultStatPhaseParent(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRecipientCategoryMarkObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreRecipientCategoryMarkObserversCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreRecipientCategoryMarkObserversCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreRecipientCategoryMarkObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRecipientCategoryMarkObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreRecipientCategoryMarkObserversCoreRecipientCategoryMarkContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactRecipientCategoryDyingCursor(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry);
}

internal interface ICoreRecipientCategoryMarkObserversCoreRecipientCategoryMarksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RecipientCategoryMarkChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRecipientCategoryMarkObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRecipientCategoryMarkObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreRecipientCategoryMarkObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecipientCategoryMarkObserversCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default);
}

internal interface ICoreRecipientCategoryMarkObserversCoreDyingOwnedCardSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactDyingOwnedCardEntry(global::CardGame.Core.ProgramSkillFrame root, out global::CardGame.Core.DyingFrame dying, out global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry);
}
