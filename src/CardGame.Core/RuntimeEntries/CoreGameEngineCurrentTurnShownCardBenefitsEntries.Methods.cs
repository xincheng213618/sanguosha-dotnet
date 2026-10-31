#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnShownCardBenefitsEntries
{
    internal bool DistanceBorrowedSlash110HasSlashUseDistanceBySuit(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash110HasSlashUseDistanceBySuit(q);
    internal bool DistancePhysicalSlash170HasSlashUseDistanceBySuit(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash170HasSlashUseDistanceBySuit(q);
    internal bool DistanceProvidedSlash50HasSlashUseDistanceBySuit(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceProvidedSlash50HasSlashUseDistanceBySuit(q);
    internal bool DistanceVirtualSlash70HasSlashUseDistanceBySuit(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash70HasSlashUseDistanceBySuit(q);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetCurrentTurnHeartSlashBonuses(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetCurrentTurnHeartSlashBonuses(attack);
    internal void GrantCurrentTurnDirectedHeartSlashBonus(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().GrantCurrentTurnDirectedHeartSlashBonus(frame, targetSeat);
    internal bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit) => _resolve().HasSlashUseDistanceBySuit(owner, kind, suit);
    internal void RevealOwnedBoundCardAppearance(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().RevealOwnedBoundCardAppearance(frame, bind);
    internal bool HasShownCardTurnCapability { get => _resolve().HasShownCardTurnCapability; }
}
