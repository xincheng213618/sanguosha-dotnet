#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDamageReductionRuntime
{
    internal void ReduceProgramCurrentDamage(ProgramSkillFrame program, int amount)
    {
        if (program.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.Prevented || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.SourceSeat != window.SourceSeat || attack.TargetSeat != window.TargetSeat || amount <= 0)
            throw new InvalidOperationException("Damage reduction lost its active damage window.");
        var reduced = Math.Min(amount, window.Amount);
        attack.ReduceFinalizedDamageAmount(reduced);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[^2].Id, window with { Amount = window.Amount - reduced, Prevented = window.Amount == reduced });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageReducedEvent(program.Id, program.SkillId, program.OwnerSeat, window.SourceSeat, window.TargetSeat, window.Amount, window.Amount - reduced));
    }
}
