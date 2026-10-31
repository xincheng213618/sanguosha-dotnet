#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFeiOlMagicEntries
{
    internal bool IsIssuedNativeDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => _resolve().IsIssuedNativeDrawPileSlashReplacement(context, replacement);
    internal NativeDrawPileSlashProof? NativeDrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => _resolve().NativeDrawPileSlashReplacementProof(frame, context);
}
