using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

internal sealed class PursueDeadGeneralDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PursueDeadGeneral;
    public override ISkillProgramEffectHandler Handler { get; } = new PursueDeadGeneralHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantSkills,
        static (_, context) => context.PublicControlValue(3));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "skillIds", "condition");
        var lost = reader.RequiredIdentifierArray("skillIds");
        if (lost.Count != 1)
            throw new InvalidOperationException($"{reader.Path}: specify one skill to lose on pursuit.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader),
            0, reader.Condition(), skillIds: lost);
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CharacterDied)];
}

internal sealed class PursueDeadGeneralHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PursueDeadGeneral;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat,
        ISkillProgramEffectHost host) => host.GetCapability<IDeadGeneralPursuitProgram>().ResolvePursuit(frame, effect);
}

internal interface IDeadGeneralPursuitProgram : IProgramRuntimeOperations
{
    SkillProgramStepOutcome ResolvePursuit(ProgramSkillFrame frame, SkillProgramEffect effect);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class DeadGeneralPursuitProgram(IProgramRuntimeFrames frames, IMatchParticipantStateAccess participants,
    IProgramContentQueries content, CoreGameEngineProgramSkillChangesEntries changes) : IDeadGeneralPursuitProgram
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62210, SkillProgramEffectOp.PursueDeadGeneral)]
    private bool CanOffer(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var effect = trigger.Effects.Single(entry => entry.Op == SkillProgramEffectOp.PursueDeadGeneral);
        return Eligible(candidate.OwnerSeat, context, effect.SkillIds.Single());
    }

    public SkillProgramStepOutcome ResolvePursuit(ProgramSkillFrame source, SkillProgramEffect effect)
    {
        var frame = frames.ActiveProgram(source.Id);
        var lostSkill = effect.SkillIds.Single();
        if (frame.WindowContext is not { TargetSeat: { } victimSeat } context || !Eligible(frame.OwnerSeat, context, lostSkill))
            throw new InvalidOperationException("追思 lost its exact eligible death window.");
        changes.ProgramSkillChanges.PursueDeadGeneral(frame, victimSeat, content, lostSkill);
        return SkillProgramStepOutcome.Continue;
    }

    private bool Eligible(int ownerSeat, ProgramSkillWindowContext context, string lostSkill)
    {
        var owner = participants.At(ownerSeat);
        if (!owner.IsAlive || !owner.HasHpValue || context.Window != SkillProgramTriggerWindow.CharacterDied ||
            context.TargetSeat is not { } victimSeat || victimSeat == ownerSeat || victimSeat < 0 ||
            victimSeat >= participants.Players.Count || !owner.SkillGrants.Grants.Any(grant =>
                grant.IsEnabled && !grant.IsModeRule && grant.SkillId == lostSkill))
            return false;
        return frames.Find(context.ParentFrameId) is ProgramKillTriggerWindowFrame { BeforeDeathCleanup: false } window &&
            window.VictimSeat == victimSeat &&
            frames.Find(window.DeathFrameId) is DeathFrame death && death.VictimSeat == victimSeat &&
            DeadGeneralPursuit.CanPursue(participants.At(victimSeat), content);
    }
}
