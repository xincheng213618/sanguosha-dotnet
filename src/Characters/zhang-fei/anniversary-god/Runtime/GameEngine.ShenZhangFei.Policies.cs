using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed record ShenZhangFeiColorlessUse(CardConversionSource Source,string GameplayHash,long ActionId);
public sealed partial record CardUseFrame
{ [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public ShenZhangFeiColorlessUse? ShenZhangFeiColorless { get; init; } }
public sealed record ShenZhangFeiColorlessIssuedEvent(long UseId,ShenZhangFeiColorlessUse Receipt) : IGameEvent;
