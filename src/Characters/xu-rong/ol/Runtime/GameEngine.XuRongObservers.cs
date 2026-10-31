using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed record XuRongActualDamageHpProof(long DamageFrameId,long DamageAttemptResolutionId,int TargetSeat,int HpBefore,int ActualAmount,int UnclampedHpAfter);
public sealed partial record DamageFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
 public XuRongActualDamageHpProof? XuRongActualHp {get;init;}
}
