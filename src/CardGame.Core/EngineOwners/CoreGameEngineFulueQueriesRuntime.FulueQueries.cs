#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFulueQueriesRuntime
{
    internal bool HasFulueCompletedCounterChild(ProgramCardTriggerWindowFrame window)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => ReferenceEquals(frame, window));
        return window.Continuation is ProgramCardContinuation.CompletedSlash or ProgramCardContinuation.CompletedCard && index >= 0 && index + 2 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 1] is ProgramSkillFrame { Fulue: { } receipt, WindowContext.CardUse: { } identity } root && _characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(root) && root.WindowContext.ParentFrameId == window.Id && receipt.UseId == window.ParentFrameId && receipt.ActionId == window.Action.ActionId && identity.ParentCardUseFrameId == window.ParentFrameId && identity.CardActionId == window.Action.ActionId && _resolutionStackOperations._resolutionStack[index + 2] is CardUseFrame { FulueCounter: { } invoice } counter && invoice.ProgramFrameId == root.Id && _characterPeiYuanShaoOlOperations.ValidFulueCounterInvoice(counter, invoice);
    }
}
