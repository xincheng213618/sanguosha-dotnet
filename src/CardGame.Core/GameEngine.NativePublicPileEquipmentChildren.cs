using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial record RecoveryPaidCardUseContinuation
{
    private readonly IReadOnlyList<CardUsePaymentMovementReceipt>? _nativeOriginalEquipmentMovements;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CardUsePaymentMovementReceipt>? NativeOriginalEquipmentMovements
    {
        get => _nativeOriginalEquipmentMovements;
        init => _nativeOriginalEquipmentMovements = value is null ? null : Array.AsReadOnly(value.Select(receipt =>
            receipt with { Batch = receipt.Batch }).ToArray());
    }
}
