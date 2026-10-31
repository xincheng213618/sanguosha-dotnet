#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanHaiBridgesEntries
{
    internal bool? ProgramMultiCardColor(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().ProgramMultiCardColor(owner, selection);
    internal Suit? ProgramMultiCardSuit(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().ProgramMultiCardSuit(owner, selection);
}
