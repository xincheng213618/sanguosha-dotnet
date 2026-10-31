#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreUnconvertedCardUseQualificationRuntime
{
    internal bool IsUnconvertedActualCardUse(CardActionContext action, SkillProgramTriggerWindow window, ProgramCardContinuation? continuation)
    {
        if (action.ConversionChain.Count != 0)
            return false;
        if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized)
            return action.Type == CardActionType.Use && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame use && use.Action?.ActionId == action.ActionId && use.SourceSeat == action.ActorSeat;
        if (window != SkillProgramTriggerWindow.CardResponseAccepted || continuation != ProgramCardContinuation.NullificationResponse || !_coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.NullificationResponse) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not NullificationWindowFrame pending || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow?.Id != pending.Id || action.OpponentSeat != pending.SourceSeat)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == pending.ParentFrameId);
        return parent?.Action?.ActionId == action.ParentActionId;
    }
}
