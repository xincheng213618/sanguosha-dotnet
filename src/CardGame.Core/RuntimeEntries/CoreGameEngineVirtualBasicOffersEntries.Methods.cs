#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineVirtualBasicOffersEntries
{
    internal void CompleteVirtualBasicUseReturn(NativeUseReturnContext context) => _resolve().CompleteVirtualBasicUseReturn(context);
    internal bool ContinueVirtualBasicEffect(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueVirtualBasicEffect(id, continuation);
    internal void FinishVirtualBasicUse(long id) => _resolve().FinishVirtualBasicUse(id);
    internal SkillProgramStepOutcome OfferVirtualBasicCard(ProgramSkillFrame frame) => _resolve().OfferVirtualBasicCard(frame);
    internal void ResolveVirtualBasicChoice(PromptChoice selected) => _resolve().ResolveVirtualBasicChoice(selected);
    internal void ReturnVirtualBasicUse(ProgramVirtualBasicReturn? receipt, long childId) => _resolve().ReturnVirtualBasicUse(receipt, childId);
    internal IEnumerable<(CardKind Kind, int Target)> VirtualBasicOptions(ProgramSkillFrame frame) => _resolve().VirtualBasicOptions(frame);
    internal IEnumerable<(CardKind Kind, int Target)> VirtualBasicOptions2(int owner, bool anyRegisteredBasic = false) => _resolve().VirtualBasicOptions(owner, anyRegisteredBasic);
}
