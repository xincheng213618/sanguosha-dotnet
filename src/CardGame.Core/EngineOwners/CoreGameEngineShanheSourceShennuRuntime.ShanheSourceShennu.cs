#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceShennuRuntime
{
    internal const string ShanheSourceShennuSkill = "shanhe:source-shennu-14214";
    internal const string ShanheSourceShennuDrawReason = "skill-program.shanhe-source-shennu-14214.draw";
    internal bool IsShanheSourceShennuMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    void IShanheSourceShennuHost.ReceiveDamage(ProgramSkillFrame frame) => ReceiveShanheSourceShennuDamage(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62048, SkillProgramEffectOp.ResolveShanheSourceShennu)]
    internal bool CanReceiveShanheSourceShennuDamage(ProgramTriggerLegalityContext context) => IsShanheSourceShennuMode && context.WindowContext is { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying, Amount: > 0 } window && window.TargetSeat == context.OwnerSeat;
    internal void ReceiveShanheSourceShennuDamage(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourceShennuMode || frame.SkillId != ShanheSourceShennuSkill || frame.ShanheShennuReceived is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying, Amount: > 0 } context || context.TargetSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != context.DamageFrameId || window.TriggerWindow != context.Window || _resolutionStackOperations._resolutionStack[^3] is not DamageFrame damage || damage.Id != window.ParentFrameId || damage.TargetSeat != frame.OwnerSeat || damage.Amount != context.Amount || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Source Shennu lost its exact positive applied-damage owner.");
        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
        if (attack.ResolutionId != damage.ParentFrameId || attack.TargetSeat != damage.TargetSeat || attack.SourceSeat != damage.SourceSeat || context.SourceSeat != (attack.IsSourceLess ? null : attack.SourceSeat))
            throw new InvalidOperationException("Source Shennu cannot borrow another damage's source or turn owner.");
        var sourceHp = context.SourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].HasHpValue ? Math.Max(0, _coreMatchParticipantsPlayersAccess._players[source].Hp) : 0;
        var before = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.ShanheSourceNu);
        var receipt = new ShanheShennuReceivedReceipt(window.Id, damage.Id, frame.InstructionIndex, context.SourceSeat, sourceHp, damage.Amount, before, checked(before + 1));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheShennuReceived = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheShennuReceivedEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), receipt));
        _coreStrategicProgramsOperations.SetStrategicMarker(frame.OwnerSeat, PlayerMarkerKind.ShanheSourceNu, frame.OwnerSeat, receipt.NuAfter, frame);
        if (sourceHp > 0)
            _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, sourceHp, null, null, SkillProgramCardSetVisibility.Private, new(ShanheSourceShennuDrawReason));
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 62048)]
    internal int ShanheShennuDamageBonus(GameEngine.IDamageAttempt attack) => IsShanheSourceShennuMode && !attack.IsSourceLess && !attack.IsChainPropagation && !attack.DamageRedirected && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], ShanheSourceShennuSkill) ? Math.Max(0, _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Markers.GetValueOrDefault(PlayerMarkerKind.ShanheSourceNu)) : 0;
    internal int ShanheShennuPublicDamageBonus(GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, GameEngine.IDamageAttempt? attack)
    {
        if (!IsShanheSourceShennuMode || sourceLess || chain || attack?.DamageRedirected == true)
            return 0;
        var source = view.Players.Single(player => player.Seat == sourceSeat);
        return source.IsAlive && CoreGameEnginePublicDamagePredictionRuntime.VisiblePredictionSkillIds(source).Contains(ShanheSourceShennuSkill) ? Math.Max(0, source.Markers?.SingleOrDefault(marker => marker.Kind == PlayerMarkerKind.ShanheSourceNu)?.Count ?? 0) : 0;
    }

    // Issued for every real outgoing damage, even at zero Nu: self-damage or
    // nested retaliation can earn Nu before this exact damage finishes.
    // Capture before Wenyi (10) or another paid-health hook can pause and
    // return directly to damage windows without revisiting later hooks.
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 1)]
    internal bool CaptureShanheShennuOutgoing(GameEngine.IDamageAttempt attack, long damageId)
    {
        if (!IsShanheSourceShennuMode || attack.IsSourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], ShanheSourceShennuSkill))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.Id != damageId || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != attack.SourceSeat || damage.TargetSeat != attack.TargetSeat || damage.Amount <= 0 || damage.ShanheShennuOutgoing is not null)
            throw new InvalidOperationException("Source Shennu outgoing receipt lost its original paid damage.");
        var receipt = new ShanheShennuOutgoingReceipt(attack.ResolutionId, new(ShanheSourceShennuSkill, "outgoing-damage", attack.SourceSeat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], ShanheSourceShennuSkill)), _contentRegistryOperations._contentRegistry.GetSkill(ShanheSourceShennuSkill).Program!.GameplayHash, attack.TargetSeat, damage.Amount);
        _coreRuntimeOperations.ReplaceRuntimeTop(damage with { ShanheShennuOutgoing = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheShennuOutgoingIssuedEvent(damage.Id, receipt));
        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62048)]
    internal void CompleteShanheShennuOutgoing(AfterDamageEvent completed)
    {
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == completed.ResolutionId)is not DamageFrame { ShanheShennuOutgoing: { } receipt } damage)
            return;
        if (receipt.Cleared || completed.SourceLess || completed.SourceSeat != receipt.Source.OwnerSeat || damage.SourceSeat != completed.SourceSeat || damage.TargetSeat != completed.TargetSeat || receipt.TargetSeat != completed.TargetSeat || damage.Amount != completed.Amount || receipt.DamageAmount != completed.Amount || damage.ParentFrameId != receipt.AttackResolutionId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheShennuOutgoingIssuedEvent>().Count(fact => fact.DamageFrameId == damage.Id && fact.Receipt == receipt) != 1)
            throw new InvalidOperationException("Source Shennu completion cannot change source, attack or paid damage identity.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(damage.Id, damage with { ShanheShennuOutgoing = receipt with { Cleared = true } });
        var owner = _coreMatchParticipantsPlayersAccess._players[receipt.Source.OwnerSeat];
        var count = owner.Markers.GetValueOrDefault(PlayerMarkerKind.ShanheSourceNu);
        owner.Markers.Remove(PlayerMarkerKind.ShanheSourceNu);
        foreach (var key in owner.MarkerSourceCounts.Keys.Where(key => key.Marker == PlayerMarkerKind.ShanheSourceNu).ToArray())
            owner.MarkerSourceCounts.Remove(key);
        if (count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(damage.Id, owner.Seat, PlayerMarkerKind.ShanheSourceNu, -count, 0, owner.Seat, "skill-program.shanhe-source-shennu-14214.after-damage"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheShennuClearedEvent(damage.Id, receipt.AttackResolutionId, owner.Seat, count));
    }
}
