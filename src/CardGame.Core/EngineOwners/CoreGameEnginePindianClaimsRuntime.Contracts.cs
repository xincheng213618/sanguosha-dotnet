#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePindianClaimsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICorePindianClaimsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICorePindianClaimsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICorePindianClaimsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePindianClaimsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginWuLuKangPostComparison(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.PindianResult result, int comparison);
}

internal interface ICorePindianClaimsCharacterJianYongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBoundaryJianYongZongshi(global::CardGame.Core.PindianFrame p);
}

internal interface ICorePindianClaimsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePindianClaimsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDouyuPairWinDraws(global::CardGame.Core.PindianFrame pair);
}

internal interface ICorePindianClaimsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePindianClaimsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICorePindianClaimsCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetPindianPrompt(global::CardGame.Core.PindianFrame frame, int playerSeat, global::CardGame.Core.DecisionKind kind, string prompt, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.SkillPromptPresentation presentation);
    void CompletePindian(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianClaimsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICorePindianClaimsCorePindianMaximumSlashClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaximumSlashPindianClaim(global::CardGame.Core.PindianFrame frame);
    bool BeginMaximumSlashPindianClaims(global::CardGame.Core.PindianFrame frame);
    void ResolveMaximumSlashPindianClaim(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PromptChoice selected);
}

internal interface ICorePindianClaimsCharacterSunCeBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBoundarySunCeZhibaClaim(global::CardGame.Core.PindianFrame pair);
}
