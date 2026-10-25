# 移动版刘表

核对日期：2026-10-25。正式身份 `mobile:liu-biao`，群、男性、三体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-liu-biao`。

- [移动版官网刘表121](https://www.sanguosha.cn/index.php/pc/hero-detail-121.html)：完整HTML同时读取自守及默认隐藏的宗室。自守按现存势力数额外摸牌，本回合使用牌不能选择其他角色为目标；宗室按现存势力数增加手牌上限。
- [移动版Wiki刘表](https://wiki.biligame.com/msgs/刘表)，2023-01-26更新：当前普通身份两项能力与官网一致，页面注明这是修订后的普通身份。后续官方修订检索未发现替代正文的移动版公告。

完整复用 `classic:zishou` 与 `classic:zongshi`，不制造移动版同规则技能。自守保留可不发动的原生摸牌阶段选择；发动时原生额外摸现存势力数张牌，并发出持续本实际回合的 `selfOnly` 用牌目标约束。该约束不是禁止使用全部牌：自身装备、回复等仍按真实目标及用牌合法性处理。宗室动态查询仍存活角色的不同势力，增加当前手牌上限。

本次同步纠正经典人物声明与共享呈现中旧有“本回合出牌阶段内”的窄化文字；运行时原已按实际回合生效：`GrantProgramTurnCardTargetRestriction` 记录回合号与回合座位，`HasTargetRestriction` 不读取阶段，租约由实际回合结束的 `ExpireTurnCardUseEffects` 清除，没有更改规则或技能ID。

未采用最初按已损失体力摸牌并跳过出牌阶段的旧自守，也未加入其他版本的额外伤害限制、手牌花色交换或杀次数政策。现有原生额外摸牌和回合目标政策已表达完整现行规则，因此仅新增身份配置、来源和独立画像；不新增 Runtime、共享端口或重复人物夹具。

独立画像为官网首张[经典形象51600](https://www.sanguosha.cn/storage/uploads/images/skins/51600.jpg)，原始530×750 JPEG，未裁剪或改色，本地 `src/CardGame.Wpf/Assets/official-mobile-liu-biao.jpg`。
