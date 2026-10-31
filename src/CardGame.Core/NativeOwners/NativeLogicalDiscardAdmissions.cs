#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeLogicalDiscardAdmissions(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : INativeLogicalDiscardAdmissions
{
    public NativeLogicalDiscardInvocationDraft? Admit(NativeLogicalDiscardInvocationDraft? draft) => getCharacterZhangFeiWumiaoRuntime().AdmitWuZishengNativeDiscard(draft);
    public void AdmitMaterials(NativeLogicalDiscardInvocationDraft draft) => getCharacterZhangFeiWumiaoRuntime().AdmitWuZishengDiscardMaterials(draft);
    public void Close(NativeLogicalDiscardInvocationDraft? draft) => getCharacterZhangFeiWumiaoRuntime().CloseWuZishengNativeDiscard(draft);
}
