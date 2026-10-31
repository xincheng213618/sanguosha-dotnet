namespace CardGame.Core;
public sealed record KuanshiProtectionGrant(long FrameId,CardConversionSource Source,string GameplayHash,int ArmedTurn,int TargetSeat);
public sealed record KuanshiDrawDebt(long GrantFrameId,int OwnerSeat,int ArmedTurn);
public sealed record KuanshiArmedEvent(KuanshiProtectionGrant Grant):IGameEvent;
public sealed record KuanshiExpiredEvent(long GrantFrameId,int TurnNumber,string Reason):IGameEvent;
public sealed record KuanshiDamagePreventedEvent(long GrantFrameId,long DamageAttemptResolutionId,int? SourceSeat,int TargetSeat,int Amount):IGameEvent;
public sealed record KuanshiDrawSkippedEvent(int OwnerSeat,int TurnNumber):IGameEvent;
public sealed partial class CharacterState
{
 private IReadOnlyList<KuanshiProtectionGrant> _kuanshiGrants=Array.AsReadOnly(Array.Empty<KuanshiProtectionGrant>());
 internal IReadOnlyList<KuanshiProtectionGrant> KuanshiGrants {get=>_kuanshiGrants;set=>_kuanshiGrants=Array.AsReadOnly(value.ToArray());}
 internal KuanshiDrawDebt? KuanshiDebt {get;set;}
}
