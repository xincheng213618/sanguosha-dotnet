namespace CardGame.Core;

// Owns program judgment admission and continuation qualification. Native
// judgments retain their current producer, frames and character-specific proofs.
[ProgramRuntimeModule]
internal sealed class ProgramJudgmentRuntime(IProgramRuntimeFrames frames, IProgramContentQueries content,
    IProgramJudgmentNativeProofs native, IProgramJudgmentProducer producer)
{
    internal SkillProgramStepOutcome StartProgramJudgment(ProgramSkillFrame frame, int targetSeat,
        string reason, string resultBind, SkillProgramCardSetVisibility visibility, int sourceSeat)
    {
        if (frames.Top is not ProgramSkillFrame current || current.Id != frame.Id ||
            !MatchesProgramJudgmentInstruction(frame, targetSeat, reason, resultBind, visibility))
        {
            throw new InvalidOperationException(
                "A program judgment requires an active program target and public result.");
        }

        producer.Begin(frame.Id, targetSeat, reason, frame.WindowContext?.CardUse?.EffectiveKind,
            sourceSeat, resultBind, visibility);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsValidProgramJudgmentContinuation(JudgmentFrame pending, ProgramSkillFrame? frame)
    {
        if (frame?.ShenZhongHui is not null) return native.IsLishiJudgment(pending, frame);
        if (frame?.ShenZhangFei is not null) return native.IsShenZfJudgment(pending, frame);
        if (frame?.ShenGuoJia is not null) return native.IsShenGuoJiaJudgment(pending, frame);
        if (frame?.WushuangCombat is { TacticId: 2019 })
            return pending.ParentFrameId == frame.Id && native.IsWushuangLightningJudgment(frame, pending.TargetSeat, pending.Reason, pending.ProgramResultBind!, pending.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private);
        if (frame?.MouGuoJia is not null) return native.IsMouGjJudgment(pending, frame);
        if (frame?.SourceHuojie is not null) return native.IsSourceHuojieJudgment(pending, frame);
        if (frame?.ZhouQun is not null) return native.IsNingwuJudgment(pending, frame);
        if (frame?.MaYuanYi is not null) return native.IsBinghuoJudgment(pending, frame);
        if (frame?.Weifu is not null) return native.IsWeifuJudgment(pending, frame);
        if (frame?.ZhangJi is not null) return native.IsZhangJiJudgment(pending, frame);
        if (pending.Continuation != JudgmentContinuationKind.ProgramSkill || frame is null ||
            pending.ParentFrameId != frame.Id ||
            pending.ProgramResultBind is not { } bind ||
            pending.ProgramResultVisibility is not { } visibility)
            return false;
        if (!MatchesProgramJudgmentInstruction(frame, pending.TargetSeat, pending.Reason, bind, visibility))
            return false;
        var judged = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!)
            .Instructions;
        if (frame.InstructionIndex < 1 || frame.InstructionIndex > judged.Count ||
            judged[frame.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.StartJudgment or SkillProgramEffectOp.StartOwnedDamagePointJudgment or SkillProgramEffectOp.StartChouceDamagePointJudgment or SkillProgramEffectOp.JudgeDamageTargetThenOfferSuitDiscard } effect)
            return true;
        var expectedSourceSeat = effect.SourceRef is { } sourceRef
            ? native.Participant(frame, sourceRef)
            : frame.OwnerSeat;
        return pending.SourceSeat == expectedSourceSeat;
    }

    internal bool MatchesProgramJudgmentInstruction(ProgramSkillFrame frame, int targetSeat,
        string reason, string resultBind, SkillProgramCardSetVisibility visibility)
    {
        if (frame.ShenZhongHui is not null)
            return native.CanBeginLishiJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.ShenZhangFei is not null)
            return native.CanBeginShenZfJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.ShenGuoJia is not null)
            return native.CanBeginShenGuoJiaJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.WushuangCombat is { TacticId: 2019 })
            return native.IsWushuangLightningJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.MouGuoJia is not null)
            return native.MouGjJudgmentRequest(frame, targetSeat, reason, resultBind, visibility);
        if (frame.SourceHuojie is not null)
            return native.CanBeginSourceHuojieJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.ZhouQun is not null)
            return native.CanBeginNingwuJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.MaYuanYi is not null)
            return native.CanBeginBinghuoJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (frame.Weifu is not null)
            return native.MatchesWeifuJudgmentInstruction(frame, targetSeat, reason, resultBind, visibility);
        if (frame.ZhangJi is not null)
            return native.CanBeginZhangJiJudgment(frame, targetSeat, reason, resultBind, visibility);
        if (visibility != SkillProgramCardSetVisibility.Public || string.IsNullOrWhiteSpace(reason) ||
            string.IsNullOrWhiteSpace(resultBind) ||
            (ProgramEntryCapabilities.For(frame.WindowContext?.Window) & ProgramContextCapability.Judgment) == 0 ||
            content.FindSkill(frame.SkillId)?.Program is not { } program || program.GameplayHash != frame.GameplayHash)
            return false;
        var instructions = ProgramInstructionResolver.Default.Resolve(frame, program).Instructions;
        if (frame.InstructionIndex < 1 || frame.InstructionIndex > instructions.Count ||
            instructions[frame.InstructionIndex - 1] is not
            { Op: SkillProgramEffectOp.StartJudgment or SkillProgramEffectOp.StartOwnedDamagePointJudgment or SkillProgramEffectOp.StartChouceDamagePointJudgment or SkillProgramEffectOp.RepeatJudgment or SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeSuitDiscard or SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeColorAllDiscard or SkillProgramEffectOp.JudgeDamageTargetThenOfferSuitDiscard } effect ||
            reason != effect.JudgmentReason || resultBind != effect.ResultBind)
            return false;
        if (effect.Op == SkillProgramEffectOp.RepeatJudgment &&
            (frame.RepeatedJudgment is not { } repeated ||
             repeated.Reason != reason || repeated.ResultBind != resultBind))
            return false;
        if (effect.Op == SkillProgramEffectOp.JudgeDamageTargetThenOfferSuitDiscard)
            return native.IsDamageJudgmentInstructionTarget(frame, targetSeat);
        return effect.Target switch
        {
            SkillProgramEffectTarget.Owner => targetSeat == frame.OwnerSeat,
            SkillProgramEffectTarget.SelectedTarget =>
                frame.SelectedTargetSeats is [var selectedSeat] && targetSeat == selectedSeat,
            _ => false
        };
    }
}
