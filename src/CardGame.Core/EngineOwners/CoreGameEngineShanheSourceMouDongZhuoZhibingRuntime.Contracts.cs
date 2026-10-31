#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSourceMouDongZhuoZhibingCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreShanheSourceMouDongZhuoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSourceMouMode { get; }
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOngoingActualUseTurn { get; }
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheSourceMouDongZhuoZhibingContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShanheSourceMouDongZhuoZhibingKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreShanheSourceMouDongZhuoZhibingCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantIssuedProgramTurnSkills(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, global::System.Collections.Generic.IReadOnlyList<string> skillIds, global::CardGame.Core.SkillGrantTurnExpiry expiry);
}
