#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreThirdPartyHandGainsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(global::CardGame.Core.CharacterState owner, int movedCardCount, global::CardGame.Core.CardMovementSourceCount sourceCount, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.CardMovementTiming? timing = default, global::CardGame.Core.CardMovementBatchContext? nativeDrawBatch = default);
}

internal interface ICoreThirdPartyHandGainsCharacterTangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKanggeMarkedSeat(int seat, int ownerSeat);
}

internal interface ICoreThirdPartyHandGainsCoreProgramMovementSourceCountsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreThirdPartyHandGainsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreThirdPartyHandGainsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
