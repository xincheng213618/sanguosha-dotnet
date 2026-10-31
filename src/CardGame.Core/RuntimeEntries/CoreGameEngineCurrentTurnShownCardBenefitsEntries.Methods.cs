#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnShownCardBenefitsEntries
{
    internal bool DistanceBorrowedSlash110HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash110HasSlashUseDistanceBySuit(q);
    internal bool DistancePhysicalSlash170HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash170HasSlashUseDistanceBySuit(q);
    internal bool DistanceProvidedSlash50HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => _resolve().DistanceProvidedSlash50HasSlashUseDistanceBySuit(q);
    internal bool DistanceVirtualSlash70HasSlashUseDistanceBySuit(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash70HasSlashUseDistanceBySuit(q);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> GetCurrentTurnHeartSlashBonuses(GameEngine.IDamageAttempt attack) => _resolve().GetCurrentTurnHeartSlashBonuses(attack);
    internal void GrantCurrentTurnDirectedHeartSlashBonus(ProgramSkillFrame frame, int targetSeat) => _resolve().GrantCurrentTurnDirectedHeartSlashBonus(frame, targetSeat);
    internal bool HasSlashUseDistanceBySuit(CharacterState owner, CardKind kind, Suit? suit) => _resolve().HasSlashUseDistanceBySuit(owner, kind, suit);
    internal void RevealOwnedBoundCardAppearance(ProgramSkillFrame frame, string bind) => _resolve().RevealOwnedBoundCardAppearance(frame, bind);
    internal bool HasShownCardTurnCapability { get => _resolve().HasShownCardTurnCapability; }
}
