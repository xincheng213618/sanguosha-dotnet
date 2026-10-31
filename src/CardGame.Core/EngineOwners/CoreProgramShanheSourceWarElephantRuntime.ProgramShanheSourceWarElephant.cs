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

    internal IReadOnlyList<Card> SeparateShanheWarElephantCounterParentProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (!IsShanheWarElephantMode || processing.Count == 0)
            return processing;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.ShanheWarElephantCounterSlash?.ChildUseId == attack.ResolutionId);
        if (parent?.ShanheWarElephantCounterSlash is not { } receipt)
            return processing;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == parent.Id);
        if (parent.SkillId != ShanheSourceXiongshou1136 || receipt.InstructionIndex != parent.InstructionIndex || parent.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 1 } context || context.TargetSeat != parent.OwnerSeat || context.SourceSeat != receipt.SourceSeat || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.Id != receipt.DamageWindowId || window.Id != context.ParentFrameId || window.TriggerWindow != context.Window || !_coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(parent, window) || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not CardUseFrame { CardId: 0, CardKind: CardKind.Slash, Action: { } action } child || child.Id != receipt.ChildUseId || child.SourceSeat != parent.OwnerSeat || attack.ProgramSkillCardUseFrameId != parent.Id || attack.SourceSeat != parent.OwnerSeat || attack.TargetSeat != receipt.SourceSeat || attack.PhysicalCards.Count != 0 || action.Type != CardActionType.Use || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat || action.PhysicalCards.Count != 0 || action.EffectiveKind != CardKind.Slash || !action.TargetSeats.SequenceEqual([receipt.SourceSeat]) || !action.ConversionChain.SequenceEqual([new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)]) || action.ParentActionId != _resolutionStackOperations._resolutionStack.Take(index).OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId)
            throw new InvalidOperationException("Source Xiongshou counter lost its exact damaged-owner window or issued native child chain.");
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 2] is not DamageFrame damage || damage.Id != window.ParentFrameId || context.DamageFrameId != damage.Id || damage.SourceSeat != receipt.SourceSeat || damage.TargetSeat != parent.OwnerSeat || damage.Amount != context.Amount)
            throw new InvalidOperationException("Source Xiongshou counter lost its exact already-applied parent damage entity.");
        // This original is deliberately inactive while its counter owns the
        // response window. Read the exact stored owner, never the active cursor.
        global::CardGame.Core.GameEngine.IDamageAttempt original = _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == damage.ParentFrameId) switch
        {
            ProgramSkillFrame { AttackAttempt: not null } program => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(program.Id),
            CardUseFrame { CardAttack: not null } use => _kernelCoreMainOperations.CreateCardAttackHandle(use.Id),
            ProgramSkillFrame { CardAttack: not null } program => _kernelCoreMainOperations.CreateCardAttackHandle(program.Id),
            JudgmentFrame { CardAttack: not null } judgment => _kernelCoreMainOperations.CreateCardAttackHandle(judgment.Id),
            _ => throw new InvalidOperationException("Source Xiongshou counter lost its original stored damage-attempt owner.")};
        if (!original.DamageWasApplied || original.TargetSeat != parent.OwnerSeat || original.IsSourceLess || original.SourceSeat != receipt.SourceSeat || original.DamageAmount != context.Amount)
            throw new InvalidOperationException("Source Xiongshou counter cannot claim another damage window's retained materials.");
        var held = new HashSet<int>();
        if (receipt.SuspendedAttackId is { } attackId)
        {
            var ownerIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == attackId);
            if (ownerIndex < 0 || ownerIndex >= index - 1 || _coreCardAttackStateOperations.GetCardAttackState(attackId)is not { Active: false } state)
                throw new InvalidOperationException("Source Xiongshou retained material requires its actual inactive ancestor attack.");
            foreach (var id in state.PhysicalCardIds)
                Hold(id);
        }

        if (receipt.SuspendedDuelId is { } duelId)
        {
            var ownerIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == duelId);
            if (ownerIndex < 0 || ownerIndex >= index - 1 || _coreCardAttackStateOperations.GetCardContinuations(duelId).Duel is not { Active: false } || _resolutionStackOperations._resolutionStack[ownerIndex] is not CardUseFrame duel)
                throw new InvalidOperationException("Source Xiongshou retained material requires its actual inactive ancestor Duel.");
            foreach (var id in duel.Action?.PhysicalCards.Select(card => card.CardId) ?? duel.PhysicalCardIds ?? [])
                Hold(id);
        }

        return held.Count == 0 ? processing : processing.Where(card => !held.Contains(card.Id)).ToArray();
        void Hold(int cardId)
        {
            if (_cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Processing)
                return;
            if (_coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(move => move.CardId == cardId)is not { To: var destination } || destination != CardLocation.Processing || attack.PhysicalCards.Any(card => card.Id == cardId))
                throw new InvalidOperationException("Source Xiongshou retained material lost its real paid ancestor ownership.");
            held.Add(cardId);
        }
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
