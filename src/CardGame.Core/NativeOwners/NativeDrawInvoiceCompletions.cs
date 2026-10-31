#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawInvoiceCompletions(Func<RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime, Func<RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime, Func<RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime) : INativeDrawInvoiceCompletions
{
    public void CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof) => getCharacterWangCanOlRuntime().CompleteSanwenNativeDrawQualification(proof);
    public void CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof) => getCharacterTangTangOlRuntime().CompleteTangTangNativeDrawQualification(proof);
    public void CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof) => getCharacterZhouFeiLeRuntime().CompleteLingkongNativeDrawQualification(proof);
}
