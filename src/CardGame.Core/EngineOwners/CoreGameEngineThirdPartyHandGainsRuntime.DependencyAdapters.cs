#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreThirdPartyHandGainsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreThirdPartyHandGainsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(global::CardGame.Core.CharacterState owner, int movedCardCount, global::CardGame.Core.CardMovementSourceCount sourceCount, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.CardMovementTiming? timing = default, global::CardGame.Core.CardMovementBatchContext? nativeDrawBatch = default) => this._targetFactory().CaptureCardsMovedTriggerFacts(owner, movedCardCount, sourceCount, window, timing, nativeDrawBatch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime))]
internal sealed class CoreThirdPartyHandGainsCharacterTangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreThirdPartyHandGainsCharacterTangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> _targetFactory = targetFactory;
    public bool IsKanggeMarkedSeat(int seat, int ownerSeat) => this._targetFactory().IsKanggeMarkedSeat(seat, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime))]
internal sealed class CoreThirdPartyHandGainsCoreProgramMovementSourceCountsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreThirdPartyHandGainsCoreProgramMovementSourceCountsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().ProgramMovementSourceCounts(batch, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreThirdPartyHandGainsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreThirdPartyHandGainsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreThirdPartyHandGainsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreThirdPartyHandGainsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}
