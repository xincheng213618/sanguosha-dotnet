using System.Globalization;
using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record HappyMedicineHandLoss(int OwnerSeat,Suit EffectiveSuit,int ActualTurnOwnerSeat,long Sequence);
public sealed partial record CardMovementRecord
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public HappyMedicineHandLoss? HappyMedicineHandLoss{get;init;}
}
public sealed partial class GameEngine
{
}
