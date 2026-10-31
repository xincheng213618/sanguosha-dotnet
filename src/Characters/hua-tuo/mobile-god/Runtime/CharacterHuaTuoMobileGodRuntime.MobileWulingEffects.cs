#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoMobileGodRuntime
{
    internal static PlayerMarkerKind MobileHtAnimalMarker(MobileWulingAnimal animal) => animal switch
    {
        MobileWulingAnimal.Tiger => PlayerMarkerKind.MobileWulingTiger,
        MobileWulingAnimal.Crane => PlayerMarkerKind.MobileWulingCrane,
        MobileWulingAnimal.Bear => PlayerMarkerKind.MobileWulingBear,
        MobileWulingAnimal.Ape => PlayerMarkerKind.MobileWulingApe,
        _ => PlayerMarkerKind.MobileWulingDeer
    };
    internal void SetMobileHtMarker(long frameId, int target, int teacher, PlayerMarkerKind marker, int count)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[target];
        var key = (marker, teacher);
        var before = p.MarkerSourceCounts.GetValueOrDefault(key);
        if (before == count)
            return;
        if (count == 0)
            p.MarkerSourceCounts.Remove(key);
        else
            p.MarkerSourceCounts[key] = count;
        var total = p.Markers.GetValueOrDefault(marker) + count - before;
        if (total == 0)
            p.Markers.Remove(marker);
        else
            p.Markers[marker] = total;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frameId, target, marker, count - before, total, teacher, "program.mobile-shen-hua-tuo.wuling"));
    }

    internal static string MobileHtCourseGrantId(long id, string skill) => $"acquired:mobile-wuling:{(id)}:{(skill)}";
    internal void GrantMobileHtCourseSkill(MobileWulingTaughtEvent course, string skill)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[course.TargetSeat];
        var grantId = MobileHtCourseGrantId(course.CourseId, skill);
        if (p.SkillGrants.Grants.Any(g => g.GrantId == grantId))
            return;
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        p.SkillGrants.Grant(new(grantId, skill, grantId, "acquired:mobile-wuling:" + course.CourseId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWulingGrantEvent(course.CourseId, p.Seat, skill, grantId, true));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(p.Seat, MobileHtWuling, Array.AsReadOnly(new[] { skill })));
    }

    internal void RevokeMobileHtCourseSkill(MobileWulingTaughtEvent course, string skill)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[course.TargetSeat];
        var grantId = MobileHtCourseGrantId(course.CourseId, skill);
        var grants = _coreDamageTargetSkillRoundDrawOperations.OwnedAndRestorableRoundSkillGrants(p, skill).Where(g => g.GrantId == grantId).ToArray();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        foreach (var g in grants)
        {
            p.SkillGrants.RemoveGrant(g.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWulingGrantRevokedEvent(course.CourseId, p.Seat, g.SkillId, g.GrantId, g.SkillInstanceId, g.SourceId));
        }
    }

    internal void EndMobileHtCourse(long id, long frameId)
    {
        var course = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWulingTaughtEvent>().Single(c => c.CourseId == id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWulingAdvancedEvent(frameId, id, MobileHtCourseOrder(id).Length, false));
        SetMobileHtMarker(frameId, course.TargetSeat, course.Source.OwnerSeat, PlayerMarkerKind.MobileWuling, 0);
        foreach (var animal in Enum.GetValues<MobileWulingAnimal>())
            SetMobileHtMarker(frameId, course.TargetSeat, course.Source.OwnerSeat, MobileHtAnimalMarker(animal), 0);
        RevokeMobileHtCourseSkill(course, MobileHtDeer);
        RevokeMobileHtCourseSkill(course, MobileHtEffects);
    }

    internal void ApplyMobileHtAnimal(ProgramSkillFrame f)
    {
        var r = f.MobileHuaTuo!;
        var course = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWulingTaughtEvent>().Single(c => c.CourseId == r.CourseId);
        if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[course.Source.OwnerSeat].IsAlive)
        {
            EndMobileHtCourse(r.CourseId, f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.Children } });
            return;
        }

        var animal = r.Order[r.AnimalIndex];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.EntryChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWulingAdvancedEvent(f.Id, r.CourseId, r.AnimalIndex, true));
        SetMobileHtMarker(f.Id, r.TargetSeat, course.Source.OwnerSeat, PlayerMarkerKind.MobileWuling, 1);
        foreach (var a in Enum.GetValues<MobileWulingAnimal>())
            SetMobileHtMarker(f.Id, r.TargetSeat, course.Source.OwnerSeat, MobileHtAnimalMarker(a), a == animal ? 1 : 0);
        GrantMobileHtCourseSkill(course, MobileHtEffects);
        if (animal == MobileWulingAnimal.Deer)
            GrantMobileHtCourseSkill(course, MobileHtDeer);
        else
            RevokeMobileHtCourseSkill(course, MobileHtDeer);
    }

    internal void BeginMobileHtEntryBenefit(ProgramSkillFrame f)
    {
        var r = f.MobileHuaTuo!;
        var animal = r.Order[r.AnimalIndex];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.Children } });
        if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            return;
        if (animal == MobileWulingAnimal.Crane)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileHuaTuo = f.MobileHuaTuo!with { Stage = MobileHuaTuoStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.TargetSeat, 3, new("skill.mobile-shen-hua-tuo.wuling.crane"));
            return;
        }

        if (animal == MobileWulingAnimal.Ape)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHuaTuo = f.MobileHuaTuo!with { Stage = MobileHuaTuoStage.Ape } });
            return;
        }

        if (animal == MobileWulingAnimal.Deer)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = f.MobileHuaTuo!with { Stage = MobileHuaTuoStage.DeerRecoveryChildren } });
            MobileHtRecoverOne(f, r.TargetSeat);
        }
    }

    internal void ClearMobileHtDeerJudgments(ProgramSkillFrame f)
    {
        var r = f.MobileHuaTuo!;
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        var judgments = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive ? _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.Children, Before = before } });
        if (judgments.Length > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var card in judgments)
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Judgment(r.TargetSeat), CardLocation.DiscardPile, new("skill.mobile-shen-hua-tuo.wuling.deer"));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileHuaTuo = f.MobileHuaTuo!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.TeachMobileShenHuaTuoWuling)]
    internal void ReturnMobileHtTeachDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnMobileHtCrane(f, outcome);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.AdvanceMobileShenHuaTuoWuling)]
    internal void ReturnMobileHtAdvanceDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnMobileHtCrane(f, outcome);
    internal void ReturnMobileHtCrane(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.MobileHuaTuo ?? throw new InvalidOperationException("Crane lost its actual native draw parent.");
        if (r.Stage != MobileHuaTuoStage.Drawing || r.Order[r.AnimalIndex] != MobileWulingAnimal.Crane || outcome.RecipientSeat != r.TargetSeat || outcome.RequestedCount != 3 || f.NativeDraw is not null)
            throw new InvalidOperationException("Crane requires its exact once-paid native three-card draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.Children, Before = outcome.SequenceBefore, After = outcome.SequenceAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileHuaTuoPaidEvent(f.Id, r.AnimalIndex, "crane", r.TargetSeat, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueMobileHt(f.Id);
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 39100)]
    internal int MobileHtTigerDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack.IsSourceLess || !MobileHtSpirit(attack.CardUserSeat, MobileWulingAnimal.Tiger) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { Type: CardActionType.Use } action } || action.ActorSeat != attack.CardUserSeat || action.EffectiveDesignatedTargetSeats is not [var target] || target != attack.TargetSeat)
            return 0;
        return 1;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 39100)]
    internal void ObserveMobileHtDeath(IGameEvent payload)
    {
        if (payload is not PlayerDiedEvent died)
            return;
        var live = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWulingTaughtEvent>().Where(c => c.Source.OwnerSeat == died.VictimSeat || c.TargetSeat == died.VictimSeat).Where(c => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWulingAdvancedEvent>().LastOrDefault(e => e.CourseId == c.CourseId)?.Active == true).ToArray();
        foreach (var c in live)
            EndMobileHtCourse(c.CourseId, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? c.CourseId);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 39100)]
    internal void AssertMobileHt(ProgramSkillFrame f)
    {
        if (f.MobileHuaTuo is not { } r)
            return;
        if (r.Source != MobileHtSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || r.Order.Count > 5 || r.Order.Distinct().Count() != r.Order.Count || r.Order.Any(a => !Enum.IsDefined(a)) || r.AnimalIndex < 0 || r.AnimalIndex > 5 || r.Cursor < 0 || r.Cursor > r.Recipients.Count || r.Cards.Distinct().Count() != r.Cards.Count || r.Recipients.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.TargetSeat != -1 && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.Op is not (SkillProgramEffectOp.TeachMobileShenHuaTuoWuling or SkillProgramEffectOp.AdvanceMobileShenHuaTuoWuling or SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi or SkillProgramEffectOp.RecoverMobileShenHuaTuoYouyi) || r.Stage is MobileHuaTuoStage.Entry or MobileHuaTuoStage.Drawing or MobileHuaTuoStage.Ape && (r.Order.Count != 5 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWulingTaughtEvent>().Count(e => e.CourseId == r.CourseId && e.TargetSeat == r.TargetSeat) != 1))
            throw new InvalidOperationException("Mobile Hua Tuo lost its exact original source, frozen order, paid materials or native cursor.");
    }
}
