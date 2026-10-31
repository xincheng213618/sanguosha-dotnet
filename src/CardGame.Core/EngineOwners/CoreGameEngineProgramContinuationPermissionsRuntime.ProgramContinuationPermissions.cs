#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsRuntime
{
    internal bool CanContinueProgramAfterOwnerDeath(ProgramSkillFrame frame) => _coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.AfterOwnerDeath, frame);
    internal bool CanContinueProgramAfterSkillLoss(ProgramSkillFrame frame) => _coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.AfterSkillLoss, frame);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1)]
    internal bool RetainDeliveredProgramStep(ProgramSkillFrame frame) => frame.TriggerId is null &&
        frame.SelectedCardPayment is { Operation: SkillProgramEffectOp.SelectAndMoveOwnedCard, MovementCommitted: true, Movement: { Delivered: true } } payment &&
        ProgramInstructionResolver.Default.FindActivation(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!, frame.ActivationId) is { ContinueAfterSkillLossAfter: { } bind } activation &&
        activation.Effects.ElementAtOrDefault(payment.InstructionIndex - 1)?.ResultBind == bind;
}
