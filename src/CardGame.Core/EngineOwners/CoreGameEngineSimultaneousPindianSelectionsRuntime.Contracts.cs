#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSimultaneousPindianSelectionsCoreSimultaneousPindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishSimultaneousChoice(global::CardGame.Core.ProgramSkillFrame f);
    bool DrainSimultaneousChildren(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianSelectionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSimultaneousPindianSelectionsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreSimultaneousPindianSelectionsRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICoreSimultaneousPindianSelectionsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDouyuSimultaneousPreReveal(global::CardGame.Core.ProgramSkillFrame frame);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> ApplyDouyuSimultaneousCards(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> original);
}

internal interface ICoreSimultaneousPindianSelectionsCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAlcoholKingIdentityPolicy(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreSimultaneousPindianSelectionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreSimultaneousPindianSelectionsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSimultaneousPindianSelectionsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner);
    bool TryBeginWuLuKangGroupBeforeReveal(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.WuLuKangPindianInput WuLuKangActualInput(global::CardGame.Core.ResolutionFrame parent, int seat, int id, global::CardGame.Core.CardLocation from, int rank, bool top);
    void RevealWuLuKangInputs(global::CardGame.Core.ResolutionFrame parent);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRevealedCard> WuLuKangOriginalSelectedGroup(global::CardGame.Core.ProgramSkillFrame f);
    bool IsWuLuKangPrivateGroup(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSimultaneousPindianSelectionsCorePindianRandomSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PindianPolicySourceLive(int seat, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.SkillProgramCardPolicyKind kind);
}

internal interface ICoreSimultaneousPindianSelectionsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreSimultaneousPindianSelectionsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreSimultaneousPindianSelectionsCorePindianPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferPindianTopChoice(global::CardGame.Core.CharacterState owner);
    int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreSimultaneousPindianSelectionsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreSimultaneousPindianSelectionsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreSimultaneousPindianSelectionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSimultaneousPindianSelectionsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EnsureDrawPile();
}

internal interface ICoreSimultaneousPindianSelectionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
