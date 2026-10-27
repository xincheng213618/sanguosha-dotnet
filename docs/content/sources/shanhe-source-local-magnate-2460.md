# 巧破城楼2460／本地豪强

来源为官方OL [Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)冻结配置，外包SHA256为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；解码 `hd_roguelike.sgs` 为 `97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72`，`cha_spell.sgs` 为 `e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219`。

原 `Fight[fightID=2460]` 的Text标题为“巧破城楼”。`General[a=2460]` 只有本地豪强一名：`b=4861,c=本地豪强,d=1000320,k=4,l=4,p=2,q=4`，即士兵来源身体4/4、正常摸2、随机起手4。没有固定装备、隐藏随从、`att_*` 或二阶段；`pvpspell=3189` 不作为单机技能授予。

| 来源难度 | 累计技能 | 复用定义 |
| --- | --- | --- |
| 1—10 | 3538束辎 | `ol:shuzi` |
| 11—15 | 再加951伪诚 | `ol:weicheng` |
| 16—25 | 再加3277顺世 | `ol:shunshi` |

束辎在真实出牌阶段限一次，交给一名角色两张手牌或装备牌，接收其返还的一张手牌；返还牌与任一原赠牌牌名相同时，可造成伤害或移动实际场牌。复用韩馥OL的原生交换帧，保留真实实体付款、返还与孩子返回。伪诚在其他角色取得自己手牌后，手牌少于当前体力时可摸一张。顺世在准备阶段或回合外受伤后，交给伤害来源以外的另一人一张牌，将增益分别留到下个实际摸牌、出牌与弃牌阶段。

三个程序分别来自 `src/Characters/han-fu/ol/Skills/ol-han-fu.rules.json`、`jiang-gan/ol/Skills/ol-jiang-gan.rules.json`、`du-fu-ren/ol/Skills/ol-du-fu-ren.rules.json`。它们按技能实例、当前帧和阶段资格工作，没有韩馥、蒋干或杜夫人的身体门槛。本包只授这三项，不复制其他武将的身体或附属技能，不重复注册既有程序。

来源身份为 `character:shanhe-source-soldier-4861`，三个技能档使用独立的 `shanhe:source-local-magnate-2460-*` 定义，原始体力均为4/4。`d=1000320` 不能作为已核本地肖像，使用明确的 `supporter` 缺图回退。

S5的 `UniversalGroup` 池15和17均引用2460，对应广成、虎牢两个第一章地区的14座普通城市。服务器前提与权重没有公开执行依据，具体候选抽选仍沿本地政策。推荐三章的两个入口也将其放到广成、虎牢及开场克隆，本地1／2／3档选来源1／11／21档，使用既有第一章单将体力、摸牌与奖励曲线，详见[普通敌军投放](shanhe-source-ordinary-troops-2026.md)。
