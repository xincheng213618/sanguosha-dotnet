using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum QizhengStage { Designation, Response, PaidResponse, Gain, Damage, Finishing }
public sealed record QizhengReceipt(long NullificationId, long ActionId, int Target, int TargetIndex, int EffectOrdinal, QizhengStage Stage)
{
    public bool Ambush { get; init; }
    public string Nonce { get; init; } = "";
    public string Commitment { get; init; } = "";
    public CardKind? ResponseKind { get; init; }
    public long ResponseActionId { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public QizhengReceipt? Qizheng { get; init; }
}
public sealed record QizhengEffectStartedEvent(long UseId, long NullificationId, long ActionId, int Target, int TargetIndex, int EffectOrdinal) : IGameEvent;
public sealed record QizhengSecretCommittedEvent(long UseId, long NullificationId, string Commitment) : IGameEvent;
public sealed record QizhengResponsePaidEvent(long UseId, long NullificationId, CardKind Kind) : IGameEvent;
public sealed record QizhengResolvedEvent(long UseId, long NullificationId, int Target, bool Ambush, CardKind? ResponseKind) : IGameEvent;
[ProgramRuntimeModule] internal static class QizhengRoutes
{ internal static bool HasReceipt(CardUseFrame f) => f.Qizheng is not null; }
