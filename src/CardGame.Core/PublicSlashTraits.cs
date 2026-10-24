namespace CardGame.Core;

/// <summary>
/// Prospective appearance of an already published Slash, derived by Core for
/// its owner. Null color means colorless; an absent traits record means unknown.
/// These estimates do not issue a use or guarantee a later damage settlement.
/// </summary>
public sealed record PublicSlashTraits(bool? IsRed, bool IgnoresArmor, DamageNature? Nature = null);
