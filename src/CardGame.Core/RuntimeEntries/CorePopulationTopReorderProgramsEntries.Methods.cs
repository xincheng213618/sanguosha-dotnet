#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePopulationTopReorderProgramsEntries
{
    internal CardSnapshot[] GetPopulationTopPrivatelyViewedCards(int viewerSeat) => _resolve().GetPopulationTopPrivatelyViewedCards(viewerSeat);
    internal void ValidatePopulationTopReorder(ProgramSkillFrame frame) => _resolve().ValidatePopulationTopReorder(frame);
}
