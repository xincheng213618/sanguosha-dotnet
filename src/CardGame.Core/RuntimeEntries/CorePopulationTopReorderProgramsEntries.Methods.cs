#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePopulationTopReorderProgramsEntries
{
    internal global::CardGame.Core.CardSnapshot[] GetPopulationTopPrivatelyViewedCards(int viewerSeat) => _resolve().GetPopulationTopPrivatelyViewedCards(viewerSeat);
    internal void ValidatePopulationTopReorder(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidatePopulationTopReorder(frame);
}
