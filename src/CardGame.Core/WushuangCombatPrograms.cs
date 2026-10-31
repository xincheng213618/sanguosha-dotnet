using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IWushuangCombatHost
{
    SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal sealed class WushuangCombatDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWushuangCombatTactic;
    public override ISkillProgramEffectHandler Handler { get; } = new WushuangCombatHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Damage, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "tactic", "sourceBind", "condition");
        var tactic = r.RequiredInt("tactic");
        if (tactic is not (2019 or 2020 or 2039 or 2040 or 2104 or 2105 or 2106 or 2116 or 2120 or 2121 or 2122 or
            2130 or 2131 or 2143 or 2174 or 2175 or 2185 or 2198 or 2245 or 2246 or 2247 or 2337 or 2365 or 2367))
            throw new InvalidOperationException($"{r.Path}: unknown combat tactic operation.");
        var bind = r.OptionalIdentifier("sourceBind");
        if ((tactic is 2039 or 2040 or 2365) != (bind is not null))
            throw new InvalidOperationException($"{r.Path}: recasts require their selected physical-card binding.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), tactic, r.Condition(), sourceBind: bind);
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => e.SourceBind is not { } bind ? [] :
        e.Amount == 2365 ? [new RequireActivationEntry(), new ReadCardSet(bind), new MoveCardSet(bind, null, SkillProgramCardDestination.DiscardPile)] :
        [new RequireOwnedCardSet(bind, SkillProgramEffectTarget.Owner, e.Amount == 2040 ? 2 : 1, [CardZoneKind.Hand, CardZoneKind.Equipment]),
            new ReadCardSet(bind), new MoveCardSet(bind, null, SkillProgramCardDestination.DiscardPile)];
}
internal sealed class WushuangCombatHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWushuangCombatTactic;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost h) =>
        h.GetCapability<IWushuangCombatHost>().Execute(f, e);
}

public sealed record WushuangCombatReceipt(int TacticId, int InstructionIndex, int TargetSeat, int Amount,
    DamageNature Nature, int Stage = 0, int Index = 0, int DrawCount = 0)
{
    private readonly IReadOnlyList<int> _seats = Array.Empty<int>();
    public IReadOnlyList<int> Seats { get => _seats; init => _seats = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WushuangCombatReceipt? WushuangCombat { get; init; }
}
public sealed record WushuangCombatIssuedEvent(long ProgramFrameId, int TacticId, int OwnerSeat, int InstructionIndex) : IGameEvent;
public sealed record WushuangCombatDamageFactEvent(long DamageFrameId, int Turn, int Round, int SourceSeat, int TargetSeat,
    int Amount, CardKind? Kind, DamageNature Nature, bool SourceLess) : IGameEvent;
public sealed record WushuangCombatUseFactEvent(long CardUseFrameId, int Turn, int Round, int OwnerSeat,
    CardKind Kind, Suit Suit, int SlashOrdinal, bool Drunken) : IGameEvent;
public sealed record WushuangLuckyNumberEvent(int OwnerSeat, int Number) : IGameEvent;
public sealed record WushuangIncomingReductionIssuedEvent(long AttackFrameId, int TargetSeat, int Round, int Ordinal) : IGameEvent;

public sealed partial class GameEngine
{
}
