#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineVirtualBasicOffersEntries
{
    internal void AssertVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().AssertVirtualBasicDraft(frame, effect);
    internal void CompleteVirtualBasicUseReturn(global::CardGame.Core.NativeUseReturnContext context) => _resolve().CompleteVirtualBasicUseReturn(context);
    internal bool ContinueVirtualBasicEffect(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueVirtualBasicEffect(id, continuation);
    internal void FinishVirtualBasicUse(long id) => _resolve().FinishVirtualBasicUse(id);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferVirtualBasicCard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OfferVirtualBasicCard(frame);
    internal void ResolveVirtualBasicChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveVirtualBasicChoice(selected);
    internal void ReturnVirtualBasicUse(global::CardGame.Core.ProgramVirtualBasicReturn? receipt, long childId) => _resolve().ReturnVirtualBasicUse(receipt, childId);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardKind Kind, int Target)> VirtualBasicOptions(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().VirtualBasicOptions(frame);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardKind Kind, int Target)> VirtualBasicOptions2(int owner, bool anyRegisteredBasic = false) => _resolve().VirtualBasicOptions(owner, anyRegisteredBasic);
}
