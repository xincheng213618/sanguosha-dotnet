#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTeamActivityProgramsRuntime
{
    internal bool PublicTeamEnemy(int owner, CharacterState target) => _coreModeAllegianceOperations.ModeAllegiancePolicy.PublicTeamEnemy(global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime.CaptureAllegianceSeat(_coreMatchParticipantsPlayersAccess._players[owner]), global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime.CaptureAllegianceSeat(target));
    internal bool OtherRevealedPublicTeamAlly(CharacterState owner, CharacterState target) => owner.IsAlive && owner.Seat != target.Seat && owner.TeamRevealed && target.TeamRevealed && _coreTeamActivitiesOperations.SamePublicTeam(owner, target);
    SkillProgramStepOutcome global::CardGame.Core.ITeamActivityProgramHost.ExecuteTeamActivity(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteTeamActivity(f, e);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27400)]
    internal bool CanOfferTeamActivity(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.FirstOrDefault(e => e.Op is SkillProgramEffectOp.SelectAllPublicTeamTargets or SkillProgramEffectOp.SelectRandomPublicTeamTarget)is { TargetKind: { } kind } selection && _coreProgramLifecycleOperations.GetProgramTargetSeats(c.OwnerSeat, kind, context).Count < selection.MinimumTargets)
            return false;
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RepeatCurrentRedTrick))
            return _coreExtraEffectsAndRankGuessingOperations.TryGetExtraEffectUse(c.OwnerSeat, context, out var use) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) && context.Facts?.CardActionCardIsRed == true;
        return true;
    }

    internal SkillProgramStepOutcome ExecuteTeamActivity(ProgramSkillFrame input, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (effect.Op is SkillProgramEffectOp.SelectAllPublicTeamTargets or SkillProgramEffectOp.SelectRandomPublicTeamTarget)
        {
            var targets = _coreProgramLifecycleOperations.GetProgramTargetSeats(frame.OwnerSeat, effect.TargetKind!.Value, frame.WindowContext);
            if (targets.Count < effect.MinimumTargets)
                throw new InvalidOperationException("The accepted public-team target set became empty.");
            // Select once before any child can pause, die or change sides. The
            // selected seat is owned by this frame; resumption never rerolls it.
            var frozenTargets = Array.AsReadOnly(effect.Op == SkillProgramEffectOp.SelectRandomPublicTeamTarget && targets.Count > 1 ? new[] { targets[_randomOperations._random.Next(targets.Count)] } : targets.ToArray());
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = frozenTargets });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicTeamTargetsBoundEvent(frame.Id, frame.OwnerSeat, effect.TargetKind.Value, frozenTargets.Count) { InstructionIndex = frame.InstructionIndex, SelectionOp = effect.Op, TargetSeatsKey = PublicTeamTargetSeatsKey(frozenTargets) });
            return SkillProgramStepOutcome.Continue;
        }

        if (effect.Op != SkillProgramEffectOp.RepeatCurrentRedTrick || !_coreExtraEffectsAndRankGuessingOperations.TryGetExtraEffectUse(frame.OwnerSeat, frame.WindowContext, out var use) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || frame.WindowContext?.Facts?.CardActionCardIsRed != true)
            throw new InvalidOperationException("A red-trick repeat requires its original accepted ordinary-trick use.");
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == frame.WindowContext.ParentFrameId);
        var source = new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ExtraCardEffect = new() { Source = source, ProgramFrameId = frame.Id, ActionId = use.Action!.ActionId, AutomaticGameplayHash = frame.GameplayHash, Simple = window.SimpleContinuation, Trick = window.TrickContinuation } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AutomaticExtraCardEffectIssuedEvent(frame.Id, source, use.Id, use.Action!.ActionId, frame.GameplayHash));
        return SkillProgramStepOutcome.Continue;
    }

    internal static string PublicTeamTargetSeatsKey(IReadOnlyList<int> seats) => string.Join(",", seats.Select(seat => seat.ToString(System.Globalization.CultureInfo.InvariantCulture)));
    internal bool IsExactPublicTeamTargetSelection(ProgramSkillFrame frame, SkillProgramEffect selection, ProgramExecutionPlan plan)
    {
        if (selection.Op is not (SkillProgramEffectOp.SelectAllPublicTeamTargets or SkillProgramEffectOp.SelectRandomPublicTeamTarget) || selection.TargetKind is not { } kind || frame.SelectedTargetSeats.Count < selection.MinimumTargets || frame.SelectedTargetSeats.Count > selection.MaximumTargets || frame.SelectedTargetSeats.Distinct().Count() != frame.SelectedTargetSeats.Count)
            return false;
        var producerIndex = 0;
        for (var index = 0; index < frame.InstructionIndex; index++)
            if (ReferenceEquals(plan.Instructions[index], selection))
                producerIndex = index + 1;
        if (producerIndex == 0)
            return false;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<PublicTeamTargetsBoundEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray();
        return issued is [var receipt] && receipt.OwnerSeat == frame.OwnerSeat && receipt.Kind == kind && receipt.InstructionIndex == producerIndex && receipt.SelectionOp == selection.Op && receipt.Count == frame.SelectedTargetSeats.Count && receipt.TargetSeatsKey == PublicTeamTargetSeatsKey(frame.SelectedTargetSeats);
    }
}
