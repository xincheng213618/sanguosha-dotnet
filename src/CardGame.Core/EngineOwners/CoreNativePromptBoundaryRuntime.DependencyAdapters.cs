#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasProgramCardIdentity(owner, card);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramCardIdentitySources(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse) => this._targetFactory().GetProgramCardIdentitySources(owner, card, effectiveKind, forResponse);
    public string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description) => this._targetFactory().DescribeConversion(source, description);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    public string ProgramConversionName(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ProgramConversionName(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get => this._targetFactory().ActiveTargetCardSelection; }
    public global::CardGame.Core.CardUseFrame? ActiveFireAttack { get => this._targetFactory().ActiveFireAttack; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreExtendedViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreExtendedViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtendedViewAsResponseChoices(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => this._targetFactory().ExtendedViewAsResponseChoices(owner, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreProgramFactionPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreProgramFactionPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramFactionPoliciesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind) => this._targetFactory().GetFactionResponsePolicy(owner, requiredKind);
    public string FactionPolicyName(global::CardGame.Core.FactionResponsePolicySource source) => this._targetFactory().FactionPolicyName(source);
    public string FactionProviderDescription(global::CardGame.Core.FactionResponsePolicySource source) => this._targetFactory().FactionProviderDescription(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreNativePromptBoundaryCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreProgramParticipantPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreProgramParticipantPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat) => this._targetFactory().GetPendingDecisionForActor(actorSeat);
    public bool IsIssuedParticipantPrompt(global::CardGame.Core.PendingDecision issued) => this._targetFactory().IsIssuedParticipantPrompt(issued);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> ProgramDodgeResponseChoices(global::CardGame.Core.CharacterState owner) => this._targetFactory().ProgramDodgeResponseChoices(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativePromptBoundaryCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime))]
internal sealed class CoreNativePromptBoundaryCoreSlashSuitDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreSlashSuitDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _targetFactory = targetFactory;
    public bool IsSlashDodgeCancellationPrevented(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().IsSlashDodgeCancellationPrevented(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime))]
internal sealed class CoreNativePromptBoundaryCoreMultiplayerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreMultiplayerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> _targetFactory = targetFactory;
    public bool IsHumanControlledSeat(int seat) => this._targetFactory().IsHumanControlledSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsOwnPublicPileCard(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response) => this._targetFactory().IsNativePublicPileOriginalChoice(owner, card, kind, response);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public bool CanUseCardAsResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().CanUseCardAsResponse(responder, card, requiredCardKind);
    public global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetZhangbaSlashPairs(responder);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetResponseCards(responder, requiredCardKind);
    public global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreNativePromptBoundaryCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreNativePromptBoundaryCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePromptBoundaryCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => this._targetFactory().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
}
