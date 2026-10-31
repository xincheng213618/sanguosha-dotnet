#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawInvoiceCompletions(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime) : INativeDrawInvoiceCompletions
{
    public void CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof) => getCharacterWangCanOlRuntime().CompleteSanwenNativeDrawQualification(proof);
    public void CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof) => getCharacterTangTangOlRuntime().CompleteTangTangNativeDrawQualification(proof);
    public void CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof) => getCharacterZhouFeiLeRuntime().CompleteLingkongNativeDrawQualification(proof);
}
