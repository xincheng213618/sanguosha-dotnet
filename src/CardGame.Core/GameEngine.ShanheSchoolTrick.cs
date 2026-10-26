using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Immediate native damage tricks. Borrowed Sword's separate Slash and a
    // delayed judgment are separate uses/effects and are not duplicated here.
    private static bool IsShanheSchoolDamageTrick(CardKind kind) =>
        IsOrdinaryTrick(kind) && (IsDamageCardKind(kind) || kind == CardKind.UnexpectedAssault);

    private bool TryGetShanheSchoolTrickUse(int owner, ProgramSkillWindowContext? context,
        out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null!; window = null!;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } identity } ||
            !HasShanheSchoolEffect(owner, 9) ||
            _resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId) is not ProgramCardTriggerWindowFrame w ||
            w.Action is not { Type: CardActionType.Use } action || action.ActorSeat != owner ||
            w.TrickContinuation is null || w.CompletedResponseReturn is not null ||
            LifecycleCardUse(w.ParentFrameId) is not { Action: { } actual, ShanheSchoolTrick: null } current ||
            current.SourceSeat != owner || actual.ActionId != action.ActionId ||
            identity.CardActionId != actual.ActionId || identity.ParentCardUseFrameId != current.Id ||
            current.CardKind != actual.EffectiveKind || !IsShanheSchoolDamageTrick(current.CardKind)) return false;
        use = current; window = w; return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62024, SkillProgramEffectOp.RepeatCurrentDamageTrick)]
    private bool CanOfferShanheSchoolTrick(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger,
        ProgramSkillWindowContext context) =>
        candidate.SkillId == ShanheSchoolEffects.TrickRepeatSkillId &&
        TryGetShanheSchoolTrickUse(candidate.OwnerSeat, context, out _, out _);

    [ProgramCapability]
    private sealed class ShanheSchoolTrickHost(GameEngine engine) : IShanheSchoolTrickHost
    {
        public void RepeatCurrentDamageTrick(ProgramSkillFrame frame) => engine.IssueShanheSchoolTrick(frame);
    }

    private void IssueShanheSchoolTrick(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheSchoolEffects.TrickRepeatSkillId || frame.InstructionIndex != 1 ||
            !TryGetShanheSchoolTrickUse(frame.OwnerSeat, frame.WindowContext, out var use, out var window))
            throw new InvalidOperationException("A school trick repeat requires its exact accepted original damage-trick use.");
        var source = new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var native = new ExtraCardEffectState { Source = source, ProgramFrameId = frame.Id,
            ActionId = use.Action!.ActionId, AutomaticGameplayHash = frame.GameplayHash, Trick = window.TrickContinuation };
        ReplaceRuntimeFrame(use.Id, use with { ShanheSchoolTrick =
            new(use.Action.ActionId, frame.Id, window.Id, frame.GameplayHash, native) });
        AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickIssuedEvent(use.Id, use.Action.ActionId, frame.Id,
            window.Id, source, frame.GameplayHash));
    }

    private bool HasUnstartedShanheSchoolTrick(long id) =>
        LifecycleCardUse(id) is { ShanheSchoolTrick: { Started: false } round, Action: { } action } use &&
        round.ActionId == action.ActionId && !IsNamedAmbushUseIneffective(use) && _winner == Winner.None;

    private void CompleteShanheSchoolTrick(long id)
    {
        if (LifecycleCardUse(id) is not { ShanheSchoolTrick: { Active: true } round } use ||
            HasRemainingSequentialTrickTargets(id) || HasRemainingAdjustedSimpleTargets(id)) return;
        ReplaceRuntimeFrame(id, use with { ShanheSchoolTrick = round with { Active = false } });
        AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickCompletedEvent(id, round.ActionId, round.ProgramFrameId));
    }

    private bool TryBeginShanheSchoolTrick(long id, bool afterAttack)
    {
        if (!HasUnstartedShanheSchoolTrick(id) ||
            LifecycleCardUse(id) is not { ShanheSchoolTrick: { } round, Action: { } action } use ||
            use.ExtraCardEffect is { WholeGroupRepeated: false } || HasUnstartedFeiYiExtraEffect(id) ||
            HasUnstartedZixiExtraEffect(id) || HasUnstartedMoQuanyuExtraRound(id) ||
            HasUnstartedMouBingcaiExtraEffect(id) || HasUnstartedSoleIncomingEffect(id) ||
            HasUnstartedMouHqRound(id) || HasUnstartedMouJjEffect(id) ||
            HasRemainingSequentialTrickTargets(id) || HasRemainingAdjustedSimpleTargets(id)) return false;
        if (_resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("A school extra effect cannot pass an unfinished native child.");
        var previous = NativeExtraEffectState(use);
        var native = round.Native with { WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? round.Native.MaterialClaims,
            FrozenGroupDamageSourceSeat = previous?.FrozenGroupDamageSourceSeat ?? round.Native.FrozenGroupDamageSourceSeat };
        ReplaceRuntimeFrame(id, use with { ShanheSchoolTrick = round with { Started = true, Active = true, Native = native },
            TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, FireAttackSelection = null,
            ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickStartedEvent(id, action.ActionId, round.ProgramFrameId));
        AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, action.ActionId, 0,
            use.TargetSeats.FirstOrDefault(use.SourceSeat), use.CardKind));
        if (afterAttack) { ActiveCardAttack = null; ActiveDuel = null; ClearPendingDecision(); }
        if (use.TargetSeats.All(seat => !_players[seat].IsAlive || IsCardEffectIneffective(id, seat)))
        { CompleteShanheSchoolTrick(id); return false; }
        ActiveGroupCard = null; ActiveBorrowedSword = null;
        var card = GetTrickRepresentation(id, use.CardId, requireProcessing: true);
        var trick = native.Trick ?? throw new InvalidOperationException("A school repeat lost its native trick continuation.");
        BeginNullificationWindow(id, card, use.SourceSeat, use.TargetSeats, trick.ActionKind,
            null, trick.RequiredCardKind, use.CardKind);
        return true;
    }

    private void AssertShanheSchoolTrickRounds()
    {
        foreach (var use in _resolutionStack.OfType<CardUseFrame>().Where(f => f.ShanheSchoolTrick is not null))
        {
            var round = use.ShanheSchoolTrick!; var native = round.Native;
            if (use.Action is not { Type: CardActionType.Use } action || action.ActionId != round.ActionId ||
                !IsShanheNativeTrickRepeatSource(use.CardKind, native.Source.SkillId) || native.Trick is null || native.Simple is not null ||
                native.Source.OwnerSeat != use.SourceSeat ||
                native.ActionId != action.ActionId || native.ProgramFrameId != round.ProgramFrameId ||
                native.AutomaticGameplayHash != round.GameplayHash || native.WholeGroupRepeated != round.Started ||
                round.Active && !round.Started ||
                ProgramEventHistory<ShanheSchoolTrickIssuedEvent>().Count(e => e.UseFrameId == use.Id &&
                    e.ActionId == round.ActionId && e.ProgramFrameId == round.ProgramFrameId &&
                    e.WindowFrameId == round.WindowFrameId && e.Source == native.Source && e.GameplayHash == round.GameplayHash) != 1 ||
                _contentRegistry.GetSkill(native.Source.SkillId).Program?.GameplayHash != round.GameplayHash ||
                ProgramEventHistory<ShanheSchoolTrickStartedEvent>().Count(e => e.UseFrameId == use.Id &&
                    e.ActionId == round.ActionId && e.ProgramFrameId == round.ProgramFrameId) != (round.Started ? 1 : 0) ||
                ProgramEventHistory<ShanheSchoolTrickCompletedEvent>().Count(e => e.UseFrameId == use.Id &&
                    e.ActionId == round.ActionId && e.ProgramFrameId == round.ProgramFrameId) != (round.Started && !round.Active ? 1 : 0))
                throw new InvalidOperationException("A school trick repeat lost its original one-use receipt or native extra-effect cursor.");
        }
    }
}
