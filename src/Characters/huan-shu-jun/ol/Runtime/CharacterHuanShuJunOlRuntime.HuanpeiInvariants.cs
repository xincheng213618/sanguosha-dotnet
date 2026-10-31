#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal ProgramSkillFrame? HuanpeiActiveDraft() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.HuanShuJun is { Op: SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, Request.Intent: not HuanpeiIntent.Play });
    internal PendingDecision? HuanpeiInvariantDecision(PendingDecision? ordinary)
    {
        if (HuanpeiActiveDraft()is not { } f)
            return ordinary is null ? null : HuanpeiNativeDecision(ordinary);
        AssertHuanShuJun(f);
        return HuanpeiNativeDecision(f.HuanShuJun!.OriginalDecision!);
    }

    internal ResolutionFrame? HuanpeiInvariantTop(ResolutionFrame? ordinary)
    {
        if (HuanpeiActiveDraft()is not { } f)
            return ordinary;
        AssertHuanShuJun(f);
        return _resolutionStackOperations._resolutionStack.Single(x => x.Id == f.HuanShuJun!.Request!.RequestFrameId);
    }

    internal bool HuanpeiDraftRidesOn(ProgramSkillFrame draft, ProgramSkillFrame parent) => draft.HuanShuJun is { Op: SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, Request: { } n, OriginalDecision: { } decision } r && HuanShuJunPaidOrPreparing(draft) && n.ParentFrameId == parent.Id && n.RequestFrameId == parent.Id && n.Cursor == parent.InstructionIndex && n.ParentSkillId == parent.SkillId && n.ParentSkillInstanceId == parent.SkillInstanceId && n.ParentGameplayHash == parent.GameplayHash && n.ParentOperation == _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op && n.ActorSeat == draft.OwnerSeat && decision.PlayerSeat == draft.OwnerSeat && decision.PromptId == n.OriginalPromptId && decision.Revision == n.OriginalRevision && draft.Id > parent.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<HuanpeiRequestCapturedEvent>().Count(e => e.FrameId == draft.Id && e.Request == n && e.DecisionHash == HuanShuJunDecisionHash(decision)) == 1;
}
