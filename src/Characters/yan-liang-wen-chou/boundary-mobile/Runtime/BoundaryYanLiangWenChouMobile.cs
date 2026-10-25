using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record BoundaryYanLiangWenChouClaimReceipt(CardConversionSource Source, string GameplayHash,
    long WindowId, long DamageId, long UseId, long ActionId, int InstructionIndex, int OccurrenceIndex,
    IReadOnlyList<int> CardIds, int Before, int After)
{
    private readonly IReadOnlyList<int> _cardIds = Array.AsReadOnly(CardIds.ToArray());
    public IReadOnlyList<int> CardIds { get => _cardIds; init => _cardIds = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryYanLiangWenChouClaimReceipt? BoundaryYanLiangWenChouClaim { get; init; }
}
// The response entities are public already; one scalar fact per actual gain
// also avoids publishing a mutable or private collection.
public sealed record BoundaryYanLiangWenChouResponseClaimedEvent(long ProgramFrameId, long UseId, long ActionId,
    int OwnerSeat, int CardId, int ClaimIndex, int MovementSequence) : IGameEvent;

internal interface IBoundaryYanLiangWenChouHost
{ SkillProgramStepOutcome Claim(ProgramSkillFrame frame); }
internal sealed class ClaimBoundaryYanLiangWenChouDuelResponsesDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses;
    public override ISkillProgramEffectHandler Handler { get; } = new ClaimBoundaryYanLiangWenChouDuelResponsesHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(8d));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class ClaimBoundaryYanLiangWenChouDuelResponsesHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int t, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryYanLiangWenChouHost>().Claim(f);
}
internal static class BoundaryYanLiangWenChouComposition
{
    [ProgramCatalogValidation(65710)]
    private static void Validate(string path, SkillProgram p)
    {
        foreach (var t in p.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses)))
            if (p.Id != "boundary:shuangxiong-mobile" || t.Window != SkillProgramTriggerWindow.AfterDamageApplied ||
                t.Subject != SkillProgramTriggerSubject.Owner || !t.Optional || t.DamageOccurrence != SkillProgramDamageOccurrence.PerDamage ||
                t.Effects is not [{ Op: SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses }])
                throw new InvalidOperationException(path + ": mobile Shuangxiong requires one optional original Duel damage-recipient response claim.");
    }
}

public sealed partial class GameEngine
{
    private const string BoundaryYanLiangWenChouSkill = "boundary:shuangxiong-mobile";
    private const string BoundaryYanLiangWenChouClaimReason = "skill.boundary-yan-liang-wen-chou.shuangxiong.claim";
    [ProgramCapability]
    private sealed class BoundaryYanLiangWenChouCapability(GameEngine engine) : IBoundaryYanLiangWenChouHost
    { public SkillProgramStepOutcome Claim(ProgramSkillFrame f) => engine.BeginBoundaryYanLiangWenChouClaim(f); }

