#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanSongAnniversaryRuntime
{
    internal bool HasAnniversaryHanSongUniqueHand(CharacterState owner) => owner.IsAlive && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(AnniversaryHanSongComposition.Yinbi) && _coreNationalSkillsOperations.HasRuntimeSkill(owner, AnniversaryHanSongComposition.Yinbi) && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner.Seat).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count != _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count);
    internal IEnumerable<RuleQueryContribution> AnniversaryHanSongNumericContributions(CharacterState owner, SkillRuleQuery query)
    {
        if (query == SkillRuleQuery.SlashLimit && HasAnniversaryHanSongUniqueHand(owner))
            yield return new UnlimitedRuleQueryContribution($"skill:{(owner.Seat)}:anniversary:yinbi:unique-hand");
    }

    internal RuleQueryEvaluation ApplyAnniversaryHanSongHandLimit(CharacterState owner, RuleQueryEvaluation ordinary)
    {
        if (!owner.IsAlive || !_contentRegistryOperations._contentRegistry.Skills.ContainsKey(AnniversaryHanSongComposition.Yinbi) || !_coreNationalSkillsOperations.HasRuntimeSkill(owner, AnniversaryHanSongComposition.Yinbi))
            return ordinary;
        var highest = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreRuleQueriesOperations.EvaluateHandLimit(p, suppressHanSongYinbi: true))).DefaultIfEmpty(CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(ordinary)).Max();
        var set = new FiniteRuleQueryContribution($"skill:{(owner.Seat)}:anniversary:yinbi:highest-hand-limit", SkillRuleOperation.Set, highest, 45900);
        var terms = Array.AsReadOnly(ordinary.Value.Contributions.Append(set).OrderBy(c => c.SourceId, StringComparer.Ordinal).ToArray());
        var stage = new RuleQueryStageSnapshot("yinbi-highest-hand-limit", ordinary.Value is FiniteRuleQueryValue n ? n.Value : 0, false, highest, Array.AsReadOnly<RuleQueryContribution>([set]));
        return new(new FiniteRuleQueryValue(highest, terms), ordinary.BaseTerms, Array.AsReadOnly(ordinary.Stages.Append(stage).ToArray()));
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 45900)]
    internal bool HanSongCardDistance(ProgramCardDistanceQuery q) => HasAnniversaryHanSongUniqueHand(q.Actor);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseLimitExemption, 45900)]
    internal bool HanSongCardDistanceLimit(CharacterState owner) => HasAnniversaryHanSongUniqueHand(owner);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 45900)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 45900)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 45900)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 45900)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 45900)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 45900)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 45900)]
    internal bool HanSongSlashDistance(ProgramSlashDistanceQuery q) => HasAnniversaryHanSongUniqueHand(q.Actor);
    internal IReadOnlyList<ProgramTriggerCandidate> AppendAnniversaryHanSongDrawEndedCandidates(CharacterState phaseOwner, IReadOnlyList<ProgramTriggerCandidate> existing)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryHanSongShuaiyan))
            return existing;
        var added = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != phaseOwner.Seat && _coreNationalSkillsOperations.HasRuntimeSkill(p, AnniversaryHanSongComposition.Shuaiyan)).SelectMany(p => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(p, SkillProgramTriggerWindow.DrawPhaseEnded, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(p))).Where(c => c.SkillId == AnniversaryHanSongComposition.Shuaiyan && c.BindingId == "other-draw-ended");
        return Array.AsReadOnly(existing.Concat(added).Distinct().OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(phaseOwner.Seat, c.OwnerSeat)).ThenByDescending(c => c.Priority).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.BindingId, StringComparer.Ordinal).ToArray());
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDrawEnd, 20)]
    internal bool IsAnniversaryHanSongForeignDrawContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => candidate.SkillId == AnniversaryHanSongComposition.Shuaiyan && candidate.BindingId == "other-draw-ended" && c.OwnerSeat == candidate.OwnerSeat && c.Window == SkillProgramTriggerWindow.DrawPhaseEnded && c.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && c.TargetSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && candidate.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { Window: SkillProgramTriggerWindow.DrawPhaseEnded, Continuation: ProgramLifecycleContinuation.CompleteDrawPhaseEnded } parent && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.Candidates.Contains(candidate);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45900, SkillProgramEffectOp.ResolveAnniversaryHanSongShuaiyan)]
    internal bool CanOfferAnniversaryHanSong(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId != AnniversaryHanSongComposition.Shuaiyan || c.SourceSeat is not { } subject || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(subject) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || c.OwnerSeat != candidate.OwnerSeat || c.TargetSeat != subject || subject != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[subject].IsAlive || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        if (c.Window is not (SkillProgramTriggerWindow.DrawPhaseEnded or SkillProgramTriggerWindow.DiscardPhaseEnded) || (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Draw && c.Window == SkillProgramTriggerWindow.DrawPhaseEnded) || (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard && c.Window == SkillProgramTriggerWindow.DiscardPhaseEnded))
            return false;
        return trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own ? subject == candidate.OwnerSeat : trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.OtherLiving && subject != candidate.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[subject]).Count == _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count;
    }
}
