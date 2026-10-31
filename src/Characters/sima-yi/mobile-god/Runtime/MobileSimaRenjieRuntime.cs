namespace CardGame.Core;

[ProgramRuntimeModule]
internal sealed class MobileSimaRenjieRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IMobileSimaRenjieFacts facts, IMobileSimaRenjieOperations operations)
{
    private CardUseFrame? LifecycleCardUse(long id) => frames.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == id);
    private void UpdateLifecycleCardUse(long id, Func<CardUseFrame, CardUseFrame> update)
    {
        var use = LifecycleCardUse(id) ?? throw new InvalidOperationException("The card-use lifecycle owner is missing.");
        frames.Replace(id, update(use));
    }
    private const string MobileSimaRenjie = "mobile-god:renjie";
    private bool HasMobileSimaRenjie => facts.SkillInstalled(MobileSimaRenjie);
    private int MobileSimaSuccessfulResponses(MobileSimaAttack attack, CardKind kind) => kind == CardKind.Dodge
        ? attack.SuccessfulDodgeResponses : facts.ActiveDuel is { } duel && duel.ResolutionId == attack.ResolutionId
            ? duel.SuccessfulSlashResponses : 0;
    internal void MarkMobileSimaRenjieResponse(MobileSimaAttack attack, int responderSeat, CardKind requiredKind)
    {
        if (!HasMobileSimaRenjie || !facts.HasSkill(responderSeat, MobileSimaRenjie) ||
            LifecycleCardUse(attack.ResolutionId) is not { } use || use.SourceSeat == responderSeat || !facts.Player(responderSeat).IsAlive) return;
        var progress = MobileSimaSuccessfulResponses(attack, requiredKind);
        if (use.MobileSimaResponse is { Settled: false } old && old.ResponderSeat == responderSeat && old.RequiredKind == requiredKind &&
            old.TargetIndex == use.TargetIndex && old.SuccessfulResponses == progress) return;
        var r = new MobileSimaResponseRequirement(frames.NextId(), use.Id, responderSeat, requiredKind, use.TargetIndex, progress);
        UpdateLifecycleCardUse(use.Id, current => current with { MobileSimaResponse = r });
        events.AdvanceFact(new MobileSimaResponseRequiredEvent(r));
    }
    internal void ClearMobileSimaRenjieResponse(MobileSimaAttack attack)
    {
        if (HasMobileSimaRenjie && LifecycleCardUse(attack.ResolutionId)?.MobileSimaResponse is not null)
            UpdateLifecycleCardUse(attack.ResolutionId, use => use with { MobileSimaResponse = null });
    }
    internal bool TryBeginMobileSimaRenjieFailedAttack(MobileSimaAttack attack)
    {
        if (!HasMobileSimaRenjie || attack.IsChainPropagation || LifecycleCardUse(attack.ResolutionId) is not { MobileSimaResponse: { Settled: false } r } use ||
            r.ResponderSeat != attack.TargetSeat || r.TargetIndex != use.TargetIndex ||
            r.SuccessfulResponses != MobileSimaSuccessfulResponses(attack, r.RequiredKind)) return false;
        if (frames.Top is not { } parent || parent.Id != use.Id &&
            (parent is not ResponseWindowFrame response || response.ParentFrameId != use.Id))
            throw new InvalidOperationException("An unanswered native attack lost its original response host.");
        if (!CanBeginMobileSimaRenjie(r.ResponderSeat))
        { UpdateLifecycleCardUse(use.Id, current => current with { MobileSimaResponse = r with { Settled = true } }); return false; }
        return BeginMobileSimaRenjie(parent, use.Id, r.ResponderSeat, MobileSimaRenjieReturnKind.FailedAttack,
            "required:" + r.Id, requirement: r.Id);
    }
    internal bool TryBeginMobileSimaRenjieNullification(NullificationWindowFrame supplied, bool explicitPass)
    {
        if (!HasMobileSimaRenjie || frames.Top is not NullificationWindowFrame f || f.Id != supplied.Id ||
            f.CandidateIndex < 0 || f.CandidateIndex >= f.CandidateSeats.Count) return false;
        var actor = f.CandidateSeats[f.CandidateIndex];
        var source = f.ChainDepth == 0 ? f.SourceSeat : facts.History<NullificationRespondedEvent>()
            .LastOrDefault(e => e.ResolutionId == f.ParentFrameId && e.ChainDepth == f.ChainDepth)?.ResponderSeat ?? f.SourceSeat;
        if (f.DelayedReturn is null && source == actor || !CanBeginMobileSimaRenjie(actor) ||
            facts.WenjiNullificationForbidden(f, actor) || facts.FuqiNullificationBanned(actor) ||
            f.ChainDepth == 0 && facts.NearbyResponseProhibited(f.SourceSeat, actor, f.EffectCardKind, f.TargetSeats)) return false;
        if (explicitPass && prompts.Current is { } p && (p.Kind != DecisionKind.Nullification || p.PlayerSeat != actor))
            throw new InvalidOperationException("Renjie cannot replace another accepted response decision.");
        prompts.Clear();
        return BeginMobileSimaRenjie(f, f.ParentFrameId, actor, MobileSimaRenjieReturnKind.Counterspell,
            $"counterspell:{f.Id}:{f.CandidateIndex}:{f.ChainDepth}", f.CandidateIndex, f.ChainDepth);
    }
    internal bool TryBeginMobileSimaRenjieBorrowedSword(MobileSimaSword pending)
    {
        var owner = facts.Player(pending.WeaponOwnerSeat);
        if (!HasMobileSimaRenjie || !owner.IsAlive || pending.SourceSeat == owner.Seat || !CanBeginMobileSimaRenjie(owner.Seat) ||
            facts.UseUnrespondable(pending.ResolutionId) || facts.YingboUnrespondable(pending.ResolutionId) ||
            facts.WenjiResponseForbidden(pending.ResolutionId, owner.Seat) ||
            !facts.LegalBorrowedSwordTarget(owner.Seat, pending.SlashTargetSeat) ||
            !facts.Equipment(owner.Seat).Any(c => EquipmentCatalog.Get(c.Kind).Slot == EquipmentSlot.Weapon) ||
            LifecycleCardUse(pending.ResolutionId) is not { } use) return false;
        var key = $"borrowed:{use.Id}:{use.TargetIndex}:{facts.ExtraEffectOrdinal(use.Id)}";
        if (facts.History<MobileSimaRenjieStartedEvent>().Any(e => e.OccurrenceKey == key)) return false;
        if (frames.Top?.Id != use.Id) throw new InvalidOperationException("Borrowed Sword's missing use lost its native owner.");
        prompts.Clear();
        return BeginMobileSimaRenjie(use, use.Id, owner.Seat, MobileSimaRenjieReturnKind.BorrowedSword, key);
    }
    private bool CanBeginMobileSimaRenjie(int actor) => facts.Winner == Winner.None && facts.Player(actor).IsAlive &&
        facts.HasSkill(actor, MobileSimaRenjie) &&
        facts.History<MobileSimaRenjieStartedEvent>().Count(e => e.Source.OwnerSeat == actor && e.RoundNumber == facts.RoundNumber) < 4;
    internal bool TryBeginMobileSimaRenjieQizheng(CardUseFrame use)
    {
        if (!HasMobileSimaRenjie || use.Qizheng is not { Stage: QizhengStage.Response, ResponseKind: null } q ||
            use.SourceSeat == q.Target || !CanBeginMobileSimaRenjie(q.Target) || facts.WenjiResponseForbidden(use.Id, q.Target) ||
            facts.UseUnrespondable(use.Id) || facts.YingboUnrespondable(use.Id)) return false;
        var key = $"qizheng:{use.Id}:{q.NullificationId}:{q.TargetIndex}:{q.EffectOrdinal}";
        if (facts.History<MobileSimaRenjieStartedEvent>().Any(e => e.OccurrenceKey == key)) return false;
        if (frames.Top?.Id != use.Id) throw new InvalidOperationException("Qizheng's unanswered response lost its original owner.");
        prompts.Clear();
        return BeginMobileSimaRenjie(use, use.Id, q.Target, MobileSimaRenjieReturnKind.Qizheng, key);
    }
    private bool BeginMobileSimaRenjie(ResolutionFrame parent, long useId, int actor, MobileSimaRenjieReturnKind kind,
        string key, int candidate = -1, int depth = -1, long requirement = 0)
    {
        if (prompts.Current is not null || frames.Top?.Id != parent.Id)
            throw new InvalidOperationException("Renjie requires one accepted, unanswered native occurrence.");
        var grant = facts.Grants(actor).First(g => g.SkillId == MobileSimaRenjie && g.IsEnabled);
        var f = new MobileSimaRenjieFrame(frames.NextId(), parent.Id, useId,
            new(MobileSimaRenjie, "unanswered-native", actor, grant.SkillInstanceId), facts.ContentHash,
            kind, key, facts.RoundNumber, candidate, depth, requirement);
        frames.Push(f);
        events.AdvanceFact(new MobileSimaRenjieStartedEvent(f.Id, parent.Id, useId, f.Source, kind, key, f.RoundNumber));
        frames.Advance(f.Id); return true;
    }
    [NativeFrameRoute(typeof(MobileSimaRenjieFrame), 40425)]
    private void ContinueMobileSimaRenjie(MobileSimaRenjieFrame supplied)
    {
        if (frames.Top is not MobileSimaRenjieFrame f || f.Id != supplied.Id || prompts.Current is not null) return;
        AssertMobileSimaRenjie(f);
        if (!f.Applied)
        {
            if (operations.BeginInvocation(f.Id, f.Source.OwnerSeat, f.Source.SkillId, f.OccurrenceKey, f.Source.SkillInstanceId)) return;
            f = (MobileSimaRenjieFrame)frames.Top!; var owner = facts.Player(f.Source.OwnerSeat);
            var gained = owner.IsAlive && facts.Winner == Winner.None;
            frames.ReplaceTop(f = f with { Applied = true });
            if (gained)
            {
                operations.SetMarkerSourceCount(owner.Seat, PlayerMarkerKind.Ren, owner.Seat,
                    facts.MarkerSourceCount(owner.Seat, PlayerMarkerKind.Ren, owner.Seat) + 1);
                operations.SetMarkerCount(owner.Seat, PlayerMarkerKind.Ren, facts.MarkerCount(owner.Seat, PlayerMarkerKind.Ren) + 1);
                events.AdvanceFact(new PlayerMarkerChangedEvent(f.Id, owner.Seat, PlayerMarkerKind.Ren, 1,
                    facts.MarkerCount(owner.Seat, PlayerMarkerKind.Ren), owner.Seat, "skill.mobile-god.renjie.unanswered"));
            }
            events.AdvanceFact(new MobileSimaRenjieAppliedEvent(f.Id, owner.Seat, f.RoundNumber, gained));
        }
        AssertMobileSimaRenjie(f);
        frames.Pop(f.Id, ResolutionFrameKind.MobileSimaRenjie);
        if (f.ReturnKind == MobileSimaRenjieReturnKind.Counterspell)
        {
            var parent = (NullificationWindowFrame)frames.Top!;
            operations.ContinueNullification(parent with { CandidateIndex = parent.CandidateIndex + 1 }); return;
        }
        if (f.ReturnKind == MobileSimaRenjieReturnKind.Qizheng)
        { operations.ResolveQizheng(f.UseFrameId); return; }
        if (f.ReturnKind == MobileSimaRenjieReturnKind.BorrowedSword)
        { operations.CompleteBorrowedSword(f.UseFrameId, true); return; }
        var use = LifecycleCardUse(f.UseFrameId)!;
        UpdateLifecycleCardUse(use.Id, current => current with { MobileSimaResponse = current.MobileSimaResponse! with { Settled = true } });
        var attack = facts.ActiveAttack ?? throw new InvalidOperationException("Renjie lost its exact native attack return.");
        if (!operations.ApplyDamage(attack.ResolutionId)) operations.CompleteAttack(attack.ResolutionId);
    }
    private bool MobileSimaRenjieParentMatches(ResolutionFrame parent, MobileSimaRenjieFrame f)
    {
        if (f.ParentFrameId != parent.Id) return false;
        if (f.ReturnKind == MobileSimaRenjieReturnKind.Counterspell)
            return parent is NullificationWindowFrame n && facts.NullificationEffectOwner(n) && n.ParentFrameId == f.UseFrameId && n.CandidateIndex == f.CandidateIndex && n.ChainDepth == f.ChainDepth &&
                n.CandidateSeats.ElementAtOrDefault(n.CandidateIndex) == f.Source.OwnerSeat && f.OccurrenceKey == $"counterspell:{n.Id}:{n.CandidateIndex}:{n.ChainDepth}";
        if (LifecycleCardUse(f.UseFrameId) is not { } use) return false;
        if (f.ReturnKind == MobileSimaRenjieReturnKind.Qizheng)
            return parent.Id == use.Id && use.Qizheng is { Stage: QizhengStage.Response, ResponseKind: null } q && q.Target == f.Source.OwnerSeat &&
                f.OccurrenceKey == $"qizheng:{use.Id}:{q.NullificationId}:{q.TargetIndex}:{q.EffectOrdinal}";
        if (f.ReturnKind == MobileSimaRenjieReturnKind.BorrowedSword)
            return parent.Id == use.Id && facts.ActiveSword is { } sword && sword.ResolutionId == use.Id && sword.WeaponOwnerSeat == f.Source.OwnerSeat &&
                f.OccurrenceKey == $"borrowed:{use.Id}:{use.TargetIndex}:{facts.ExtraEffectOrdinal(use.Id)}";
        return (parent.Id == use.Id || parent is ResponseWindowFrame response && response.ParentFrameId == use.Id) &&
            use.MobileSimaResponse is { Settled: false } r && r.Id == f.RequirementId && r.ResponderSeat == f.Source.OwnerSeat && r.TargetIndex == use.TargetIndex &&
            facts.ActiveAttack is { } attack && attack.ResolutionId == use.Id && attack.TargetSeat == r.ResponderSeat &&
            r.SuccessfulResponses == MobileSimaSuccessfulResponses(attack, r.RequiredKind) && f.OccurrenceKey == "required:" + r.Id &&
            facts.History<MobileSimaResponseRequiredEvent>().Count(e => e.Requirement == r) == 1;
    }
    private void AssertMobileSimaRenjie(MobileSimaRenjieFrame f)
    {
        var parent = frames.ParentOf(f.Id);
        if (parent is null || f.Source.SkillId != MobileSimaRenjie || f.ContentHash != facts.ContentHash ||
            f.RoundNumber != facts.RoundNumber || !MobileSimaRenjieParentMatches(parent, f) ||
            facts.History<MobileSimaRenjieStartedEvent>().Count(e => e == new MobileSimaRenjieStartedEvent(f.Id, f.ParentFrameId, f.UseFrameId,
                f.Source, f.ReturnKind, f.OccurrenceKey, f.RoundNumber)) != 1 ||
            facts.History<MobileSimaRenjieAppliedEvent>().Count(e => e.FrameId == f.Id) != (f.Applied ? 1 : 0))
            throw new InvalidOperationException("Renjie lost its exact unanswered native cursor, real invocation or once-only marker result.");
    }
    internal bool MobileSimaRenjieStructuralEdge(ResolutionFrame parent, ResolutionFrame child) =>
        child is MobileSimaRenjieFrame f && MobileSimaRenjieParentMatches(parent, f) &&
        facts.History<MobileSimaRenjieStartedEvent>().Count(e => e.FrameId == f.Id && e.ParentFrameId == parent.Id && e.OccurrenceKey == f.OccurrenceKey) == 1;
    internal bool HasMobileSimaRenjieNativeSubtree(long useId)
    {
        var root = frames.OfType<MobileSimaRenjieFrame>().LastOrDefault(frame => frame.UseFrameId == useId);
        return root is not null && frames.ParentOf(root.Id) is { } parent && MobileSimaRenjieStructuralEdge(parent, root) &&
            (frames.ChildOf(root.Id) is not { } child || child is SkillInvocationFrame invocation &&
                facts.InvocationStructuralEdge(root, invocation) && facts.InvocationObserverSuffix(invocation.Id));
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 40425)]
    private bool TryAdvanceMobileSimaRenjieSubtree()
    {
        if (prompts.Current is not null || frames.OfType<MobileSimaRenjieFrame>().LastOrDefault() is not { } root ||
            !HasMobileSimaRenjieNativeSubtree(root.UseFrameId)) return false;
        if (frames.Top?.Id == root.Id) { frames.Advance(root.Id); return true; }
        return operations.AdvanceObserverSubtree();
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 40425)]
    private void AssertMobileSimaRenjieFrames()
    { foreach (var f in frames.OfType<MobileSimaRenjieFrame>()) AssertMobileSimaRenjie(f); }
}
