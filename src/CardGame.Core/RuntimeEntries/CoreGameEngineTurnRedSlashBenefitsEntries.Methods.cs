#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsEntries
{
    internal bool DistanceBorrowedSlash130HasTurnRedSlashPolicy(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash130HasTurnRedSlashPolicy(q);
    internal bool DistancePhysicalSlash180HasTurnRedSlashPolicy(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash180HasTurnRedSlashPolicy(q);
    internal bool DistanceProvidedSlash70HasRedSlashPolicy(ProgramSlashDistanceQuery q) => _resolve().DistanceProvidedSlash70HasRedSlashPolicy(q);
    internal bool DistanceVirtualSlash80HasTurnRedSlashPolicyForColor(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash80HasTurnRedSlashPolicyForColor(q);
    internal void GrantTurnRedSlashBenefits(ProgramSkillFrame frame) => _resolve().GrantTurnRedSlashBenefits(frame);
    internal bool HasTurnRedSlashPolicy(int actor, CardKind kind, Suit? suit) => _resolve().HasTurnRedSlashPolicy(actor, kind, suit);
    internal bool HasTurnRedSlashPolicyForColor(int actor, CardKind kind, bool? color) => _resolve().HasTurnRedSlashPolicyForColor(actor, kind, color);
    internal bool IsRedSlashProviderPaymentLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) => _resolve().IsRedSlashProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
    internal bool? PhysicalGroupColor(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().PhysicalGroupColor(owner, cards);
    internal bool CapturesActionColor { get => _resolve().CapturesActionColor; }
    internal bool HasTurnRedSlashCapability { get => _resolve().HasTurnRedSlashCapability; }
}
