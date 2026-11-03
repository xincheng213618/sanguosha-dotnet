#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsRuntime
{
    internal bool CanContinueProgramAfterOwnerDeath(ProgramSkillFrame frame) =>
        ProgramAcceptedCardMovements.DrainsOwnerDeath(frame) ||
        frame.InvocationAuthorization?.RetainOwnerDeath == true && HasInvocationAuthorization(frame) ||
        _coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.AfterOwnerDeath, frame);
    internal bool CanContinueProgramAfterSkillLoss(ProgramSkillFrame frame) => ProgramAcceptedCardMovements.RetainsSourceLoss(frame) ||
        HasInvocationAuthorization(frame) ||
        _coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.AfterSkillLoss, frame);

    private bool HasInvocationAuthorization(ProgramSkillFrame frame) => ProgramInvocations.Allows(frame,
        frame.WindowContext is { } context ? _frames.Find(context.ParentFrameId) : null);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1)]
    internal bool RetainDeliveredProgramStep(ProgramSkillFrame frame) => frame.TriggerId is null &&
        frame.SelectedCardPayment is { Operation: SkillProgramEffectOp.SelectAndMoveOwnedCard, MovementCommitted: true, Movement: { Delivered: true } } payment &&
        ProgramInstructionResolver.Default.FindActivation(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!, frame.ActivationId) is { ContinueAfterSkillLossAfter: { } bind } activation &&
        activation.Effects.ElementAtOrDefault(payment.InstructionIndex - 1)?.ResultBind == bind;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 2)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 2)]
    internal bool RetainDeliveredDrawStep(ProgramSkillFrame frame)
    {
        if (frame.NativeDraw is { ReturnKind: NativeDrawAttemptReturnKind.ProgramInstruction, RecipientApplied: true } draw &&
            draw.InstructionIndex == frame.InstructionIndex)
            return true;
        if (frame.NativeDraw is not null || frame.InstructionDelivery is not { Operation: SkillProgramEffectOp.Draw } delivered ||
            delivered.InstructionIndex != frame.InstructionIndex)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        return delivered.InstructionIndex == plan.Instructions.Count && delivered.InstructionIndex > 0 &&
            plan.Instructions[delivered.InstructionIndex - 1].Op == SkillProgramEffectOp.Draw;
    }
}
