#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CorePindianClaimsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CorePindianClaimsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CorePindianClaimsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CorePindianClaimsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool TryBeginWuLuKangPostComparison(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.PindianResult result, int comparison) => this._targetFactory().TryBeginWuLuKangPostComparison(parent, result, comparison);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime))]
internal sealed class CorePindianClaimsCharacterJianYongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCharacterJianYongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TryBeginBoundaryJianYongZongshi(global::CardGame.Core.PindianFrame p) => this._targetFactory().TryBeginBoundaryJianYongZongshi(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CorePindianClaimsCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginDouyuPairWinDraws(global::CardGame.Core.PindianFrame pair) => this._targetFactory().TryBeginDouyuPairWinDraws(pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CorePindianClaimsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime))]
internal sealed class CorePindianClaimsCorePindianAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCorePindianOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> _targetFactory = targetFactory;
    public void SetPindianPrompt(global::CardGame.Core.PindianFrame frame, int playerSeat, global::CardGame.Core.DecisionKind kind, string prompt, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.SkillPromptPresentation presentation) => this._targetFactory().SetPindianPrompt(frame, playerSeat, kind, prompt, choices, cards, targets, presentation);
    public void CompletePindian(global::CardGame.Core.PindianFrame frame) => this._targetFactory().CompletePindian(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CorePindianClaimsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime))]
internal sealed class CorePindianClaimsCorePindianMaximumSlashClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCorePindianMaximumSlashClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> _targetFactory = targetFactory;
    public bool HasMaximumSlashPindianClaim(global::CardGame.Core.PindianFrame frame) => this._targetFactory().HasMaximumSlashPindianClaim(frame);
    public bool BeginMaximumSlashPindianClaims(global::CardGame.Core.PindianFrame frame) => this._targetFactory().BeginMaximumSlashPindianClaims(frame);
    public void ResolveMaximumSlashPindianClaim(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveMaximumSlashPindianClaim(frame, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime))]
internal sealed class CorePindianClaimsCharacterSunCeBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianClaimsCharacterSunCeBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TryBeginBoundarySunCeZhibaClaim(global::CardGame.Core.PindianFrame pair) => this._targetFactory().TryBeginBoundarySunCeZhibaClaim(pair);
}
