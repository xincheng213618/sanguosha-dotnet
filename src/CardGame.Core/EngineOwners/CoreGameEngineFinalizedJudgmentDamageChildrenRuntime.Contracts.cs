#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreFinalizedJudgmentDamageChildrenCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenZfJudgmentGainDisposition(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreFinalizedJudgmentDamageChildrenCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreFinalizedJudgmentDamageChildrenCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesProgramJudgmentInstruction(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
    bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame);
}

internal interface ICoreFinalizedJudgmentDamageChildrenCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreFinalizedJudgmentDamageChildrenResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreFinalizedJudgmentDamageChildrenCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageProgramMatchesWindow(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DamageTriggerWindowFrame window);
}

internal interface ICoreFinalizedJudgmentDamageChildrenCoreJudgmentOutcomeClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasExactJudgmentOutcomeClaimDisposition(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreFinalizedJudgmentDamageChildrenContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreFinalizedJudgmentDamageChildrenCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}
