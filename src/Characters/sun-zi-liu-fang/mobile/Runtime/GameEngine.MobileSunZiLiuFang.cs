namespace CardGame.Core;

public sealed record MobileJiyuInvokedEvent(long FrameId, CardConversionSource Source, string Hash,
    SkillGrantPhaseExpiry Phase, int TurnOwner, int Target, bool ExtraUse) : IGameEvent;
public sealed record MobileJiyuCompletedEvent(long FrameId, bool EarnedExtraUse) : IGameEvent;

internal interface IMobileJiyuSelection : IProgramSelectionQuery
{
    int PhaseLimit(int owner);
    bool MaySelect(int owner, int target);
}
[ProgramSelectionModule(ProgramSelectionStage.PhaseLimit, 78300, SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition)]
internal sealed class MobileJiyuPhaseLimit : IProgramSelectionPhaseLimitProvider
{
    public int? GetLimit(ProgramSelectionContext context, IProgramSelectionQueries queries) => context.SkillId == "mobile:jiyu"
        ? queries.Get<IMobileJiyuSelection>().PhaseLimit(context.OwnerSeat) : context.Activation.UsesPerPhase;
}
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 78300, SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition)]
internal sealed class MobileJiyuTargetFilter : IProgramSelectionTargetFilter
{
    public bool Allows(ProgramSelectionContext context, int targetSeat, IProgramSelectionQueries queries) => context.SkillId != "mobile:jiyu" ||
        queries.Get<IMobileJiyuSelection>().MaySelect(context.OwnerSeat, targetSeat);
}
[ProgramSelectionValidator(ProgramSelectionValidationStage.AfterCardFilters, 78300, SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition)]
internal sealed class MobileJiyuTargetValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) => request.Context.SkillId != "mobile:jiyu" ||
        request.Targets is [var target] && queries.Get<IMobileJiyuSelection>().MaySelect(request.Context.OwnerSeat, target)
        ? null : new(CommandErrorCode.IllegalAction, "讥谀的额外次数不能选择本回合已经选择过的角色。");
}
internal static class MobileJiyuComposition
{
    [ProgramCatalogValidation(78300)]
    private static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "mobile:jiyu") return;
        if (program.Triggers.Count != 0 || program.Activations is not [{ Id: "hand-discard-restriction", MinCards: 0, MaxCards: 0,
            MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.AnyLivingWithHand, UsesPerTurn: null, UsesPerPhase: 1,
            Condition.Kind: SkillProgramConditionKind.Always, TargetPhaseLedgerId: null,
            Effects: [{ Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1,
                Zones: [CardZoneKind.Hand], ResultBind: "discard" },
                { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "discard", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true },
                { Op: SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition, Target: SkillProgramEffectTarget.Owner, SourceBind: "discard" },
                { Op: SkillProgramEffectOp.TurnOver, Target: SkillProgramEffectTarget.Owner },
                { Op: SkillProgramEffectOp.LoseHp, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] }])
            throw new InvalidOperationException(path + ": mobile Jiyu requires its original native discard, turn suit prohibition, turnover and HP loss in that order.");
    }
}
