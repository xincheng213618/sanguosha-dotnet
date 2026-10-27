using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Installs only the selected campaign or battle's tier and lifecycle rules.</summary>
public sealed class StandardBattleFeatureFactory : IGameModeFeatureFactory
{
    public string Id => "mode:standard-battle";
    public string ModeId => ModeFeatureTargets.AllModes;

    public IGameModeFeature Create(ModeFeatureCreationContext context)
    {
        var options = context.Options ?? throw new InvalidOperationException("Battle rules require actual match options.");
        var mode = context.ModeDefinition ?? throw new InvalidOperationException("Battle rules require the selected mode definition.");
        var features = new List<IGameModeFeature>();
        if (options.ShanheBattle is { } campaign)
        {
            features.Add(new ShanheBattleFeature(campaign, context.GetHost<IStandardBattleHost>()));
            features.Add(new ShanheInitialDongdangFeature(campaign, context.GetHost<IModeEncounterHost>(),
                context.GetHost<IStandardBattleHost>()));
            if (campaign.BossTransition is not null)
                features.Add(new ShanheBossStageFeature(campaign, context.GetHost<IModeEncounterHost>(),
                    context.GetHost<IStandardBattleHost>()));
        }
        if (mode.ShenwuActivity is not null)
            features.Add(new ShenwuBattleFeature(options, mode.ShenwuActivity, context.ContentRegistry,
                context.GetHost<IStandardBattleHost>(), context.GetHost<IModeEncounterHost>()));
        if (mode.BossActivity is not null && mode.HellActivity is null)
            features.Add(new BossBattleNumericRules(options, mode, context.GetHost<IStandardBattleHost>(), context.GetHost<IModeEncounterHost>()));
        if (mode.TeamActivity is not null)
            features.Add(new JiangeBattleRules(options, mode, context.GetHost<IStandardBattleHost>()));
        return new ModeFeatureBundle(features);
    }
}
