using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IShanheSourceYanyuHost
{
    SkillProgramStepOutcome ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal sealed class ResolveShanheSourceYanyuDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceYanyu;
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheSourceYanyuHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.StartJudgment, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "outcomeBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0,
            reader.Condition(), sourceBind: reader.RequiredIdentifier("sourceBind"))
        { OutcomeBind = reader.RequiredIdentifier("outcomeBind") };
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow), new ReadSelectedTarget(),
         new ReadCardSet(effect.SourceBind!), new MoveCardSet(effect.SourceBind!, null, SkillProgramCardDestination.OwnerHand)];
}

internal sealed class ResolveShanheSourceYanyuHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheSourceYanyu;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheSourceYanyuHost>().ResolveSourceYanyu(frame, effect);
}

internal static class ShanheSourceYanyuContract
{
    [ProgramCatalogValidation(62046)]
    internal static void Validate(string path, SkillProgram program)
    {
        const string skill = "shanhe:source-yanyu-3292";
        if (program.Id != skill && !program.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheSourceYanyu)) &&
            !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheSourceYanyu))) return;
        if (program.Id != skill || program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.Contributions.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.Triggers is not [{ Id: "other-preparation-red-at-most-three", Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Optional: false,
                Condition.Kind: SkillProgramTriggerConditionKind.Always, Effects.Count: 7 } trigger] ||
            trigger.UsageScope is not null || trigger.UsageLimit is not null || trigger.DynamicUsageLimit is not null ||
            trigger.ChoiceGroup is not null || trigger.SourceSkillId is not null || trigger.SourceViewAsId is not null ||
            trigger.CardKinds.Count != 0 || trigger.Suits.Count != 0 ||
            trigger.Effects[0] is not { Op: SkillProgramEffectOp.SelectTarget, Target: SkillProgramEffectTarget.Owner,
                TargetKind: SkillProgramTargetKind.CurrentTurnPlayer, Condition.Kind: SkillProgramConditionKind.Always })
            throw new InvalidOperationException(path + ": source Yanyu requires its single locked other-preparation trigger and exact bounded selection.");
        for (var round = 0; round < 3; round++)
        {
            var judgment = trigger.Effects[1 + round * 2];
            var settle = trigger.Effects[2 + round * 2];
            if (judgment is not { Op: SkillProgramEffectOp.StartJudgment, Target: SkillProgramEffectTarget.SelectedTarget,
                    JudgmentReason: "shanhe.source-yanyu-3292", Visibility: SkillProgramCardSetVisibility.Public,
                    Condition.Kind: SkillProgramConditionKind.Always, SourceRef: null } ||
                judgment.ResultBind != "judgment-" + round || judgment.OutcomeBind != "outcome-" + round ||
                settle is not { Op: SkillProgramEffectOp.ResolveShanheSourceYanyu, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always } ||
                settle.SourceBind != judgment.ResultBind || settle.OutcomeBind != judgment.OutcomeBind)
                throw new InvalidOperationException(path + ": source Yanyu requires exactly three ordered native judgment/result-settlement pairs.");
        }
    }
}

public enum ShanheSourceYanyuStage { Damage, Movement }

/// <summary>One bounded judgment's promised damage and exact original-material disposition.</summary>
public sealed record ShanheSourceYanyuReceipt(int InstructionIndex, int TurnNumber, int TargetSeat,
    long PreparationFrameId, string CardBind, ProgramJudgmentOutcomeBinding Outcome, bool Red, bool DamagePromised,
    int? RetainedCardId, ShanheSourceYanyuStage Stage, int? MovementSequence = null,
    CardLocation? Destination = null);

public sealed record ShanheSourceYanyuDamageRequestedEvent(long ProgramFrameId, int InstructionIndex,
    long JudgmentFrameId, int OwnerSeat, int TargetSeat, int TurnNumber) : IGameEvent;
public sealed record ShanheSourceYanyuCardSettledEvent(long ProgramFrameId, int InstructionIndex,
    long JudgmentFrameId, int OwnerSeat, int TargetSeat, int? CardId, bool Red,
    int? MovementSequence, CardLocation? Destination) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheSourceYanyuReceipt? ShanheSourceYanyu { get; init; }
}
