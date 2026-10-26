using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed record CustomGeneralLibrary
{
    public const int CurrentFormatVersion = 1;
    public int FormatVersion { get; init; } = CurrentFormatVersion;
    public IReadOnlyList<CustomGeneralDraft> Generals { get; init; } = [];
    public static CustomGeneralLibrary Empty { get; } = new();
}

public sealed record CustomGeneralDraft
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string FactionId { get; init; } = "wei";
    public int BaseHp { get; init; } = 4;
    public int? InitialHp { get; init; }
    public GeneralGender Gender { get; init; } = GeneralGender.Male;
    public string PortraitKey { get; init; } = "supporter";
    public IReadOnlyList<string> SkillIds { get; init; } = [];
    public IReadOnlyList<CustomSkillDraft> CustomSkills { get; init; } = [];
    public bool Enabled { get; init; } = true;
}

public sealed record CustomSkillDraft
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public CustomSkillTrigger Trigger { get; init; } = CustomSkillTrigger.Active;
    public CustomSkillTarget Target { get; init; } = CustomSkillTarget.Self;
    public CustomSkillCondition Condition { get; init; } = CustomSkillCondition.Always;
    public int ConditionValue { get; init; } = 2;
    public int UsesPerTurn { get; init; } = 1;
    public bool Optional { get; init; } = true;
    public IReadOnlyList<CustomSkillEffect> Effects { get; init; } = [];
}

public sealed record CustomSkillEffect
{
    public CustomSkillEffectKind Kind { get; init; } = CustomSkillEffectKind.Draw;
    public int Amount { get; init; } = 1;
    public DamageNature DamageNature { get; init; } = DamageNature.Normal;
}

public enum CustomSkillTrigger { Active = 0, TurnStart = 1, PlayStart = 2, PlayEnd = 3, TurnEnd = 4, AfterDamage = 5, AfterDealtDamage = 6 }
public enum CustomSkillTarget { Self = 0, Other = 1, Any = 2 }
public enum CustomSkillCondition { Always = 0, Wounded = 1, HandEmpty = 2, HpAtMost = 3 }
public enum CustomSkillEffectKind { Draw = 0, Recover = 1, Damage = 2, LoseHp = 3 }
