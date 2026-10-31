#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramJudgmentOutcomesRuntime
{
    internal SkillProgramEffect? JudgmentOutcomeInstruction(ProgramSkillFrame frame, int instructionIndex)
    {
        if (!_contentRegistryOperations._contentRegistry.Skills.TryGetValue(frame.SkillId, out var definition) || definition.Program is not { } program || program.GameplayHash != frame.GameplayHash)
            return null;
        var instructions = ProgramInstructionResolver.Default.Resolve(frame, program).Instructions;
        return instructionIndex >= 1 && instructionIndex <= instructions.Count && instructions[instructionIndex - 1] is { Op: SkillProgramEffectOp.StartJudgment, OutcomeBind: not null } effect ? effect : null;
    }

    internal bool TryReturnProgramJudgmentOutcome(JudgmentFrame judgment)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(judgment.ParentFrameId);
        if (JudgmentOutcomeInstruction(frame, frame.InstructionIndex)is not { } effect)
            return false;
        if (!_coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(judgment, frame) || frame.JudgmentOutcomeBindings?.Any(binding => binding.Name == effect.OutcomeBind) == true)
            throw new InvalidOperationException("A judgment outcome lost its unique paused producer.");
        var results = _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Where(result => result.ResolutionId == judgment.Id).ToArray();
        if (results is not [var result] || result.ParentResolutionId != frame.Id || result.TargetSeat != judgment.TargetSeat || result.Reason != effect.JudgmentReason)
            throw new InvalidOperationException("A judgment outcome requires its exact finalized native result.");
        var outcome = new ProgramJudgmentOutcomeBinding(effect.OutcomeBind!, judgment.Id, frame.InstructionIndex, judgment.TargetSeat, judgment.Reason, effect.ResultBind!, result.CardId, result.Suit, result.Rank);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { JudgmentOutcomeBindings = (frame.JudgmentOutcomeBindings ?? []).Append(outcome).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentOutcomeRecordedEvent(frame.Id, outcome));
        // A claim transferred ownership. Its public result remains usable, but
        // the original instruction no longer owns that physical card to clean.
        var retained = result.CardId is { } id && IsUnclaimedJudgmentOutcomeCard(judgment, id) ? new[]
        {
            id
        }

        : [];
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, effect.ResultBind!, retained, SkillProgramCardSetVisibility.Public, retained.Select(_ => CardLocation.Processing).ToArray(), retained.Length == 1 ? result.Suit : null);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal bool IsUnclaimedJudgmentOutcomeCard(JudgmentFrame judgment, int cardId)
    {
        if (_cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Processing || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramJudgmentCardClaimedEvent>().Any(claim => claim.JudgmentFrameId == judgment.Id && claim.OwnerSeat == judgment.TargetSeat && claim.CardId == cardId))
            return false;
        var last = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(fact => fact is not JudgmentResolvedEvent result || result.ResolutionId != judgment.Id).OfType<CardMovedEvent>().LastOrDefault(move => move.CardId == cardId);
        return last is not null && last.From == CardLocation.Judgment(judgment.TargetSeat) && last.To == CardLocation.Processing && last.Reason.Value == "skill-program.judgment.result";
    }

    internal bool IsValidProgramJudgmentOutcome(ProgramSkillFrame frame, ProgramJudgmentOutcomeBinding outcome)
    {
        if (outcome.JudgmentFrameId <= 0 || outcome.InstructionIndex > frame.InstructionIndex || JudgmentOutcomeInstruction(frame, outcome.InstructionIndex)is not { } effect || effect.OutcomeBind != outcome.Name || effect.ResultBind != outcome.ResultBind || effect.JudgmentReason != outcome.Reason || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(outcome.TargetSeat) || outcome.CardId.HasValue != outcome.Suit.HasValue || outcome.CardId.HasValue != outcome.Rank.HasValue || outcome.Suit is { } suit && !Enum.IsDefined(suit) || outcome.Rank is < 0 or > 13)
            return false;
        var results = _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Where(result => result.ResolutionId == outcome.JudgmentFrameId).ToArray();
        return results is [var result] && result.ParentResolutionId == frame.Id && result.TargetSeat == outcome.TargetSeat && result.Reason == outcome.Reason && result.CardId == outcome.CardId && result.Suit == outcome.Suit && result.Rank == outcome.Rank && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramJudgmentOutcomeRecordedEvent>().Count(recorded => recorded.ProgramFrameId == frame.Id && recorded.Outcome == outcome) == 1;
    }

    internal bool IsJudgmentOutcomePrevention(SkillProgramTrigger trigger) => trigger.Effects.Any(effect => effect is { Op: SkillProgramEffectOp.StartJudgment, OutcomeBind: not null }) && trigger.Effects.Any(effect => effect is { Op: SkillProgramEffectOp.PreventCurrentDamage, Condition.Kind: SkillProgramConditionKind.JudgmentOutcomeMatchesSuits });
    internal bool HasRecipientJudgmentOutcomePrevention(int targetSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[targetSeat], SkillProgramTriggerWindow.BeforeDamageApplied).Any(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate)is { Subject: SkillProgramTriggerSubject.DamageTarget } trigger && IsJudgmentOutcomePrevention(trigger));
}
