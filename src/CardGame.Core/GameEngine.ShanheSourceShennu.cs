using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSourceShennuSkill = "shanhe:source-shennu-14214";
    private const string ShanheSourceShennuDrawReason = "skill-program.shanhe-source-shennu-14214.draw";
    private bool IsShanheSourceShennuMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;
    [ProgramCapability]
    private sealed class ShanheSourceShennuCapability(GameEngine engine) : IShanheSourceShennuHost
    { public void ReceiveDamage(ProgramSkillFrame frame) => engine.ReceiveShanheSourceShennuDamage(frame); }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62048, SkillProgramEffectOp.ResolveShanheSourceShennu)]
    private bool CanReceiveShanheSourceShennuDamage(ProgramTriggerLegalityContext context) => IsShanheSourceShennuMode &&
        context.WindowContext is { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying, Amount: > 0 } window &&
        window.TargetSeat == context.OwnerSeat;

    private void ReceiveShanheSourceShennuDamage(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourceShennuMode || frame.SkillId != ShanheSourceShennuSkill || frame.ShanheShennuReceived is not null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying, Amount: > 0 } context ||
            context.TargetSeat != frame.OwnerSeat || _resolutionStack.Count < 3 ||
            _resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId ||
            window.ParentFrameId != context.DamageFrameId || window.TriggerWindow != context.Window ||
            _resolutionStack[^3] is not DamageFrame damage || damage.Id != window.ParentFrameId ||
            damage.TargetSeat != frame.OwnerSeat || damage.Amount != context.Amount ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Source Shennu lost its exact positive applied-damage owner.");
        var attack = GetDamageTriggerAttack(window);
        if (attack.ResolutionId != damage.ParentFrameId || attack.TargetSeat != damage.TargetSeat ||
            attack.SourceSeat != damage.SourceSeat || context.SourceSeat != (attack.IsSourceLess ? null : attack.SourceSeat))
            throw new InvalidOperationException("Source Shennu cannot borrow another damage's source or turn owner.");
        var sourceHp = context.SourceSeat is { } source && IsValidPlayerSeat(source) && _players[source].HasHpValue
            ? Math.Max(0, _players[source].Hp) : 0;
        var before = _players[frame.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.ShanheSourceNu);
        var receipt = new ShanheShennuReceivedReceipt(window.Id, damage.Id, frame.InstructionIndex,
            context.SourceSeat, sourceHp, damage.Amount, before, checked(before + 1));
        ReplaceRuntimeTop(frame = frame with { ShanheShennuReceived = receipt });
        AdvanceEventRulesAndQueueFact(new ShanheShennuReceivedEvent(frame.Id,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), receipt));
        SetStrategicMarker(frame.OwnerSeat, PlayerMarkerKind.ShanheSourceNu, frame.OwnerSeat, receipt.NuAfter, frame);
        if (sourceHp > 0) DrawProgramCards(frame.Id, frame.OwnerSeat, sourceHp, null, null,
            SkillProgramCardSetVisibility.Private, new(ShanheSourceShennuDrawReason));
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 62048)]
    private int ShanheShennuDamageBonus(IDamageAttempt attack) =>
        IsShanheSourceShennuMode && !attack.IsSourceLess && !attack.IsChainPropagation && !attack.DamageRedirected &&
        IsValidPlayerSeat(attack.SourceSeat) && HasRuntimeSkill(_players[attack.SourceSeat], ShanheSourceShennuSkill)
            ? Math.Max(0, _players[attack.SourceSeat].Markers.GetValueOrDefault(PlayerMarkerKind.ShanheSourceNu)) : 0;

    private int ShanheShennuPublicDamageBonus(GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, IDamageAttempt? attack)
    {
        if (!IsShanheSourceShennuMode || sourceLess || chain || attack?.DamageRedirected == true) return 0;
        var source = view.Players.Single(player => player.Seat == sourceSeat);
        return source.IsAlive && VisiblePredictionSkillIds(source).Contains(ShanheSourceShennuSkill)
            ? Math.Max(0, source.Markers?.SingleOrDefault(marker => marker.Kind == PlayerMarkerKind.ShanheSourceNu)?.Count ?? 0) : 0;
    }

    // Issued for every real outgoing damage, even at zero Nu: self-damage or
    // nested retaliation can earn Nu before this exact damage finishes.
    // Capture before Wenyi (10) or another paid-health hook can pause and
    // return directly to damage windows without revisiting later hooks.
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 1)]
    private bool CaptureShanheShennuOutgoing(IDamageAttempt attack, long damageId)
    {
        if (!IsShanheSourceShennuMode || attack.IsSourceLess || !IsValidPlayerSeat(attack.SourceSeat) ||
            !HasRuntimeSkill(_players[attack.SourceSeat], ShanheSourceShennuSkill)) return false;
        if (_resolutionStack.LastOrDefault() is not DamageFrame damage || damage.Id != damageId ||
            damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != attack.SourceSeat ||
            damage.TargetSeat != attack.TargetSeat || damage.Amount <= 0 || damage.ShanheShennuOutgoing is not null)
            throw new InvalidOperationException("Source Shennu outgoing receipt lost its original paid damage.");
        var receipt = new ShanheShennuOutgoingReceipt(attack.ResolutionId,
            new(ShanheSourceShennuSkill, "outgoing-damage", attack.SourceSeat,
                GetRuntimeSkillInstanceId(_players[attack.SourceSeat], ShanheSourceShennuSkill)),
            _contentRegistry.GetSkill(ShanheSourceShennuSkill).Program!.GameplayHash, attack.TargetSeat, damage.Amount);
        ReplaceRuntimeTop(damage with { ShanheShennuOutgoing = receipt });
        AdvanceEventRulesAndQueueFact(new ShanheShennuOutgoingIssuedEvent(damage.Id, receipt));
        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62048)]
    private void CompleteShanheShennuOutgoing(AfterDamageEvent completed)
    {
        if (_resolutionStack.SingleOrDefault(item => item.Id == completed.ResolutionId) is not DamageFrame
            { ShanheShennuOutgoing: { } receipt } damage) return;
        if (receipt.Cleared || completed.SourceLess || completed.SourceSeat != receipt.Source.OwnerSeat ||
            damage.SourceSeat != completed.SourceSeat || damage.TargetSeat != completed.TargetSeat ||
            receipt.TargetSeat != completed.TargetSeat || damage.Amount != completed.Amount || receipt.DamageAmount != completed.Amount ||
            damage.ParentFrameId != receipt.AttackResolutionId ||
            ProgramEventHistory<ShanheShennuOutgoingIssuedEvent>().Count(fact => fact.DamageFrameId == damage.Id && fact.Receipt == receipt) != 1)
            throw new InvalidOperationException("Source Shennu completion cannot change source, attack or paid damage identity.");
        ReplaceRuntimeFrame(damage.Id, damage with { ShanheShennuOutgoing = receipt with { Cleared = true } });
        var owner = _players[receipt.Source.OwnerSeat];
        var count = owner.Markers.GetValueOrDefault(PlayerMarkerKind.ShanheSourceNu);
        owner.Markers.Remove(PlayerMarkerKind.ShanheSourceNu);
        foreach (var key in owner.MarkerSourceCounts.Keys.Where(key => key.Marker == PlayerMarkerKind.ShanheSourceNu).ToArray())
            owner.MarkerSourceCounts.Remove(key);
        if (count > 0) AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(damage.Id, owner.Seat,
            PlayerMarkerKind.ShanheSourceNu, -count, 0, owner.Seat, "skill-program.shanhe-source-shennu-14214.after-damage"));
        AdvanceEventRulesAndQueueFact(new ShanheShennuClearedEvent(damage.Id, receipt.AttackResolutionId, owner.Seat, count));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62048)]
    private void AssertShanheShennuReceived(ProgramSkillFrame frame)
    {
        if (frame.ShanheShennuReceived is not { } receipt) return;
        if (!IsShanheSourceShennuMode || frame.SkillId != ShanheSourceShennuSkill || receipt.InstructionIndex != frame.InstructionIndex ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying } context ||
            receipt.DamageWindowId != context.ParentFrameId || receipt.DamageFrameId != context.DamageFrameId ||
            receipt.SourceSeat != context.SourceSeat || context.TargetSeat != frame.OwnerSeat || receipt.DamageAmount != context.Amount ||
            receipt.SourceHp < 0 || receipt.NuBefore < 0 || receipt.NuAfter != checked(receipt.NuBefore + 1) ||
            ProgramEventHistory<ShanheShennuReceivedEvent>().Count(fact => fact.ProgramFrameId == frame.Id && fact.Receipt == receipt &&
                fact.Source == new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId)) != 1)
            throw new InvalidOperationException("Source Shennu lost its frozen source HP and one-Nu applied-damage receipt.");
    }
}
