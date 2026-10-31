#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreExactTopReorderProgramsEntries
{
    internal global::CardGame.Core.CardSnapshot[] GetExactTopPrivatelyViewedCards(int viewerSeat) => _resolve().GetExactTopPrivatelyViewedCards(viewerSeat);
    internal void ValidateExactTopReorder(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateExactTopReorder(frame);
}
