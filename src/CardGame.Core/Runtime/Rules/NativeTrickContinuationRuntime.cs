using CardGame.Core.EngineOwners;

namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeTrickContinuationRuntime(IProgramRuntimeFrames frames,
    INativeCardRepresentations representations) : INativeTrickContinuations
{
    public void ValidateIssuance(long useId, CardActionContext action, ProgramCardContinuation stage,
        ProgramTrickContinuation continuation)
    {
        if (frames.Top is not CardUseFrame use || use.Id != useId ||
            !MatchesUse(use, action, continuation) || !MatchesStage(use, stage))
            throw new InvalidOperationException("A trick window requires its original native use and typed continuation.");
    }

    public bool Matches(ProgramCardTriggerWindowFrame window) =>
        window.Continuation is { } stage && window.TrickContinuation is { } continuation &&
        window.SimpleContinuation is null && window.CompletedResponseReturn is null &&
        window.AttackOwnerFrameId is null && window.ResponseCompletion is null && window.CardSupplyCompletion is null &&
        frames.Find(window.ParentFrameId) is CardUseFrame use &&
        MatchesUse(use, window.Action, continuation) && MatchesStage(use, stage);

    public NativeTrickContinuationRead Read(ProgramCardTriggerWindowFrame window)
    {
        if (!Matches(window) || frames.Top is not CardUseFrame use || use.Id != window.ParentFrameId)
            throw new InvalidOperationException("A trick window must return to its original native parent.");
        var continuation = window.TrickContinuation!;
        return new(use, representations.Get(use.Id, continuation.EffectCardId, requireProcessing: true), continuation);
    }

    public NativeTrickContinuationRead Read(ActualUseTargetWindowFrame window)
    {
        if (window.ReturnKind != ActualUseTargetReturnKind.OrdinaryTrick || window.TrickReturn is not { } original ||
            frames.Top is not CardUseFrame use || use.Id != window.ParentFrameId ||
            !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || use.Action is not { } action)
            throw new InvalidOperationException("An actual-target trick window must return to its original native parent.");
        var continuation = new ProgramTrickContinuation(original.EffectCardId, original.ActionKind,
            original.TargetCardId, original.RequiredCardKind);
        if (!MatchesUse(use, action, continuation) || window.Contexts.Any(context =>
                context.ActualUseTarget is { } identity &&
                (identity.CardUseFrameId != use.Id || identity.ActionId != action.ActionId)))
            throw new InvalidOperationException("An actual-target trick continuation changed its original use identity.");
        return new(use, representations.Get(use.Id, continuation.EffectCardId, requireProcessing: true), continuation);
    }

    private static bool MatchesUse(CardUseFrame use, CardActionContext action,
        ProgramTrickContinuation continuation) => use.Action is { Type: CardActionType.Use } current &&
        current.ActionId > 0 && action.Type == CardActionType.Use && current.ActionId == action.ActionId &&
        current.EffectiveKind == use.CardKind && action.EffectiveKind == use.CardKind &&
        Enum.IsDefined(use.CardKind) && CardCatalog.Get(use.CardKind).CategoryName == "锦囊牌" &&
        use.NativeRepresentation is { } representation && representation.Id == use.CardId &&
        continuation.EffectCardId == representation.Id;

    private static bool MatchesStage(CardUseFrame use, ProgramCardContinuation stage) => stage switch
    {
        ProgramCardContinuation.CommittedTrick => use.ProgramUseCommitted,
        ProgramCardContinuation.FinalizedTrick => use.FinalizedTrickProgramsStarted,
        ProgramCardContinuation.BeforeTrickTargetEffects => true,
        ProgramCardContinuation.FinalizedDelayedPlacement => use.ProgramUseAccepted &&
            use.FinalizedTrickProgramsStarted && CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind),
        _ => false
    };
}