    private bool BoundaryYanLiangWenChouOriginalDuel(int owner, string instance, ProgramSkillWindowContext context, out CardUseFrame use)
    {
        use = null!;
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } || context.TargetSeat != owner ||
            _resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is not { } damage ||
            damage.TriggerWindow != context.Window || damage.TargetSeat != owner || damage.ParentFrameId != context.DamageFrameId ||
            _resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damage.ParentFrameId) is not { } applied ||
            LastDamageSourceOriginalAttack(applied.ParentFrameId) is not { } attack)
            return false;
        if (attack.IsSourceLess || attack.IsChainPropagation || attack.TargetSeat != owner || attack.SourceSeat == owner ||
            attack.EffectiveCardKind != CardKind.Duel || LifecycleCardUse(attack.ResolutionId) is not { CardKind: CardKind.Duel, Action: { } action } current ||
            current.SourceSeat != owner || action.ActorSeat != owner || action.EffectiveKind != CardKind.Duel ||
            !action.ConversionChain.Any(s => s.SkillId == BoundaryYanLiangWenChouSkill && s.OwnerSeat == owner && s.SkillInstanceId == instance &&
                s.BindingId == "reveal-two-instead-of-drawing.turn-2") ||
            !ProgramEventHistory<DuelResponseEvent>().Any(e => e.ResolutionId == current.Id && e.ResponderSeat == owner && !e.UsedSlash))
            return false;
        use = current; return true;
    }

    private IReadOnlyList<int> BoundaryYanLiangWenChouResponseEntities(CardUseFrame use, int owner) =>
        ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action)
            .Where(a => a.Type == CardActionType.Response && a.ParentActionId == use.Action!.ActionId && a.ActorSeat != owner &&
                a.ResponderSeat == a.ActorSeat && IsSlashCard(a.EffectiveKind) &&
                ProgramEventHistory<DuelResponseEvent>().Any(e => e.ResolutionId == use.Id && e.ResponderSeat == a.ActorSeat && e.UsedSlash))
            .SelectMany(a => a.PhysicalCards).Select(c => c.CardId).Distinct()
            .Where(id => id > 0 && _cardZones.GetLocation(id) == CardLocation.DiscardPile).ToArray();

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 65710, SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses)]
    private bool OfferBoundaryYanLiangWenChouClaim(ProgramTriggerLegalityContext c) =>
        BoundaryYanLiangWenChouOriginalDuel(c.OwnerSeat, c.Candidate.SkillInstanceId, c.Context, out var use) &&
        BoundaryYanLiangWenChouResponseEntities(use, c.OwnerSeat).Count > 0;

    private SkillProgramStepOutcome BeginBoundaryYanLiangWenChouClaim(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != BoundaryYanLiangWenChouSkill || f.InstructionIndex != 1 || f.BoundaryYanLiangWenChouClaim is not null ||
            f.WindowContext is not { } context || !BoundaryYanLiangWenChouOriginalDuel(f.OwnerSeat, f.SkillInstanceId, context, out var use))
            throw new InvalidOperationException("Shuangxiong lost its original converted Duel and actual recipient-damage window.");
        var ids = BoundaryYanLiangWenChouResponseEntities(use, f.OwnerSeat);
        if (ids.Count == 0) return SkillProgramStepOutcome.Continue;
        var before = _movementSequence;
        var r = new BoundaryYanLiangWenChouClaimReceipt(new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash,
            context.ParentFrameId, context.DamageFrameId!.Value, use.Id, use.Action!.ActionId, f.InstructionIndex, context.OccurrenceIndex,
            ids, before, before + ids.Count);
        ReplaceRuntimeTop(f with { BoundaryYanLiangWenChouClaim = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var cards = ids.Select(id => _cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == id)).ToArray();
        var index = 0;
        MoveCards(cards, CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new(BoundaryYanLiangWenChouClaimReason), beforeFact: m =>
        {
            var i = index++;
            if (m.CardId != r.CardIds[i]) throw new InvalidOperationException("Shuangxiong's frozen response gain order changed.");
            AdvanceEventRulesAndQueueFact(new BoundaryYanLiangWenChouResponseClaimedEvent(f.Id, use.Id, use.Action.ActionId, f.OwnerSeat, m.CardId, i, m.Sequence));
        }, emitCardMovedEvents: false);
        ContinueBoundaryYanLiangWenChouClaim(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool ValidBoundaryYanLiangWenChouClaim(ProgramSkillFrame f)
    {
        if (f.BoundaryYanLiangWenChouClaim is not { } r || f.SkillId != BoundaryYanLiangWenChouSkill || f.InstructionIndex != 1 || r.InstructionIndex != 1 ||
            f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.DamageFrameId != r.DamageId || c.OccurrenceIndex != r.OccurrenceIndex ||
            r.Source != new CardConversionSource(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash ||
            !BoundaryYanLiangWenChouOriginalDuel(f.OwnerSeat, f.SkillInstanceId, c, out var use) || use.Id != r.UseId || use.Action!.ActionId != r.ActionId ||
            r.CardIds.Count == 0 || r.CardIds.Distinct().Count() != r.CardIds.Count || r.CardIds.Any(id => id <= 0) || r.Before < 0 || r.After != r.Before + r.CardIds.Count)
            return false;
        var parent = _resolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == r.WindowId);
        if (parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            !MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].ToProgramCandidate())) return false;
        var claims = ProgramEventHistory<BoundaryYanLiangWenChouResponseClaimedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        return claims.Length == r.CardIds.Count && claims.Select(e => e.CardId).SequenceEqual(r.CardIds) &&
            !claims.Where((e, i) => e.UseId != r.UseId || e.ActionId != r.ActionId || e.OwnerSeat != f.OwnerSeat || e.ClaimIndex != i ||
                e.MovementSequence != r.Before + i + 1 || _cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId &&
                    m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryYanLiangWenChouClaimReason) != 1 ||
                !ProgramEventHistory<CardActionAcceptedEvent>().Any(a => a.Action.Type == CardActionType.Response &&
                    a.Action.ParentActionId == r.ActionId && a.Action.ActorSeat != f.OwnerSeat && IsSlashCard(a.Action.EffectiveKind) &&
                    a.Action.PhysicalCards.Any(p => p.CardId == e.CardId))).Any();
    }

    private void ContinueBoundaryYanLiangWenChouClaim(long id)
    {
        var f = GetActiveProgramFrame(id);
        if (!ValidBoundaryYanLiangWenChouClaim(f)) throw new InvalidOperationException("Shuangxiong lost its original response entities and committed gain receipt.");
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(id)) return;
        ReplaceRuntimeTop(f = GetActiveProgramFrame(id) with { PendingMovementContinuation = null });
        FinishProgramSkill(f, completed: true);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 65710)]
    private bool ProveBoundaryYanLiangWenChouClaimMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses || !ValidBoundaryYanLiangWenChouClaim(f) ||
            f.BoundaryYanLiangWenChouClaim is not { } r || pending != f.PendingMovementContinuation ||
            pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null) return false;
        // The native batch is the paid gain, even if a later observer moves an
        // entity again. Retain the original Duel/response invoice on this frame.
        return _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch =>
            batch.ParentFrameId == f.Id && batch.OriginOwnerSeat == f.OwnerSeat && batch.OriginSkillId == f.SkillId &&
            batch.OriginSkillInstanceId == f.SkillInstanceId && batch.Movements.Select(m => m.CardId).SequenceEqual(r.CardIds) &&
            batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DiscardPile &&
                m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryYanLiangWenChouClaimReason && _cardMovements.Contains(m)));
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 65710)]
    private bool ResumeBoundaryYanLiangWenChouClaim(long id)
    {
        if (GetActiveProgramFrame(id).BoundaryYanLiangWenChouClaim is null) return false;
        ContinueBoundaryYanLiangWenChouClaim(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 65710)]
    private bool ReturnBoundaryYanLiangWenChouClaim(ProgramSkillFrame f)
    {
        if (f.BoundaryYanLiangWenChouClaim is null) return false;
        if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat)
            throw new InvalidOperationException("Shuangxiong's actual gain child lost its typed original-frame return.");
        ContinueBoundaryYanLiangWenChouClaim(f.Id); return true;
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 65710)]
    private void AssertBoundaryYanLiangWenChouClaim(ProgramSkillFrame f)
    {
        if (f.BoundaryYanLiangWenChouClaim is not null && !ValidBoundaryYanLiangWenChouClaim(f))
            throw new InvalidOperationException("Shuangxiong's paid gain changed its original native Duel identity.");
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 65710)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 65710)]
    private bool KeepPaidBoundaryYanLiangWenChouClaim(ProgramSkillFrame f) => f.BoundaryYanLiangWenChouClaim is not null && ValidBoundaryYanLiangWenChouClaim(f);
}
