#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{

    internal bool HuanpeiDraftRidesOn(ProgramSkillFrame draft, ProgramSkillFrame parent) => draft.HuanShuJun is { Op: SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, Request: { } n, OriginalDecision: { } decision } r && HuanShuJunPaidOrPreparing(draft) && n.ParentFrameId == parent.Id && n.RequestFrameId == parent.Id && n.Cursor == parent.InstructionIndex && n.ParentSkillId == parent.SkillId && n.ParentSkillInstanceId == parent.SkillInstanceId && n.ParentGameplayHash == parent.GameplayHash && n.ParentOperation == _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op && n.ActorSeat == draft.OwnerSeat && decision.PlayerSeat == draft.OwnerSeat && decision.PromptId == n.OriginalPromptId && decision.Revision == n.OriginalRevision && draft.Id > parent.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuanpeiRequestCapturedEvent>().Count(e => e.FrameId == draft.Id && e.Request == n && e.DecisionHash == HuanShuJunDecisionHash(decision)) == 1;
}
