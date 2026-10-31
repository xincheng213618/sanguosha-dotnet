#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMouRuntime
{
    internal MouGuoJiaRuntime MouGuoJia => _mouGuoJiaOperations.Get();

    int global::CardGame.Core.IMouGuoJiaFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long global::CardGame.Core.IMouGuoJiaFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouGuoJiaFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouGuoJiaFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool global::CardGame.Core.IMouGuoJiaFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string global::CardGame.Core.IMouGuoJiaFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    SkillProgram? global::CardGame.Core.IMouGuoJiaFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouGuoJiaFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool global::CardGame.Core.IMouGuoJiaFacts.Parent(ProgramSkillFrame f) => MouGjParent(f);
    bool global::CardGame.Core.IMouGuoJiaFacts.Yin(int s, string instance) => _coreProgramPersistentStateOperations.GetProgramBooleanState(s, MouGuoJiaRuntime.Xianmou, instance, "yin");
    IReadOnlyList<int> global::CardGame.Core.IMouGuoJiaFacts.Living => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    IReadOnlyList<MouGjEntity> global::CardGame.Core.IMouGuoJiaFacts.Hand(int s) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Select(c => new MouGjEntity(c.Id, c.Kind, CardLocation.Hand(s), c.DisplayName)).ToArray());
    bool global::CardGame.Core.IMouGuoJiaFacts.At(MouGjEntity c) => _cardZonesOperations._cardZones.GetLocation(c.Id) == c.From;
    bool global::CardGame.Core.IMouGuoJiaFacts.Discardable(int s, int id) => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(s) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(s, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(s), OwnedCardMoveIntent.Discard);
    int global::CardGame.Core.IMouGuoJiaFacts.LossCount(int s) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && m.From.OwnerSeat == s && m.From != m.To && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment);
    IReadOnlyList<(int Seat, string Instance)> global::CardGame.Core.IMouGuoJiaFacts.XianmouOwners => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreSkillProgramsOperations.EnabledSkillPrograms(p).Any(s => s.Id == MouGuoJiaRuntime.Xianmou)).Select(p => (p.Seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(p, MouGuoJiaRuntime.Xianmou))).ToArray());

    internal bool MouGjParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != w.ParentFrameId)
            return false;
        if (f.TriggerId == "opening-state")
            return f.SkillId == MouGuoJiaRuntime.Xianmou && w.Window == SkillProgramTriggerWindow.GameStarting && _resolutionStackOperations._resolutionStack[at - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } life && life.CandidateIndex >= 0 && life.CandidateIndex < life.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, life.Candidates[life.CandidateIndex]);
        if (f.SkillId == MouGuoJiaRuntime.Xianmou && f.TriggerId is "own-ended-loss" or "other-ended-loss")
            return w.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStackOperations._resolutionStack[at - 1] is DeferredTurnEndFrame ended && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended));
        return f.SkillId == MouGuoJiaRuntime.Yiji && f.TriggerId == "per-damage-point" && w.Window == SkillProgramTriggerWindow.AfterDamageApplied && w.TargetSeat == f.OwnerSeat && w.Amount > 0 && _resolutionStackOperations._resolutionStack[at - 1] is DamageTriggerWindowFrame damage && damage.ParentFrameId == w.DamageFrameId && damage.TriggerWindow == w.Window && damage.CandidateIndex >= 0 && damage.CandidateIndex < damage.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, damage.Candidates[damage.CandidateIndex].ToProgramCandidate());
    }

    void global::CardGame.Core.IMouGuoJiaNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouGuoJiaNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouGuoJiaNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouGuoJiaNative.State(ProgramSkillFrame f, bool yin)
    {
        if (!MouGuoJia.Valid(f) || f.SkillId != MouGuoJiaRuntime.Xianmou)
            throw new InvalidOperationException("Xianmou state requires its issued opening or paid original branch.");
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, f.SkillId, f.SkillInstanceId, "yin", yin);
    }

    IReadOnlyList<MouGjEntity> global::CardGame.Core.IMouGuoJiaNative.Peek(ProgramSkillFrame f, int count)
    {
        if (!MouGuoJia.Valid(f) || f.MouGuoJia is not { } r || !(r.Stage == MouGuoJiaStage.RemoveGrantChildren && count == 5 || r.Operation == SkillProgramEffectOp.ResolveMouGuoJiaYiji && r.Stage == MouGuoJiaStage.YijiCard && count == 2))
            throw new InvalidOperationException("Mou Guo Jia can only privately view the exact current top five or inherited Yiji top two.");
        _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        return Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(count).Reverse().Select(c => new MouGjEntity(c.Id, c.Kind, CardLocation.DrawPile, c.DisplayName)).ToArray());
    }

    void global::CardGame.Core.IMouGuoJiaNative.Move(ProgramSkillFrame f)
    {
        if (!MouGuoJia.Valid(f) || f.MouGuoJia is not { Stage: MouGuoJiaStage.MovementChildren or MouGuoJiaStage.YijiChildren } r || r.Selected.Any(id => !r.Viewed.Any(c => c.Id == id && _cardZonesOperations._cardZones.GetLocation(id) == c.From)) || r.Yin && r.Selected.Any(id => !_characterGuoJiaMouOperations.CreateMouGuoJiaFacts().Discardable(r.Target, id)))
            throw new InvalidOperationException("Mou Guo Jia lost his original privately viewed payable entity.");
        var yiji = r.Stage == MouGuoJiaStage.YijiChildren;
        var to = r.Yin ? CardLocation.DiscardPile : CardLocation.Hand(yiji ? r.Target : f.OwnerSeat);
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouGuoJia!;
            if (!MouGuoJia.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Viewed.Any(c => r.Selected.Contains(c.Id) && c.Id == m.CardId && c.From == m.From) || m.Sequence <= r.Before || m.To != to && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Guo Jia lost his actual selected gain, native logical discard or top-card Give receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouGuoJia = paid with { Actual = moves.Count(m => m.To == to), After = moves.Max(m => m.Sequence) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGjMovementPaidEvent(f.Id, r.Cursor, moves.Count(m => m.To == to), r.Before, moves.Max(m => m.Sequence)));
            if (yiji)
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, r.Selected, r.Before);
        }

        if (r.Yin)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, r.Selected, new("skill-program.mou:xianmou.discard"), Paid);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, to, new(yiji ? "skill-program.mou-guo-jia:yiji.give" : "skill-program.mou:xianmou.obtain"), Paid);
    }

    void global::CardGame.Core.IMouGuoJiaNative.YijiGrant(ProgramSkillFrame f, bool acquire)
    {
        if (!MouGuoJia.Valid(f) || f.MouGuoJia is not { Yin: false } r || !(acquire ? r.Stage == MouGuoJiaStage.GrantChildren && r.Actual < r.X : r.Stage == MouGuoJiaStage.RemoveGrantChildren))
            throw new InvalidOperationException("Only another actual Yang activation may replace its own earlier Yiji source.");
        var key = MouGuoJiaRuntime.GrantSource(f);
        var count = 0;
        if (acquire)
            count = _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], key, [MouGuoJiaRuntime.Yiji]).Count;
        else
            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == MouGuoJiaRuntime.Yiji && g.SourceId == "acquired:" + key).ToArray())
                if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId))
                    count++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGjYijiGrantEvent(f.Id, r.Source, key, acquire, count));
    }

    void global::CardGame.Core.IMouGuoJiaNative.Judge(ProgramSkillFrame f) => BeginMouGjJudgment(f);
    void global::CardGame.Core.IMouGuoJiaNative.Damage(ProgramSkillFrame f)
    {
        if (!MouGuoJia.Valid(f) || f.MouGuoJia is not { Stage: MouGuoJiaStage.DamageChildren, LightningHit: true } r || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGjLightningIssuedEvent>().Count(p => p.FrameId == f.Id && p.JudgmentId == r.JudgmentId && p.Target == f.OwnerSeat && p.Amount == 3) != 1)
            throw new InvalidOperationException("Xianmou requires his exact finalized Lightning hit once.");
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 3, nature: DamageNature.Thunder, sourceLess: true);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 37100)]
    internal CardSnapshot[] MouGjPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.MouGuoJia is not null).SelectMany(f => f.MouGuoJia!.Viewed.Select(c => c.Id)).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray();
}
