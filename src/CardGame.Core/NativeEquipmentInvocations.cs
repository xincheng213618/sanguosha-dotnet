using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum NativeEquipmentInvocationKind { Bagua, FactionBagua, IceSword, StoneAxe, QilinBow, Cixiong, Qinglong }
public sealed record NativeEquipmentInvocationReceipt(long ParentFrameId, long AttackId,
    NativeEquipmentInvocationKind Kind, int ActorSeat, int TargetSeat, string SkillKey, string OccurrenceKey,
    int EquipmentCardId = 0, int SelectedCardId = 0, CardKind? EffectiveSlashKind = null,
    int PhysicalOwnerSeat = -1, long? FactionRequestId = null, CardConversionSource? ConversionSource = null,
    Card? SlashCard = null, int ProviderRewardCount = 0, string CommitmentNonce = "")
{
    private IReadOnlyList<int> _materials = Array.AsReadOnly(Array.Empty<int>());
    private IReadOnlyList<CardLocation> _from = Array.AsReadOnly(Array.Empty<CardLocation>());
    public IReadOnlyList<int> PhysicalCardIds { get => _materials; init => _materials = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardLocation> PhysicalFrom { get => _from; init => _from = Array.AsReadOnly(value.ToArray()); }
}
public abstract partial record ResolutionFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeEquipmentInvocationReceipt? NativeEquipmentInvocation { get; init; }
}
// The event stores only scalar proof. The receipt's nested materials are frozen
// on the original owning frame; the fingerprint binds those IDs to issuance.
public sealed record NativeEquipmentInvocationIssuedEvent(long ParentFrameId, long AttackId,
    NativeEquipmentInvocationKind Kind, int ActorSeat, int TargetSeat, string SkillKey,
    string OccurrenceKey, string Fingerprint) : IGameEvent;
public sealed record NativeEquipmentInvocationReturnedEvent(long InvocationId, long ParentFrameId,
    long AttackId, NativeEquipmentInvocationKind Kind, string OccurrenceKey, bool Canceled) : IGameEvent;
