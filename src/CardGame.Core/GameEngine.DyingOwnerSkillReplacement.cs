namespace CardGame.Core;

/// <summary>The already-paid owner HP loss and its one remaining unconditional draw.</summary>
public sealed record ProgramReplacedPaidHpContinuation(
    long ProgramFrameId, int OwnerSeat, string SkillId, string BindingId,
    string SkillInstanceId, string GameplayHash, int PaidInstructionIndex, int DrawAmount,
    long DyingFrameId, long EntryWindowFrameId, long ReplacementFrameId,
    string ReplacementSkillId, string ReplacementBindingId, string ReplacementSkillInstanceId,
    string ReplacementGameplayHash, int ReplacementInstructionIndex, string GrantedSkillId,
    int ActualTurnNumber, int ActualTurnOwnerSeat);

public sealed record ProgramReplacedPaidHpContinuationIssuedEvent(
    ProgramReplacedPaidHpContinuation Continuation) : IGameEvent;

internal static class DyingOwnerSkillReplacementContract
{
    internal static bool HasReplacement(IReadOnlyList<SkillProgramEffect> effects) =>
        effects.Any(effect => effect.Op == SkillProgramEffectOp.LoseOwnerSkillsAndGrant);

    internal static void ValidateTrigger(string path, IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerWindow window, SkillProgramTriggerSubject? subject,
        bool optional, SkillUsageScope? scope, int? limit)
    {
        if (!HasReplacement(effects) || window != SkillProgramTriggerWindow.DyingEntering) return;
        if (subject != SkillProgramTriggerSubject.Owner || optional || scope != SkillUsageScope.Game || limit != 1)
            throw new InvalidOperationException($"Invalid skill program at {path}: dying-entry owner skill replacement requires a mandatory owner game-once binding.");
    }

    internal static bool IsPaidOwnerDrawTail(IReadOnlyList<SkillProgramEffect> effects) =>
        effects.Count == 2 &&
        effects[0] is { Op: SkillProgramEffectOp.LoseHp, Target: SkillProgramEffectTarget.Owner,
            Condition.Kind: SkillProgramConditionKind.Always, CompiledInstruction: LoseHpProgramInstruction { Amount: 1 } } &&
        effects[1] is { Op: SkillProgramEffectOp.Draw, Condition.Kind: SkillProgramConditionKind.Always,
            CompiledInstruction: DrawProgramInstruction { Target: SkillProgramEffectTarget.Owner,
                Amount: FixedProgramAmount { Value: > 0 }, TargetReference: null } };
}
