#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanHaiBridgesEntries
{
    internal bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().ProgramMultiCardColor(owner, selection);
    internal global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().ProgramMultiCardSuit(owner, selection);
}
