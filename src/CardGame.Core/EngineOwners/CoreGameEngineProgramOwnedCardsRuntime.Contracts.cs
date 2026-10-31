#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramOwnedCardsCoreTurnDrawDebtsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect TurnDrawDebtOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreProgramOwnedCardsCoreHalfHandPhaseDebtProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect PhaseHandDebtOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
    bool MatchesDeferredHandDebtCost(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location);
}

internal interface ICoreProgramOwnedCardsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreProgramOwnedCardsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramOwnedCardsCorePublicPileCashOutOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesPublicPileStorageSelection(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location);
}

internal interface ICoreProgramOwnedCardsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreProgramOwnedCardsCoreOwnedExactCountAndNamedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FreezeExactOwnedSelectionSuits(long frameId, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations);
}

internal interface ICoreProgramOwnedCardsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreProgramOwnedCardsCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect DeferredOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect original);
    void CompleteDeferredAlignmentDiscard(long frameId, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation> locations);
}

internal interface ICoreProgramOwnedCardsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramOwnedCardsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false);
}

internal interface ICoreProgramOwnedCardsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
    void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default);
}

internal interface ICoreProgramOwnedCardsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreProgramOwnedCardsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramOwnedCardsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetLivingPlayersMinHp();
    int GetLivingFactionCount();
}

internal interface ICoreProgramOwnedCardsCoreTriggeredCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesTriggeredSlashMaterial(global::CardGame.Core.ProgramSkillFrame frame, string bind, int ownerSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location);
}

internal interface ICoreProgramOwnedCardsCoreSharedSlashOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesProgramOwnedSelectionKind(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> kinds);
}

internal interface ICoreProgramOwnedCardsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit GetProgramEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation = default);
}

internal interface ICoreProgramOwnedCardsCorePindianHandReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FreezePindianHalfReturn(global::CardGame.Core.ProgramSkillFrame f, int count);
    bool HasPaidPindianHandReturn(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramOwnedCardsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreProgramOwnedCardsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramOwnedCardsCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSelectSelfDiscardBinding(global::CardGame.Core.ProgramSkillFrame f, string bind, int selectionActorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location);
}

internal interface ICoreProgramOwnedCardsCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasForeignDiscardCapability { get; }
}

internal interface ICoreProgramOwnedCardsCoreForcedPhaseAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseAlcoholChain(global::CardGame.Core.ProgramSkillFrame frame);
    bool CanUsePhaseOwnerAlcohol(global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramOwnedCardsCoreProgramCategoryTargetLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetProgramCategoryTargetTurnUsage(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramOwnedCardsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramOwnedCardsCoreEquipmentPairPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect EquipmentPairOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
    bool MatchesEquipmentPairOwnedCost(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from);
}

internal interface ICoreProgramOwnedCardsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}
