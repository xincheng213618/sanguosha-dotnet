#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentTurnShownCardBenefitsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default);
}

internal interface ICoreCurrentTurnShownCardBenefitsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCurrentTurnShownCardBenefitsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreCurrentTurnShownCardBenefitsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueCurrentTurnNonLockedSkillSuppression(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCurrentTurnShownCardBenefitsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
}
