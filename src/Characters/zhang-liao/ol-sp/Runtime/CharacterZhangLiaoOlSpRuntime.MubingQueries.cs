#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoOlSpRuntime
{
    internal int MubingMilitaryCount(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<MubingCardsAcquiredEvent>().Where(e => e.Source.OwnerSeat == seat).Sum(e => e.MilitaryCardCount);
    internal static bool IsMubingMilitaryCard(Card card) => RuntimeSkills.RuntimeRules_zhang_liao_ol_sp_Mubing_Queries.IsMubingMilitaryCard(card);
    internal IReadOnlyList<int> MubingAvailableReveal(ProgramSkillFrame f) => _coreProgramLifecycleOperations.GetProgramCardSet(f, MubingReveal).CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
    internal IReadOnlyList<int> MubingAvailableGifts(ProgramSkillFrame f) => f.SpZhangLiao!.Obtained.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)).ToArray();
}
