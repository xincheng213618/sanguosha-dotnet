#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeLogicalDiscardAdmissions(Func<RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : INativeLogicalDiscardAdmissions
{
    public NativeLogicalDiscardInvocationDraft? Admit(NativeLogicalDiscardInvocationDraft? draft) => getCharacterZhangFeiWumiaoRuntime().AdmitWuZishengNativeDiscard(draft);
    public void AdmitMaterials(NativeLogicalDiscardInvocationDraft draft) => getCharacterZhangFeiWumiaoRuntime().AdmitWuZishengDiscardMaterials(draft);
    public void Close(NativeLogicalDiscardInvocationDraft? draft) => getCharacterZhangFeiWumiaoRuntime().CloseWuZishengNativeDiscard(draft);
}
