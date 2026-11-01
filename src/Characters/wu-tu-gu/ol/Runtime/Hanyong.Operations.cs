namespace CardGame.Core;

public sealed record WuTuGuHanyongQualifiedEvent(long UseId,long ActionId,CardConversionSource Source,string GameplayHash,int ActualTurn,int Round,int HpAtUse,int ProviderSeat,CardKind Kind):IGameEvent;
internal sealed class ArmHanyongNativeGroupDamageDescriptor():WuTuGuDescriptor(SkillProgramEffectOp.ArmHanyongNativeGroupDamage){}
