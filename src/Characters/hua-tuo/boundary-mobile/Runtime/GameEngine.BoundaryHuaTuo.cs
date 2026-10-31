using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record BoundaryHuaTuoQingnangReceipt(CardConversionSource Source, string Hash, SkillGrantPhaseExpiry Phase,
    int Target, int CardId, CardKind Kind, Suit Suit, bool Red, long Before, long MovementSequence = 0);
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryHuaTuoQingnangReceipt? BoundaryHuaTuoQingnang { get; init; } }
public sealed record BoundaryHuaTuoQingnangPaidEvent(long FrameId, BoundaryHuaTuoQingnangReceipt Payment) : IGameEvent;

internal interface IBoundaryHuaTuoSelection : IProgramSelectionQuery
{
    int PhaseLimit(int owner);
    bool MaySelect(int owner, int target);
}
[ProgramSelectionModule(ProgramSelectionStage.PhaseLimit, 60910, SkillProgramEffectOp.Recover)]
internal sealed class BoundaryHuaTuoQingnangPhaseLimit : IProgramSelectionPhaseLimitProvider
{
    public int? GetLimit(ProgramSelectionContext context, IProgramSelectionQueries queries) => context.SkillId == "boundary:qingnang-mobile"
        ? queries.Get<IBoundaryHuaTuoSelection>().PhaseLimit(context.OwnerSeat) : context.Activation.UsesPerPhase;
}
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 60910, SkillProgramEffectOp.Recover)]
internal sealed class BoundaryHuaTuoQingnangTargetFilter : IProgramSelectionTargetFilter
{
    public bool Allows(ProgramSelectionContext context, int targetSeat, IProgramSelectionQueries queries) => context.SkillId != "boundary:qingnang-mobile" ||
        queries.Get<IBoundaryHuaTuoSelection>().MaySelect(context.OwnerSeat, targetSeat);
}
[ProgramSelectionValidator(ProgramSelectionValidationStage.AfterCardFilters, 60910, SkillProgramEffectOp.Recover)]
internal sealed class BoundaryHuaTuoQingnangTargetValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) => request.Context.SkillId != "boundary:qingnang-mobile" ||
        request.Targets is [var target] && queries.Get<IBoundaryHuaTuoSelection>().MaySelect(request.Context.OwnerSeat, target)
        ? null : new(CommandErrorCode.IllegalAction, "青囊不能再次选择本回合已选择的角色。");
}
internal static class BoundaryHuaTuoComposition
{
    [ProgramCatalogValidation(60910)]
    private static void Validate(string path, SkillProgram p)
    {
        if (p.Id != "boundary:qingnang-mobile") return;
        if (p.Triggers.Count != 0 || p.Activations is not [{ Id: "discard-and-heal", MinCards: 1, MaxCards: 1, SourceZones: [CardZoneKind.Hand],
            MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.AnyWounded, UsesPerTurn: null, UsesPerPhase: 1,
            Effects: [{ Op: SkillProgramEffectOp.DiscardSelected, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] }])
            throw new InvalidOperationException(path + ": mobile Qingnang requires one native hand discard and one original-target recovery with its red-payment phase allowance.");
    }
}

public sealed partial class GameEngine
{
}
