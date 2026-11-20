using CardGame.Core.EngineOwners;
using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

public sealed record VirtualDamageIssuedEvent(long ResolutionId, long ProgramFrameId,
    int SourceSeat, int TargetSeat, int Amount) : IGameEvent;

internal sealed class IssueVirtualDamageDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.IssueVirtualDamage;
    public override ISkillProgramEffectHandler Handler { get; } = new IssueVirtualDamageHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage,
        static (_, context) => context.PublicControlValue(1));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "amount", "condition");
        var amount = reader.RequiredInt("amount");
        if (amount is < 1 or > 20)
            throw new InvalidOperationException($"{reader.Path}: virtual damage needs a positive amount.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), amount, reader.Condition());
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadTargetSet(2, 2)];
}

internal sealed class IssueVirtualDamageHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.IssueVirtualDamage;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat,
        ISkillProgramEffectHost host) => host.GetCapability<IVirtualDamageProgram>().BeginVirtualDamage(frame, effect.Amount);
}

internal interface IVirtualDamageProgram : IProgramRuntimeOperations
{
    SkillProgramStepOutcome BeginVirtualDamage(ProgramSkillFrame frame, int amount);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class VirtualDamageProgram(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IMatchParticipantStateAccess participants, IKernelCreateProgramAttackHandleOperations attacks,
    CoreGameEngineProgramAttackAttemptEntries activeAttacks, CoreNativeDamageWindowsEntries windows,
    CoreNativeDamageCompletionEntries completion, CoreNativeRuleQueriesEntries distances) : IVirtualDamageProgram
{
    public SkillProgramStepOutcome BeginVirtualDamage(ProgramSkillFrame supplied, int amount)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.TriggerId is not null || frame.WindowContext is not null || frame.AttackAttempt is not null ||
            activeAttacks.CurrentDamageAttempt is not null || frame.SelectedTargetSeats is not [var source, var target] ||
            source == target || frame.OwnerSeat == source || frame.OwnerSeat == target ||
            !participants.At(source).IsAlive || !participants.At(target).IsAlive || amount <= 0)
            throw new InvalidOperationException("Virtual damage requires two distinct other living selected characters.");

        frames.ReplaceTop(frame with
        {
            AttackAttempt = new(source, target, amount, DamageNature.Normal, SourceLess: false, DamageAmountFinalized: true,
                SourceToTargetDistanceAtDamage: distances.GetCombatDistance(source, target)),
            AttackReturn = new(null, null, null)
        });
        var damage = new DamageFrame(frames.NextId(), frame.Id, source, target, amount) { IsVirtual = true };
        frames.Push(damage);
        events.AdvanceFact(new VirtualDamageIssuedEvent(damage.Id, frame.Id, source, target, amount));
        var attack = attacks.CreateProgramAttackHandle(frame.Id);
        if (!windows.TryBeginDamageTriggerWindow(attack, damage.Id, SkillProgramTriggerWindow.AfterDamageApplied))
        {
            frames.Pop(damage.Id, ResolutionFrameKind.Damage);
            completion.CompleteDamageAttack(attack);
        }

        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static bool IsExactVirtualDamage(ProgramSkillFrame owner, DamageFrame damage) =>
        damage.ParentFrameId == owner.Id && owner.TriggerId is null && owner.WindowContext is null &&
        damage.SourceSeat != damage.TargetSeat && damage.SourceSeat != owner.OwnerSeat && damage.TargetSeat != owner.OwnerSeat &&
        owner.SelectedTargetSeats.SequenceEqual([damage.SourceSeat, damage.TargetSeat]) &&
        owner.AttackAttempt is { DamageWasApplied: false, DamageAmountFinalized: true, SourceLess: false } attack &&
        attack.SourceSeat == damage.SourceSeat && attack.TargetSeat == damage.TargetSeat &&
        attack.DamageAmount == damage.Amount && attack.Nature == damage.Nature && damage.AppliedHpAfter is null;
}
