using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core;

/// <summary>One paid native segment that issued a propagation base before a newly capped recipient.</summary>
public sealed record LateFixedTargetDamageCapScopeReceipt(long AttackFrameId, long DamageFrameId,
    int? SourceSeat, bool SourceLess, int PaidTargetSeat, int Amount, DamageNature Nature, CardKind? SourceCard,
    int ChainTargetIndex, int NextTargetIndex, int NextTargetSeat, int ChainTargetCount, string ChainTargetsHash,
    int RequestedEventIndex, int AppliedEventIndex, int AfterDamageEventIndex, int? ProgramUseInstructionIndex,
    CardUseEffectSource CapSource, string CapGameplayHash, int Cap);
