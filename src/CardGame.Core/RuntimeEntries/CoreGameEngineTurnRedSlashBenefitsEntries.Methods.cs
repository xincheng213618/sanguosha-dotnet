#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsEntries
{
    internal bool DistanceBorrowedSlash130HasTurnRedSlashPolicy(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash130HasTurnRedSlashPolicy(q);
    internal bool DistancePhysicalSlash180HasTurnRedSlashPolicy(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash180HasTurnRedSlashPolicy(q);
    internal bool DistanceProvidedSlash70HasRedSlashPolicy(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceProvidedSlash70HasRedSlashPolicy(q);
    internal bool DistanceVirtualSlash80HasTurnRedSlashPolicyForColor(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash80HasTurnRedSlashPolicyForColor(q);
    internal void GrantTurnRedSlashBenefits(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GrantTurnRedSlashBenefits(frame);
    internal bool HasTurnRedSlashPolicy(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit) => _resolve().HasTurnRedSlashPolicy(actor, kind, suit);
    internal bool HasTurnRedSlashPolicyForColor(int actor, global::CardGame.Core.CardKind kind, bool? color) => _resolve().HasTurnRedSlashPolicyForColor(actor, kind, color);
    internal bool IsRedSlashProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false) => _resolve().IsRedSlashProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
    internal bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().PhysicalGroupColor(owner, cards);
    internal bool CapturesActionColor { get => _resolve().CapturesActionColor; }
    internal bool HasTurnRedSlashCapability { get => _resolve().HasTurnRedSlashCapability; }
}
