#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFeiOlMagicEntries
{
    internal bool IsIssuedNativeDrawPileSlashReplacement(global::CardGame.Core.NativeDrawAttemptContext context, global::CardGame.Core.NativeDrawReplacementResult replacement) => _resolve().IsIssuedNativeDrawPileSlashReplacement(context, replacement);
    internal global::CardGame.Core.NativeDrawPileSlashProof? NativeDrawPileSlashReplacementProof(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawAttemptContext context) => _resolve().NativeDrawPileSlashReplacementProof(frame, context);
}
