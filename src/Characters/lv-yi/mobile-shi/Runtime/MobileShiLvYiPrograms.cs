using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShiLyHuilvGrantedEvent(long FrameId, int OwnerSeat, int ActorSeat, int Round, bool Red, int CardId) : IGameEvent;
public sealed record ShiLyHuilvSettledEvent(long FrameId, long GrantFrameId, int OwnerSeat, int ActorSeat, string Outcome, int? GivenToSeat) : IGameEvent;
public sealed record ShiLySongyanUsedEvent(long FrameId, int OwnerSeat, int Round, long UseId) : IGameEvent;
public sealed record ShiLyRoundDiscardedEvent(int Seat, int Round, int CardId, bool Red) : IGameEvent;
public sealed record ShiLyShishiRecordedEvent(int OwnerSeat, int SourceSeat, int Round) : IGameEvent;
public sealed record ShiLySongyanOrigin(long FrameId, long UseId, long ActionId, int OwnerSeat, int[] OuterProcessing)
{
    private readonly int[] _outerProcessing = OuterProcessing.ToArray();
    public int[] OuterProcessing { get => _outerProcessing; init => _outerProcessing = value.ToArray(); }
}

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiLySongyanOrigin? ShiLySongyanOrigin { get; init; }
}

public enum ShiLyStage { HuilvGrant = 0, HuilvSettleDamage = 1, HuilvSettleGive = 2 }

public sealed record ShiLyReceipt(string Operation, ShiLyStage Stage)
{
    public int ActorSeat { get; init; }
    public bool Red { get; init; }
    public int CardId { get; init; }
    public long GrantFrameId { get; init; }
    public bool Used { get; init; }
    public bool ThenGive { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShiLyReceipt? LvYi { get; init; }
}

internal static class ShiLvYiComposition
{
    internal const string HuilvSkill = "mobile:shi-huilv";
    internal const string SongyanSkill = "mobile:shi-songyan";
    internal const string ShishiSkill = "mobile:shi-shishi";
    internal const string HuilvStartTrigger = "huilv-play-start";
    internal const string HuilvEndTrigger = "huilv-play-end";
    internal const string SongyanPrepTrigger = "songyan-prep";
    internal const string SongyanDamageTrigger = "songyan-damage";
    internal const string SongyanViewAs = "songyan-as-nullification";
    internal const string ProgramAction = "mobile-shi-lv-yi";
    internal const string HuilvGainReason = "skill-program.mobile:shi-huilv.gain";
    internal const string HuilvGiveReason = "skill-program.mobile:shi-huilv.give";

    [ProgramCatalogValidation(68600)]
    internal static void ValidateProgram(string path, SkillProgram program)
    {
        if (program.Id == ShishiSkill)
        {
            if (program.DamageModifiers.Count != 0 || program.Modifiers.Count != 0 ||
                program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
                program.Contributions.Count != 0 || program.Activations.Count != 0 || program.Triggers.Count != 0 ||
                program.CardPolicies.Count != 0 || program.BooleanStates is not
                [{ Id: "shishi-inert", InitialValue: false }] || program.ViewAs is not
                [{ Id: "shishi-inert-alias", OutputKind: CardKind.Slash, ForPlay: true, ForResponse: false }])
                throw new InvalidOperationException(path +
                    ": ShiShi must stay a locked skill with only its unreachable inert conversion.");
            return;
        }
        if (program.Id == SongyanSkill)
        {
            if (program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 || program.CardPolicies.Count != 0 ||
                program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 || program.Contributions.Count != 0 ||
                program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
                program.ViewAs is not [{ Id: SongyanViewAs, OutputKind: CardKind.Nullification, ForResponse: true, ForPlay: false }] ||
                program.Triggers is not
                [
                    { Id: SongyanPrepTrigger, Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiLySongyan, Target: SkillProgramEffectTarget.Owner }] },
                    { Id: SongyanDamageTrigger, Window: SkillProgramTriggerWindow.AfterDamageApplied,
                        Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false,
                        DamageOccurrence: SkillProgramDamageOccurrence.PerDamage,
                        Effects: [{ Op: SkillProgramEffectOp.ResolveShiLySongyan, Target: SkillProgramEffectTarget.Owner }] }
                ])
                throw new InvalidOperationException(path +
                    ": SongYan requires its prep and damage triggers plus exactly its response Nullification conversion.");
            return;
        }
        if (program.Id != HuilvSkill) return;
        if (program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.CardIdentities.Count != 0 || program.HpSkillGrants.Count != 0 ||
            program.Contributions.Count != 0 || program.BooleanStates.Count != 0 || program.Activations.Count != 0 ||
            program.Triggers is not
            [
                { Id: HuilvStartTrigger, Window: SkillProgramTriggerWindow.PlayPhaseStarting,
                    Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving,
                    Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiLyHuilv, Target: SkillProgramEffectTarget.Owner }] },
                { Id: HuilvEndTrigger, Window: SkillProgramTriggerWindow.PlayEnding,
                    Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving,
                    Optional: false, IsSkillInvocation: false,
                    Effects: [{ Op: SkillProgramEffectOp.ResolveShiLyHuilvSettle, Target: SkillProgramEffectTarget.Owner }] }
            ])
            throw new InvalidOperationException(path +
                ": HuiLv requires exactly its other-living play-start grant and play-end settlement triggers.");
    }
}

internal interface IShiLvYiHost
{
    SkillProgramStepOutcome ResolveHuilv(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveHuilvSettle(ProgramSkillFrame frame);
    SkillProgramStepOutcome ResolveSongyan(ProgramSkillFrame frame);
}

internal sealed class ResolveShiLyHuilvDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiLyHuilv;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiLyHandler(SkillProgramEffectOp.ResolveShiLyHuilv);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Gift,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiLyHuilvSettleDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiLyHuilvSettle;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiLyHandler(SkillProgramEffectOp.ResolveShiLyHuilvSettle);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ResolveShiLySongyanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShiLySongyan;
    public override ISkillProgramEffectHandler Handler { get; } = new ShiLyHandler(SkillProgramEffectOp.ResolveShiLySongyan);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs,
        static (_, context) => context.PublicControlValue(1));
    public override ProgramFactRequirements FactRequirements => new();
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
}

internal sealed class ShiLyHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveShiLyHuilv => host.GetCapability<IShiLvYiHost>().ResolveHuilv(frame),
        SkillProgramEffectOp.ResolveShiLyHuilvSettle => host.GetCapability<IShiLvYiHost>().ResolveHuilvSettle(frame),
        _ => host.GetCapability<IShiLvYiHost>().ResolveSongyan(frame),
    };
}
