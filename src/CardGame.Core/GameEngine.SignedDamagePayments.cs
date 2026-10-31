using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
internal readonly record struct SignedDamageProfile(int OwnerHp, int OtherHp, bool WaiveHp, bool AnyColor,
        bool AllowEquipment, int DeadRoleMask, string? QualifierInstanceId);
}
