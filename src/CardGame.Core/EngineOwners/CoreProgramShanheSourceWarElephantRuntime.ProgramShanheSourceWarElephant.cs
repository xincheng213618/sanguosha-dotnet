#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramShanheSourceWarElephantRuntime
{
    internal const string ShanheSourceShouyi673 = "shanhe:source-shouyi-673";
    internal const string ShanheSourceXiongshou1136 = "shanhe:source-xiongshou-1136";
    internal bool IsShanheWarElephantMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    internal bool ShanheWarElephantHas(CharacterState owner, string skillId) => IsShanheWarElephantMode && owner.IsAlive && _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(program => program.Id == skillId);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 24530)]
    internal bool ShanheWarElephantUnlimitedCardDistance(ProgramCardDistanceQuery query) => ShanheWarElephantHas(query.Actor, ShanheSourceShouyi673);
    internal bool ShanheWarElephantCannotTurnOver(CharacterState owner) => ShanheWarElephantHas(owner, ShanheSourceXiongshou1136);
    SkillProgramStepOutcome global::CardGame.Core.IShanheWarElephantProgramHost.CounterSlash(ProgramSkillFrame frame) => UseShanheWarElephantCounterSlash(frame);
    internal SkillProgramStepOutcome UseShanheWarElephantCounterSlash(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsShanheWarElephantMode || frame.SkillId != ShanheSourceXiongshou1136 || frame.ShanheWarElephantCounterSlash is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 1 } context || context.TargetSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.TriggerWindow != context.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Source Xiongshou lost its actual first large-damage owner and original after-damage window.");
        global::CardGame.Core.GameEngine.IDamageAttempt original = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        if (original.TargetSeat != frame.OwnerSeat || original.DamageAmount != context.Amount || context.SourceSeat != (original.IsSourceLess ? null : original.SourceSeat))
            throw new InvalidOperationException("Source Xiongshou cannot borrow another damage's source or amount.");
        // The first qualifying damage consumes this actual turn's trigger even if
        // it has no living legal source. Smaller hits do not consume the trigger.
        if (context.SourceSeat is not { } source || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || source == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[source], new Card(0, CardKind.Slash, Suit.None, 0), new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), CardKind.Slash, noEffectiveRank: true, physicalCardIds: []))
            return SkillProgramStepOutcome.Continue;
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId;
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel?.OwnerFrameId;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheWarElephantCounterSlash = new(frame.InstructionIndex, window.Id, source, attack, duel, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1) });
        if (attack is { } attackId)
            _coreCardAttackStateOperations.UpdateCardAttackState(attackId, state => state!with { Active = false });
        if (duel is { } duelId)
            _coreCardAttackStateOperations.UpdateCardContinuations(duelId, state => state with { Duel = state.Duel!with { Active = false } });
        return _coreTriggeredCardProgramsOperations.UseTriggeredProgramVirtualSlash(frame, source);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 24530)]
    internal bool RestoreShanheWarElephantCounterSlashParent(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheWarElephantCounterSlash: { } receipt } parent || parent.Id != parentId)
            return false;
        if (receipt.ChildUseId != completion.ResolutionId || receipt.InstructionIndex != parent.InstructionIndex || _resolutionStackOperations._resolutionStack.Any(frame => frame.Id == receipt.ChildUseId))
            throw new InvalidOperationException("Source Xiongshou lost its exact retired counter Slash child.");
        if (receipt.SuspendedAttackId is { } attackId)
            _coreCardAttackStateOperations.UpdateCardAttackState(attackId, state => state!with { Active = true });
        if (receipt.SuspendedDuelId is { } duelId)
            _coreCardAttackStateOperations.UpdateCardContinuations(duelId, state => state with { Duel = state.Duel!with { Active = true } });
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { ShanheWarElephantCounterSlash = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }
}
