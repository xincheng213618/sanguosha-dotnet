namespace CardGame.Core;

public enum ActualTurnKind { Normal, Extra }
/// <summary>One actual turn, its singular extra beneficiary and the saved normal seat position.</summary>
public sealed partial record ActualTurnProgression(ActualTurnKind Kind, int OwnerSeat, int TurnNumber,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] int? PendingBeneficiarySeat = null,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] int? NormalResumeSeat = null);
