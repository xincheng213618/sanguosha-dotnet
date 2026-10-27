using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ZhaoguMirrorStage { Template, RevealChildren, UseChooser, BasicChild, TrickIssuing, TrickChild, TrickCleanup }
public sealed record ZhaoguMirrorReceipt(CardConversionSource Source, string GameplayHash, int InstructionIndex,
    long LifecycleFrameId, int TurnNumber, ZhaoguMirrorStage Stage = ZhaoguMirrorStage.Template,
    CardSnapshot? Template = null, long TrickChildId = 0, long TrickActionId = 0);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ZhaoguMirrorReceipt? ZhaoguMirror { get; init; }
}

internal interface IZhaoguMirrorHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame); }
internal sealed class ResolveShanheZhaoguMirrorDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheZhaoguMirror;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayEnding)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs,
        static (_, context) => context.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheZhaoguMirrorHandler();
}
internal sealed class ResolveShanheZhaoguMirrorHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheZhaoguMirror;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IZhaoguMirrorHost>().Resolve(frame);
}
internal static class ZhaoguMirrorContract
{
    [ProgramCatalogValidation(62057)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheZhaoguMirror)) &&
            !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheZhaoguMirror))) return;
        if (program.Id != "shanhe:zhaogu-mirror-6018" || program.Activations.Count != 0 || program.ViewAs.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.Triggers is not
            [{ Id: "show-hand-card-and-use", Window: SkillProgramTriggerWindow.PlayEnding,
                Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true,
                Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShanheZhaoguMirror, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": Zhaogu Mirror requires its single optional actual own PlayEnding template instruction.");
    }
}
