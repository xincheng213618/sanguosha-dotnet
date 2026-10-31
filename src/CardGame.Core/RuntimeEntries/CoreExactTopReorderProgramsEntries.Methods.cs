#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreExactTopReorderProgramsEntries
{
    internal CardSnapshot[] GetExactTopPrivatelyViewedCards(int viewerSeat) => _resolve().GetExactTopPrivatelyViewedCards(viewerSeat);
    internal void ValidateExactTopReorder(ProgramSkillFrame frame) => _resolve().ValidateExactTopReorder(frame);
}
