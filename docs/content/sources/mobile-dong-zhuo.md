# 董卓·移动版

核查日期：2026-10-26。普通移动身份，群、男、8体力、神话再临林包。完整引用 `classic:jiuchi`、`classic:roulin`、`classic:benghuai`、`classic:baonve`；最后一个ID保留本库既有拼写。只增加人物内容，不复制技能或运行时。

来源：[移动官网45](https://www.sanguosha.cn/pc/hero-detail-45.html)、[移动BWIKI人物34832](https://wiki.biligame.com/msgs/index.php?title=董卓&oldid=34832)、[酒池20644](https://wiki.biligame.com/msgs/index.php?title=酒池/董卓&oldid=20644)、[肉林27707](https://wiki.biligame.com/msgs/index.php?title=肉林/董卓&oldid=27707)、[崩坏40255](https://wiki.biligame.com/msgs/index.php?title=崩坏/董卓&oldid=40255)、[暴虐26590](https://wiki.biligame.com/msgs/index.php?title=暴虐/董卓&oldid=26590)。本次重读当前原始API修订：人物2025-12-05，四技能分别2023-01-04、2024-07-17、2026-08-27、2024-03-28，oldid保持上述版本。普通栏为林包、220将魂兑换或招募获取；官网45列出全部四技能但只呈现酒池完整正文，另外三项以当前普通移动技能页补足，不从缺段推断没有技能。

酒池只允许黑桃手牌当酒。`classic:jiuchi` 的ViewAs默认来源确为Hand，限制Spade，输出原生Alcohol，保留正常出牌酒的实际回合次数与本人濒死响应酒入口。没有界版黑色手牌、酒无次数限制或酒杀后免崩坏收益。

肉林双向适用：本人对女性使用杀，或女性对本人使用杀时，真实被杀目标需连续使用两张闪。`classic:roulin` 分别查询真实EventTargetGender/EventSourceGender，使用原生minimumResponseCount与minimumResponseCountAsTarget数值2；多个要求取最大值而不相乘。旧完整三杀列表通过共享 `ProgramCardKindMatch` 覆盖冰杀，不把山河不论男女版本当作普通规则。

崩坏在结束阶段，若本人不是全场体力最低者，强制选择失1体力或减1体力上限。`classic:benghuai` 的实际TurnEnding窗口和 `currentHp > livingPlayersMinHp` 判据一致；与其他角色平最低时不触发，两分支分别沿原生LoseHP及MaximumHP变化，不改为伤害或无条件衰减。

暴虐保留主公技资格。其他当前群势力角色造成伤害后，由该伤害源选择是否进行实际判定，黑桃令董卓回复1体力。`classic:baonve` 使用AfterDamageApplied/PerDamage、非Owner伤害源及动态DamageSourceFactionIs Qun；一次2伤只判一次，不逐点发动。ChooserRef与判定参与者均为原EventSource，判定原实体和黑桃结果后的回复仍走既有原生子流程，并非让主公代选。既有Lord标签与共享角色资格负责是否开放主公技。

四技能完整定义已存在于 `dong-zhuo/classic/Skills/classic-dong-zhuo.rules.json`。本次纯Content引用继承共享转换、连续响应、真实体力变化、判定及原帧返回检查，不新增重复人物快照。构建与批次验证由主代理统一执行。

普通经典图：[4500.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/4500.jpg)，官网45首张；已与移动Wiki“董卓-经典形象.jpg”视觉核对一致。`portraitKey` 为 `mobile-dong-zhuo`，图鉴分组 `myth-forest`，人物及将池序号86799。
