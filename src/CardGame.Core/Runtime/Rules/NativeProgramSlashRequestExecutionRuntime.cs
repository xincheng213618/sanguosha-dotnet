namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeProgramSlashRequestExecutionRuntime(IProgramRuntimeFrames frames,
    INativeProgramSlashRequests requests, RuntimeEntries.CoreGameEngineProgramSlashRequestEntries choices,
    IProgramRuntimePrompts prompts)
    : INativeProgramSlashRequestExecution
{
    public ProgramSlashRequestAcceptance? GetAcceptance(ProgramSkillFrame frame, int actorSeat, int targetSeat) =>
        frames.Find(frame.Id) is ProgramSkillFrame { SlashRequest.Acceptance: { } acceptance } &&
        requests.OwnsAcceptance(frame, acceptance, actorSeat, targetSeat) ? acceptance : null;

    [ProgramContinuation(ProgramContinuationKind.Resume, 795)]
    internal bool ResumeFailedRequest(long frameId)
    {
        if (prompts.Current is not null || frames.Top is not ProgramSkillFrame
            { SlashRequest.Failure: { Consumed: false } pending } frame || frame.Id != frameId ||
            pending.Acceptance.Identity.Spec.FailureReturnPolicy != ProgramSlashRequestFailureReturnPolicy.ResumeOriginalInstruction ||
            pending.Acceptance.Identity.Spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.NativeInstruction)
            return false;
        var spec = pending.Acceptance.Identity.Spec;
        var existing = spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.ChoiceResult
            ? frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == spec.ResultBind) : null;
        if (existing is not null && (existing.OptionId != RequestSlashByTargetProgramOperationDescriptor.DeclinedOption ||
            existing.ChooserSeat != pending.Acceptance.Identity.ActorSeat))
            throw new InvalidOperationException("A failed Slash request cannot replace a committed successful choice.");
        if (requests.TakeFailure(frame) is not { } failure) return false;
        requests.Close(frame);
        frame = (ProgramSkillFrame)frames.Find(frameId)!;
        frames.Replace(frameId, frame with { Step = ResolutionFrameStep.ResolvingEffect,
            AssistedSlashRequest = null, NearestLegalSlashRequest = null, FixedTargetSlash = null });
        if (spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.ChoiceResult && existing is null)
            choices.CommitProgramChoiceResult(frameId, spec.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.DeclinedOption,
                failure.Acceptance.Identity.ActorSeat, "未使用【杀】。");
        // The ordinary executor retains its original owner/source and unpaid-suffix gates.
        frames.AdvanceProgram(frameId);
        return true;
    }

    public ProgramSkillFrame PrepareAcceptedUse(ProgramSkillFrame supplied, ProgramSlashRequestAcceptance acceptance)
    {
        var frame = frames.Find(supplied.Id) as ProgramSkillFrame ??
            throw new InvalidOperationException("An accepted Slash use lost its original program frame.");
        if (!requests.OwnsAcceptance(frame, acceptance, acceptance.Identity.ActorSeat, acceptance.TargetSeat))
            throw new InvalidOperationException("A native Slash use requires its original accepted request.");
        if (!requests.CanExecuteAcceptedUse(frame, acceptance))
            throw new InvalidOperationException("An accepted Slash payment does not retain permission to issue this native use.");
        var spec = acceptance.Identity.Spec;
        var existing = spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.ChoiceResult
            ? frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == spec.ResultBind) : null;
        if (existing is not null && (existing.OptionId != RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption ||
            existing.ChooserSeat != acceptance.Identity.ActorSeat))
            throw new InvalidOperationException("An accepted Slash request cannot replace its original choice result.");
        if (spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.FixedTarget &&
            (frame.FixedTargetSlash is not { } draft || draft.TargetSeat != acceptance.TargetSeat))
            throw new InvalidOperationException("A fixed Slash return requires its original native draft.");
        // The actual native-success protocol is committed before its public result notification.
        // Later output observers cannot reopen this payment attempt as another offer.
        frame = frame with { SlashRequest = frame.SlashRequest! with { PreparingNativeUse = true } };
        frames.Replace(frame.Id, frame);
        if (spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.ChoiceResult)
        {
            if (existing is null)
                choices.CommitProgramChoiceResult(frame.Id, spec.ResultBind!,
                    RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, acceptance.Identity.ActorSeat, "使用【杀】。");
            frame = (ProgramSkillFrame)frames.Find(frame.Id)!;
        }
        var fixedTarget = frame.FixedTargetSlash;
        if (spec.ReturnProtocol == ProgramSlashRequestReturnProtocol.FixedTarget)
        {
            fixedTarget = fixedTarget! with { Issued = true };
        }
        frame = frame with { AssistedSlashRequest = null, NearestLegalSlashRequest = null, FixedTargetSlash = fixedTarget };
        frames.Replace(frame.Id, frame);
        return frame;
    }

    public CardUseFrame BindExecution(CardUseFrame supplied, ProgramSlashRequestAcceptance acceptance, long? originalProgramId = null)
    {
        var programId = originalProgramId ?? acceptance.Identity.FrameId;
        if (programId != acceptance.Identity.FrameId || frames.Find(programId) is not ProgramSkillFrame parent ||
            !requests.OwnsAcceptance(parent, acceptance, acceptance.Identity.ActorSeat, acceptance.TargetSeat) ||
            frames.Find(supplied.Id) is not CardUseFrame use || !EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            throw new InvalidOperationException("A native Slash execution requires its real original accepted program request.");
        if (parent.SlashRequest!.ExecutionFrameId is { } previous && previous != use.Id)
            throw new InvalidOperationException("One accepted Slash request cannot issue two native uses.");
        var execution = new ProgramSlashRequestExecution(programId, use.Id, use.Action?.ActionId, acceptance);
        if (use.ProgramSlashRequestExecution is { } captured && captured != execution)
            throw new InvalidOperationException("A native Slash use cannot change its original request execution.");
        // The explicit request parent is independent of the physical stack parent.
        // A paid provider's intermediate frame and typed return remain in place.
        frames.Replace(parent.Id, parent with { SlashRequest = parent.SlashRequest! with { ExecutionFrameId = use.Id } });
        use = use with { ProgramSlashRequestExecution = use.ProgramSlashRequestExecution ?? execution };
        frames.Replace(use.Id, use);
        return use;
    }
}
