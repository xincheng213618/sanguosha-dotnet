#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePindianRandomSelectionsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICorePindianRandomSelectionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICorePindianRandomSelectionsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICorePindianRandomSelectionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePindianRandomSelectionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICorePindianRandomSelectionsCorePindianPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ReserveOrReadPindianTop(global::CardGame.Core.CharacterState owner, bool reserve);
}

internal interface ICorePindianRandomSelectionsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICorePindianRandomSelectionsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetPindianPrompt(global::CardGame.Core.PindianFrame frame, int playerSeat, global::CardGame.Core.DecisionKind kind, string prompt, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.SkillPromptPresentation presentation);
    void RevealPindian(global::CardGame.Core.PindianFrame frame, int opponentCardId, bool opponentTop = false);
    void PublishPindianSelection(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianRandomSelectionsRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICorePindianRandomSelectionsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}
